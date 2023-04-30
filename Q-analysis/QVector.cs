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
        List<int> numOfRows;
        List<List<string>> finalRows;
        Dictionary<string, string> pathDictionary = new Dictionary<string, string>();
        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
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
                } else
                {
                    if (pathDictionary.ContainsKey(key))
                    {
                        keyValuePairs[pathDictionary[key]] += ';' + value; 
                        continue;
                    } else
                    {
                        if (!pathDictionary.ContainsKey(value))
                        {
                            pathDictionary.Add(value, key);
                            keyValuePairs.Add(key, value);
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
        }
        public int size()
        {
            return dimensionSize;
        }
        public QVector(List<string> lst)
        {
            finalRows = new();
            listOfRows = lst;
            numOfRows = new();
            for (int i = 0; i < listOfRows.Count; i++)
            {
                numOfRows.Add(int.Parse(listOfRows[i][1].ToString()));
            }
            fixList();
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
                string[] values = keyValuePairs[el].Split(';');
                SortedSet<string> sortedStr = new SortedSet<string>();

                foreach (var item in values)
                {
                    sortedStr.Add(item);
                }
                string sortedValue = string.Join(";", sortedStr);

                keyValuePairs[el] = sortedValue;
            }
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < numOfRows.Count; i++)
            {
                yield return numOfRows[i];
            }
        }

        public List<int> getVector()
        {
            return numOfRows;
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
