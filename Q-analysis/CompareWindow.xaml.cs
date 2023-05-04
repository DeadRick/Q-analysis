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

namespace Q_analysis
{
    /// <summary>
    /// Interaction logic for CompareWindow.xaml
    /// </summary>
    public partial class CompareWindow : Window
    {
        QVectorFix qv;
        private List<object> items = new List<object>();
        Color bckgColor;

        public CompareWindow()
        {
            InitializeComponent();
            double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            WpfPlot1.Plot.AddScatter(dataX, dataY);
            WpfPlot1.Refresh();
        }

        public CompareWindow(QVectorFix qv)
        {
            InitializeComponent();
            this.qv = qv;

            LoadFirstPlot();
            
            int cnt = 0;
            foreach (var el in qv.finalDict.Keys)
            {
                string data1 = "q = " + (el - 1);
                items.Add(new { DimensionFirstModel = data1, QValueFirstModel = qv.finalDict[el].Count, VectorsFirstModel= qv.GetString(el) });
                cnt++;
            }

            qVectorFirstModel.ItemsSource = items;

            //qVectorFirstModel.MouseDoubleClick += new MouseEventHandler(ListViewFirstModel_DoubleClick);
        }

        void LoadFirstPlot()
        {
            double[] dataX = qv.QVectorDoubleSize();
            double[] dataY = qv.QVectorDouble();
            WpfPlot1.Plot.AddBar(dataY);
            WpfPlot1.Plot.Title(qv.QVectorString().ToString());
            WpfPlot1.Refresh();

        }

        void LoadSecondPlot()
        {
            double[] dataX = qv.QVectorDoubleSize();
            double[] dataY = qv.QVectorDouble();
            WpfPlot2.Plot.AddBar(dataY);
            WpfPlot2.Refresh();

        }

        private void FirstModel_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            object r = qVectorFirstModel.SelectedItem;
            WpfPlot1.Plot.Clear();
            LoadFirstPlot();
            string value1 = r.ToString().Split('=')[2].Split(',')[0];
            string value2 = r.ToString().Split('=')[3].Split(',')[0];
            double x = double.Parse(value1);
            
            x = (qv.QVectorDouble().Length - 1) - x;
            double y = double.Parse(value2);
            testblock.Text = x.ToString();

            var arrow = WpfPlot1.Plot.AddArrow(x, y, x, y + 1);
            WpfPlot1.Refresh();

        }
    }

    public partial class ItemVM

}
