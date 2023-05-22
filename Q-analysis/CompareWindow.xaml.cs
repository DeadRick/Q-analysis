using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ListViewItem = System.Windows.Controls.ListViewItem;
using File = System.IO.File;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using System.Data;
using ScottPlot;

namespace Q_analysis
{
    /// <summary>
    /// Interaction logic for CompareWindow.xaml
    /// </summary>
    public partial class CompareWindow : Window
    {
        QVector qvFirst;
        QVector qvSecond;
        private List<object> items = new List<object>();
        List<ListViewItem> items_new = new List<ListViewItem>();
        Dictionary<int, List<int>> rowsListSecondModel = new Dictionary<int, List<int>>();
        public DataTable MatrixSecondModel { get; set; }


        public CompareWindow(QVector qv)
        {
            InitializeComponent();
            this.qvFirst = qv;
            WpfPlot1.Configuration.LockHorizontalAxis = true;
            WpfPlot1.Configuration.LockVerticalAxis = true;
            WpfPlot2.Configuration.LockHorizontalAxis = true;
            WpfPlot2.Configuration.LockVerticalAxis = true;

            LoadFirstPlot();

            int cnt = 0;
            foreach (var el in qv.finalDict.Keys)
            {
                string data1 = "q = " + (el - 1);
                items.Add(new { DimensionFirstModel = data1, QValueFirstModel = qv.finalDict[el].Count, VectorsFirstModel = qv.GetString(el) });
                cnt++;
            }


            qVectorFirstModel.ItemsSource = items;

            //qVectorFirstModel.MouseDoubleClick += new MouseEventHandler(ListViewFirstModel_DoubleClick);
        }

        void LoadFirstPlot()
        {
            double[] dataX = qvFirst.QVectorDoubleSize();
            double[] dataY = qvFirst.QVectorDouble();
            //WpfPlot1.Plot.XAxis.TickLabelFormat("D6", false);
            WpfPlot1.Plot.XAxis.ManualTickSpacing(1);
            WpfPlot1.Plot.YAxis.ManualTickSpacing(1);


            WpfPlot1.Plot.AddBar(dataY);
            WpfPlot1.Plot.Title(qvFirst.QVectorString().ToString());
            WpfPlot1.Refresh();

        }

        void LoadSecondPlot()
        {
            items_new.Clear();
            WpfPlot2.Plot.Clear();
            int cnt = 0;
            foreach (var el in qvSecond.finalDict.Keys)
            {
                string data1 = "q = " + (el - 1);
                ListViewItem oneItem = new ListViewItem();
                if (qvFirst.finalDict.ContainsKey(el))
                {
                    if (qvFirst.finalDict[el].Count != qvSecond.finalDict[el].Count)
                    {
                        oneItem.Foreground = Brushes.Green;
                    } 
                    else    
                    {

                        if (qvFirst.GetString(el).ToString() != qvSecond.GetString(el).ToString())
                        {
                            oneItem.Foreground = Brushes.Orange;
                        }
                    }
                }
  
                var r = new { DimensionSecondModel = data1, QValueSecondModel = qvSecond.finalDict[el].Count, VectorsSecondModel = qvSecond.GetString(el) };
                oneItem.Content = r;

                items_new.Add(oneItem);
                qVectorSecondModel.ItemsSource = items_new;
                cnt++;
            }


            qVectorSecondModel.Items.Refresh();

            LoadSecondPlotProcedure();

        }

        private void LoadSecondPlotProcedure()
        {
            double[] dataY = qvSecond.QVectorDouble();
            WpfPlot2.Plot.AddBar(dataY);
            WpfPlot2.Plot.Palette = ScottPlot.Palette.Category10;
            WpfPlot2.Plot.Title(qvSecond.QVectorString().ToString());
            WpfPlot2.Plot.XAxis.ManualTickSpacing(1);
            WpfPlot2.Plot.YAxis.ManualTickSpacing(1);
            WpfPlot2.Refresh();
        }

        private void FirstModel_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            object selectedItem = qVectorFirstModel.SelectedItem;   
            if (selectedItem is null)
            {
                return;
            }
            WpfPlot1.Plot.Clear();
            LoadFirstPlot();
            string? value1 = selectedItem.ToString().Split('=')[2].Split(',')[0];
            string? value2 = selectedItem.ToString().Split('=')[3].Split(',')[0];
            double x = double.Parse(value1);

            //x = (qvFirst.QVectorDouble().Length - 1) - x;
            double y = double.Parse(value2);

            var arrow = WpfPlot1.Plot.AddArrow(x, y, x, y + 0.1);
            arrow.Color = System.Drawing.Color.Red;
            arrow.ArrowheadWidth = 10;
            arrow.ArrowheadWidth = 10;
            WpfPlot1.Refresh();

        }

        private void SecondModel_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            object selectedItem = qVectorSecondModel.SelectedItem;
            if (selectedItem is null)
            {
                return;
            }
            WpfPlot2.Plot.Clear();
            LoadSecondPlotProcedure();
            string? value1 = selectedItem.ToString().Split('=')[2].Split(',')[0];
            string? value2 = selectedItem.ToString().Split('=')[3].Split(',')[0];
            double x = double.Parse(value1);

            double y = double.Parse(value2);

            var arrow = WpfPlot2.Plot.AddArrow(x, y, x, y + 0.1);
            arrow.Color = System.Drawing.Color.Red;
            arrow.ArrowheadWidth = 10;
            arrow.ArrowheadWidth = 10;
            WpfPlot2.Refresh();
        }
        private void loadSecondBtn(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                var fileData = File.ReadAllLines(ofd.FileName);

                int sizeN = fileData.Length;
                int sizeM = fileData[0].ToString().Split(',').Length;

                this.MatrixSecondModel = new DataTable();

                for (var i = 0; i < sizeM; i++)
                    MatrixSecondModel.Columns.Add(new DataColumn("y" + (i + 1), typeof(int)));
                for (var i = 0; i < sizeN; i++)
                {
                    var r = MatrixSecondModel.NewRow();
                    for (var c = 0; c < sizeM; c++)
                    {
                        int num;
                        var arrSplit = fileData[i].Split(',');
                        int.TryParse(arrSplit[c], out num);
                        r[c] = num;
                    }
                    MatrixSecondModel.Rows.Add(r);
                }
                rowsListSecondModel = QAnalysisFunc.GetRowsList(MatrixSecondModel);
                this.qvSecond = new QVector(rowsListSecondModel, MatrixSecondModel);
                LoadSecondPlot();
            }
        }

        private void closeWindowBtn(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void questionMarkBtnn(object sender, RoutedEventArgs e)
        {
            HelpModalWindow hmw = new("The simplices that differ in size are marked in green. \nThe simplices that do not differ in size but have different components are marked in orange.");
            hmw.Show();
        }
    }
}
    
