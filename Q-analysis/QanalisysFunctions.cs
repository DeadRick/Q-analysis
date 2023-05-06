using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Q_analysis
{
    internal class QAnalysisFunc
    {

        internal static string GetNameOfProject() // #TODO Change THIS METHODE!
        {
            return File.ReadAllText(Environment.CurrentDirectory+ "\\qanalysis_info.txt");
        }

        internal static Dictionary<int, List<int>> GetRowsList(DataTable matrix)
        {
            Dictionary<int, List<int>> rowsList = new Dictionary<int, List<int>>();
            int count = 0;
            for (int i = 0; i < matrix.Rows.Count; i++)
            {
                count = 0;
                for (int j = 0; j < matrix.Columns.Count; j++)
                {
                    DataRow row = matrix.Rows[i];
                    int element = Convert.ToInt32(row["y" + (j + 1)]);
                    if (element == 1)
                    {
                        count++;
                    }
                }


                for (int k = count; k > 0; k--)
                {
                    if (rowsList.ContainsKey(k))
                    {
                        rowsList[k].Add(i);
                    }
                    else
                    {
                        rowsList[k] = new List<int>() { i };
                    }
                }

            }
            return rowsList;
        }
    }
}
