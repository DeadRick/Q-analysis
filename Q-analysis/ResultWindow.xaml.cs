using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Q_analysis
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        private SettingWindow settingWindow;
        public DataTable matrix;
        private List<object> items = new List<object>();
        DataGrid dg = new DataGrid();
        Dictionary<int, QVector> countDict = new Dictionary<int, QVector>();

        public void Update(DataTable oldMatrix)
        {
            items.Clear();
            countDict.Clear();
            qVector.ItemsSource = null;
            this.matrix = oldMatrix;

            qAnalysisProcedure();
        }
        private void qAnalysisProcedure() {
            int count = 0;
            for (int i = 0; i < matrix.Rows.Count; i++)
            {
                QVector qVector = new QVector();
                count = 0;
                for (int j = 0; j < matrix.Columns.Count; j++)
                {
                    DataRow row = matrix.Rows[i];
                    int element = Convert.ToInt32(row["c" + j.ToString()]);
                    if (element == 1)
                    {
                        count++;
                    }
                }


                for (int k = count; k > 0; k--)
                {
                    if (countDict.ContainsKey(k))
                    {
                        countDict[k].Add("r" + (i + 1));
                    } else
                    {
                        countDict.Add(k, new QVector("r" + (i + 1)));
                    }
                }
        
            }

            qVectorSort();

            visualMatrix.ItemsSource = matrix.DefaultView;
            qVector.ItemsSource = items;


            foreach (var el in countDict)
            {
                items.Add(new { Dimension = el.Key - 1, QValue = el.Value.size(), Vectors = el.Value });

            }
        }

        public void qVectorSort()
        {
            Dictionary<int, QVector> tempDict = new();
            foreach (var el in countDict)
            {
                int countOne = el.Key;
                List<int> numRows = el.Value.getVector();
                for (int k = 0; k < numRows.Count; k++)
                {
                    for (int j = k; j < numRows.Count; j++)
                    {
                        if (numRows[k] != numRows[j])
                        {
                            DataRow rowF = matrix.Rows[numRows[k] - 1];
                            DataRow rowS = matrix.Rows[numRows[j] - 1];
                            int rowSize = rowF.ItemArray.Length;

                            int count = 0;

                            for (int i = 0; i < rowSize; i++)
                            {
                                int el1 = Convert.ToInt32(rowF["c" + i.ToString()]);
                                int el2 = Convert.ToInt32(rowS["c" + i.ToString()]);

                                if (el1 == 1 && el2 == 1)
                                {
                                    count++;
                                }
                            }

                            if (count >= countOne)
                            {
                                if (tempDict.ContainsKey(el.Key))
                                {
                                    tempDict[el.Key].Add("r" + numRows[k] + ";r" + numRows[j]);
                                }
                                else
                                {
                                    tempDict.Add(el.Key, new QVector("r" + numRows[k] + ";r" + numRows[j]));
                                }

                            }
                        }
                    }
                }
            }

            foreach (var el in countDict)
            {
                foreach (var it in el.Value)
                {
                    if (tempDict.ContainsKey(el.Key))
                    {

                        tempDict[el.Key].Add("r" + it);
                    }
                    else
                    {
                        tempDict.Add(el.Key, new QVector("r" + it));
                    }
                }
            }
            var sortedDict = tempDict.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            //sortedDict[1] = new QVector("all");
            countDict = sortedDict;
        }

        public ResultWindow(SettingWindow settingWindow, DataTable matrix)
        {
            this.settingWindow = settingWindow;
            this.matrix = matrix;
            this.matrix.Rows[0].Table.Columns[0].ColumnName = "c123";

            dg.ItemsSource = matrix.DefaultView;
            //dg.RowHeaderVisible = true;
            dg.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
            dg.IsReadOnly = true;

            for (int i = 0; i < dg.Items.Count; i++)
            {
                var row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(i);
                if (row != null)
                {
                    row.Header = "r" + i;
                }
            }
         
            {

            }
            dg.HeadersVisibility = DataGridHeadersVisibility.None;

            InitializeComponent();
            qAnalysisProcedure();

        }

        private void check(object sender, RoutedEventArgs e)
        {
            this.Hide();
            settingWindow.Show();
        }
    }
}
