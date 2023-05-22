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
using System.Windows.Forms;
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
        private bool? castiEcc, ducksteinEcc;
        private List<object> items = new List<object>();
        private List<object> eccsList = new List<object>();
        private List<object> qVectorStr = new List<object>();
        DataGrid dg = new DataGrid();
        Dictionary<int, List<int>> rowsList = new Dictionary<int, List<int>>();
        QVector qv;


        public void Update(DataTable oldMatrix, bool? Casti, bool? Duckstein)
        {
            items.Clear();
            eccsList.Clear();
            qVector.ItemsSource = null;
            eccentricity.ItemsSource = null;
            this.matrix = oldMatrix;
            castiEcc = Casti;
            ducksteinEcc = Duckstein;
            qAnalysisProcedure();
        }


        private void qAnalysisProcedure() {
            rowsList.Clear();
            rowsList = QAnalysisFunc.GetRowsList(matrix);

            qv = new QVector(rowsList, matrix);

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

            if (!(bool)castiEcc && !(bool)ducksteinEcc)
            {
                eccentricity.Visibility = Visibility.Collapsed;
            }

            for (int i = 0; i < matrix.Rows.Count; i++)
            {
                string data1 = "", data2 = "";
                if ((bool)castiEcc)
                {
                    data1 = "ecc(x" + (i + 1) + ") = " + qv.getCastiEcc(i).ToString("F3");
                }
                if ((bool)ducksteinEcc)
                {
                   data2 = "ecc(x" + (i + 1) + ") = " + qv.getDucksteinEcc(i).ToString("F3");
                }
                eccsList.Add(new { eccCasti = data1, eccDuck =  data2});
            }

            qVectorText.Text = qv.QVectorString().ToString();
            eccentricity.ItemsSource = eccsList;
        }


        public ResultWindow(SettingWindow settingWindow, DataTable matrix, string projectName, bool? Casti, bool? Duckstein)
        {
            this.settingWindow = settingWindow;
            this.matrix = matrix;
            this.WindowState = WindowState.Maximized;
            qv = null;

            castiEcc = Casti;
            ducksteinEcc = Duckstein;

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
            this.projectNameText.Text = projectName;

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

        private void DataGridLoading(object sender, DataGridRowEventArgs e)
        {
            string res = "x" + (e.Row.GetIndex() + 1);
            e.Row.Header = res;
        }

        private void compareBtn(object sender, RoutedEventArgs e)
        {
            CompareWindow compareWindow = new CompareWindow(qv);
            compareWindow.Show();
        }

        private void ExitWindow(object sender, EventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            this.Close();
        }


    }
}
