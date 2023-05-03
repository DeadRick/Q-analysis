using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace Q_analysis
{
    internal class QVector : IEnumerable
    {
        List<string> listOfRows;
        SortedSet<int> numOfRows;
        List<List<string>> finalRows;
        Dictionary<string, string> pathDictionary = new Dictionary<string, string>();
        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
        SortedSet<string> visited = new SortedSet<string>();
        int dimensionSize = 0;
        private void fixList()
        {
            keyValuePairs.Clear();
            pathDictionary.Clear();
            foreach (var el in listOfRows)
            {
                string[] splitEl = el.Split(';');
                if (splitEl.Length == 1)
                {
                    if (visited.Contains(splitEl[0])) {
                        continue;
                    }
                    if (keyValuePairs.ContainsKey(splitEl[0]))
                    {
                        
                        keyValuePairs[splitEl[0]] = "";
                    } else
                    {
                        keyValuePairs[splitEl[0]] = "";
                    }
                    continue;
                }
                string key = splitEl[0];
                string value = splitEl[1];
                if (keyValuePairs.ContainsKey(key))
                {
                    if (pathDictionary.ContainsKey(value))
                    {
                        continue;
                    }
                    string str = keyValuePairs[key];
                    keyValuePairs[key] = str + ';' + value;
                    pathDictionary[value] = key;
                    visited.Add(value);
                } else
                {
                    if (pathDictionary.ContainsKey(value))
                    {
                        keyValuePairs[pathDictionary[value]] += ";" + key;
                        pathDictionary[key] = pathDictionary[value];
                        visited.Add(value);
                        continue;
                    }
                    if (pathDictionary.ContainsKey(key))
                    {
                        keyValuePairs[pathDictionary[key]] += ';' + value;
                        pathDictionary[value] = pathDictionary[key];
                        continue;
                    } else
                    {
                        if (!pathDictionary.ContainsKey(value))
                        {
                            pathDictionary.Add(value, key);
                            keyValuePairs.Add(key, value);
                            visited.Add(value);
                        }
                        else
                        {
                            keyValuePairs[pathDictionary[value]] += ";" + key;
                        }
                    }
                }
            }
            dimensionSize = keyValuePairs.Count;
            sortDict();
        
        }

        public QVector()
        {
            listOfRows = new();
            numOfRows = new();
            finalRows = new();
        }
        
        public QVector(string el)
        {
            bool check = true;
            listOfRows = new();
            numOfRows = new();
            finalRows = new();
            foreach (var item in listOfRows)
            {
                if (item.Contains(el[1]) && el.Length == 2)
                {
                    check = false;
                    break;
                }
            }
            if (check)
            {
                if (!listOfRows.Contains("r" + el[1]))
                {
                    listOfRows.Add(el);
                }
                if (el.Any(char.IsDigit))
                {
                    var splitEl = el.Split(';');
                    foreach (var row in splitEl)
                    {
                        numOfRows.Add(int.Parse(row.Substring(1)));
                    }

                }
            }
            fixList();

        }
        public int size()
        {
            return dimensionSize;
        }
        public QVector(List<string> lst)
        {
            finalRows = new();
            listOfRows = lst;
            fixList();
            numOfRows = new();
            for (int i = 0; i < listOfRows.Count; i++)
            {
                numOfRows.Add(int.Parse(listOfRows[i][1].ToString()));
            }
        }
        
        public void Add(string el)
        {
            bool check = true;
            foreach (var item in listOfRows)
            {
                if (item.Contains(el[1]) && el.Length == 2)
                {
                    check = false;
                    break;
                }
            }
            if (check)
            {
                listOfRows.Add(el);
                if (el.Any(char.IsDigit))
                {
                    var splitEl = el.Split(';');
                    foreach (var row in splitEl)
                    {
                        numOfRows.Add(int.Parse(row.Substring(1)));
                    }

                }
            }
            fixList();
        }

        private void sortDict()
        {
            foreach (var el in keyValuePairs.Keys) 
            {
                string[] values;
                if (keyValuePairs[el].Contains(';'))
                {
                   values = keyValuePairs[el].Split(';');
                } else
                {
                    keyValuePairs[el] = "";
                    continue;
                }
                SortedSet<int> sortedStr = new SortedSet<int>();

                foreach (var item in values)
                {
                    if (item == "")
                        continue;

                    string res;
                    if (item[item.Length - 1] == 'r')
                    {
                        res = item.Substring(0, item.Length - 1);
                        sortedStr.Add(int.Parse(res.Substring(1)));
                    }
                    else
                    {
                        sortedStr.Add(int.Parse(item.Substring(1)));
                    }
                }
                string sortedValue = "";
                if (sortedStr.Count == 1)
                {
                    sortedValue = "r" + sortedStr.ElementAt(0);
                } else
                {
                    sortedValue += 'r';
                    sortedValue = string.Join(";", sortedStr.Select(x => "r" + x));
                }
                
           
                keyValuePairs[el] = sortedValue;
            }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var item in numOfRows)
            {
                yield return item;
            }
        }

        public List<int> getVector()
        {

            List<int> nums = new();
            foreach (var item in numOfRows)
            {
                nums.Add(item);
            }
            return nums;
        }
        

        public override string ToString()
        {
            fixList();
            string res = "";
            int size = keyValuePairs.Count;
            int cnt = 0;
            foreach (var key in keyValuePairs.Keys)
            {
                cnt++;
                if (keyValuePairs[key] == "")
                {
                    if (cnt != size)
                    {
                        res = res + " {" + key + "}, ";

                    } else
                    {
                        res = res + " {" + key + "}";
                    }
                }
                else
                {
                    res = res + " {" + key + ',' + keyValuePairs[key].Replace(';', ',') + "}";
                    if (cnt != size)
                    {
                        res += ", ";
                    }
                    else
                    {
                        res += " ";
                    }
                }
                
            }    
            return res;
        }
    }
}
