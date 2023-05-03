using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Q_analysis
{
    public class QVectorFix
    {
        private Dictionary<int, List<int>> rowsList;
        private DataTable matrix;
        private Dictionary<int, int> pathDict = new Dictionary<int, int>();
        private Dictionary<int, SortedSet<int>> mergedDict = new Dictionary<int, SortedSet<int>>();
        public Dictionary<int, List<List<int>>> finalDict = new Dictionary<int, List<List<int>>>();

        public QVectorFix(Dictionary<int, List<int>> rowsList, DataTable matrix)
        {
            this.rowsList = rowsList;
            this.matrix = matrix;
            procedure();
        }

        public void procedure()
        {
            foreach (var val in rowsList.Keys)
            {
                pathDict.Clear();
                mergedDict.Clear();
             

                SortedSet<int> sortValues = new SortedSet<int>(rowsList[val]);
                List<int> values = new List<int>(sortValues);
                SortedSet<int> visited = new();
                for (int i = 0; i < values.Count; i++)
                {
                    DataRow firstRow = matrix.Rows[values[i]];
                    int rowSize = firstRow.ItemArray.Length;
                    for (int j = i; j < values.Count; j++)
                    {
                        if (i == j)
                            continue;


                        DataRow secondRow = matrix.Rows[values[j]];
                        int count = 0;
                        for (int k = 0; k < rowSize; k++)
                        {
                            int el1 = Convert.ToInt32(firstRow["y" + (k + 1)]);
                            int el2 = Convert.ToInt32(secondRow["y" + (k + 1)]);

                            if (el1 == 1 && el2 == 1)
                            {
                                count++;
                            }
                        }

                        if (count >= val)
                        {
                            int firstValue = values[i];
                            int secondValue = values[j];
                            visited.Add(secondValue);
                            visited.Add(firstValue);

                            if (mergedDict.ContainsKey(values[i]))
                            {
                                mergedDict[firstValue].Add(secondValue);
                                pathDict[secondValue] = firstValue;
                            }
                            else
                            {
                                if (pathDict.ContainsKey(secondValue))
                                {
                                    mergedDict[pathDict[secondValue]].Add(firstValue);
                                    pathDict[firstValue] = pathDict[secondValue];
                                }
                                else
                                {
                                    if (pathDict.ContainsKey(firstValue))
                                    {
                                        mergedDict[pathDict[firstValue]].Add(secondValue);
                                        pathDict[secondValue] = pathDict[firstValue];
                                    }
                                    else
                                    {
                                        SortedSet<int> newSet = new();
                                        newSet.Add(secondValue);
                                        mergedDict.Add(firstValue, newSet);
                                        pathDict[secondValue] = firstValue;
                                    }
                                }
                            }
                        }
                    }

                }
                foreach (var check in values)
                {
                    if (!visited.Contains(check))
                    {
                        if (!mergedDict.ContainsKey(check))
                            mergedDict.Add(check, new SortedSet<int>());
                    }
                }


                if (values.Count != visited.Count)
                {
                    foreach (var pair in mergedDict.Keys)
                    {
                    
                        if (finalDict.ContainsKey(val))
                        {
                            List<int> tempList = new List<int>();
                            tempList.Add(pair);
                            foreach (var el in mergedDict[pair])
                            {
                                tempList.Add(el);
                            }
                            finalDict[val].Add(tempList);
                        }
                        else
                        {
                            List<int> tempList = new List<int>();
                            tempList.Add(pair);
                            foreach (var el in mergedDict[pair])
                            {
                                tempList.Add(el);
                            }
                            finalDict.Add(val, new List<List<int>>() { tempList });
                        }
                    }
                } else
                {
                    if (mergedDict.Count == 1)
                    {
                        finalDict.Add(val, new List<List<int>>() { new List<int>(visited) });
                    } else
                    {
                        foreach (var item in mergedDict.Keys)
                        {
                            mergedDict[item].Add(item);
                            if (finalDict.ContainsKey(val))
                            {
                                finalDict[val].Add(new List<int>(mergedDict[item]));

                            } else
                            {
                                finalDict.Add(val, new List<List<int>> { new List<int>(mergedDict[item]) });

                            }
                        }
                    }
                }
            }
            finalDict.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }

        public double getDucksteinEcc(int row)
        {
            double qMax = finalDict.First().Key;
            double qSum = 0;
            int rowPositon = -1;

            foreach (var item in finalDict.Keys)
            {
                if (rowPositon != -1)
                    break;
                foreach (var el in finalDict[item])
                {
                    if (rowPositon != -1)
                        break;

                    if (el.Contains(row))
                    {
                        rowPositon = item;
                    }
                }
            }

            foreach (var item in finalDict.Keys)
            {
                if (item <= rowPositon)
                {
                    foreach (var el in finalDict[item])
                    {
                        if (el.Contains(row))
                        {
                            double qI = el.Count;
                            double face = item - 1;
                            qSum += (face / qI);
                        }
                    }
                }
            }
            double res = (2 * qSum) / (qMax * (qMax - 1));
            return res;

        }

        public double getCastiEcc(int row)
        {
            double q1 = -1, q2 = -1;

            foreach (var item in finalDict.Keys)
            {
                if (q1 != -1 && q2 != -1)
                {
                    break;
                }
                foreach (var el in finalDict[item])
                {
                  
                    if (el.Contains(row) && q1 == -1)
                    {
                        q1 = item - 1;
                    }

                    if (el.Count > 1 && el.Contains(row) && q2 == -1)
                    {
                        q2 = item - 1;
                    }
                }
            }

            if (q1 == -1 && q2 == -1)
            {
                return 0;
            }
            return ((q1 - q2) / (q2 + 1));
        }

        public StringBuilder GetString(int dim)
        {
            StringBuilder res = new();
            foreach (var item in finalDict[dim])
            {
                res.Append("{");
                for (int i = 0; i < item.Count; i++)
                {
            
                    if (i == item.Count - 1)
                    {
                        res.Append("x" + (item[i] + 1));
                    } else
                    {
                        res.Append("x" + (item[i] + 1) + ", ");
                    }
                }
                res.Append("} ");
            }

            return res;
        }

        public StringBuilder QVectorString()
        {
            StringBuilder res = new StringBuilder();
            res.Append("Q-Vector = ( ");
            foreach (var item in finalDict.Keys)
            {
                res.Append(finalDict[item].Count + " ");
            }
            res.Append(")");
            return res;
        }

        public double[] QVectorDouble()
        {
            double[] res = new double[finalDict.Count];

            int i = 0;
            foreach (var item in finalDict.Keys) 
            {
                res[i] = (double)finalDict[item].Count;
                i++;
            }

            return res;
        }

        public double[] QVectorDoubleSize()
        {
            double[] res = new double[finalDict.Count];

            int i = 0;
            foreach (var item in finalDict.Keys)
            {
                res[i] = i;
                i++;
            }

            return res;
        }
    }
}
