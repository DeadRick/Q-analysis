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
        private List<object> eccsList = new List<object>();
        private List<object> qVectorStr = new List<object>();
        DataGrid dg = new DataGrid();
        Dictionary<int, QVector> countDict = new Dictionary<int, QVector>();
        Dictionary<int, List<int>> rowsList = new Dictionary<int, List<int>>();
        QVectorFix qv;


        public void Update(DataTable oldMatrix)
        {
            items.Clear();
            eccsList.Clear();
            countDict.Clear();
            qVector.ItemsSource = null;
            eccentricity.ItemsSource = null;
            this.matrix = oldMatrix;

            qAnalysisProcedure();
        } 

        private void qAnalysisProcedure() {
            rowsList.Clear();
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
                    } else
                    {
                        rowsList[k] = new List<int>() { i };
                    }
                }
        
            }

            qv = new QVectorFix(rowsList, matrix);

            //qVectorSort();

            visualMatrix.ItemsSource = matrix.DefaultView;

            int cnt = 0;
            foreach (var el in qv.finalDict.Keys)
            {
                string data1 = "q = " + (el - 1);
                items.Add(new { Dimension = data1, QValue = qv.finalDict[el].Count, Vectors = qv.GetString(el) });
                cnt++;  
            }

            qVector.ItemsSource = items;

            for (int i = 0; i < matrix.Rows.Count; i++)
            {
                string data1 = "ecc(x" + (i + 1) + ") = " + qv.getCastiEcc(i).ToString();
                string data2 = "ecc(x" + (i + 1) + ") = " + qv.getDucksteinEcc(i).ToString();
                eccsList.Add(new { eccCasti = data1, eccDuck =  data2});
            }

            qVectorText.Text = qv.QVectorString().ToString();
            eccentricity.ItemsSource = eccsList;
        }


        public ResultWindow(SettingWindow settingWindow, DataTable matrix)
        {

            this.settingWindow = settingWindow;
            this.matrix = matrix;
            this.WindowState = WindowState.Maximized;
            qv = null;

            dg.ItemsSource = matrix.DefaultView;
            //dg.RowHeaderVisible = true;
            dg.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
            dg.IsReadOnly = true;

            for (int i = 0; i < dg.Items.Count; i++)
            {
                var row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(i);
                if (row != null)
                {
                    row.Header = "x" + i;
                }
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

        private void visualBtn(object sender, RoutedEventArgs e)
        {
            VisualisationWindow vw = new(qv);
            vw.Show();
        }
    }
}
