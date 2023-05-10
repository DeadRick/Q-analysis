using System;
using System.Collections.Generic;
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
    /// Interaction logic for VisualisationWindow.xaml
    /// </summary>
    public partial class VisualisationWindow : Window
    {
        QVector qv;
        private List<object> items = new List<object>();


        public VisualisationWindow()
        {
            InitializeComponent();
            double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            WpfPlot1.Plot.AddScatter(dataX, dataY);
            WpfPlot1.Refresh();
        }

        public VisualisationWindow(QVector qv)
        {
            InitializeComponent();
            this.qv = qv;

            LoadPlot();


            int cnt = 0;
            foreach (var el in qv.finalDict.Keys)
            {
                string data1 = "q = " + (el - 1);
                items.Add(new { DimensionFirstModel = data1, QValueFirstModel = qv.finalDict[el].Count, VectorsFirstModel = qv.GetString(el) });
                cnt++;
            }


            qVectorFirstModel.ItemsSource = items;

        }

        void LoadPlot()
        {
            double[] dataX = qv.QVectorDoubleSize();
            double[] dataY = qv.QVectorDouble();
            WpfPlot1.Plot.XAxis.ManualTickSpacing(1);
            WpfPlot1.Plot.YAxis.ManualTickSpacing(1);
            WpfPlot1.Plot.AddBar(dataY);
            WpfPlot1.Plot.Title(qv.QVectorString().ToString());
            WpfPlot1.Refresh();
        }

        private void FirstModel_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            object selectedItem = qVectorFirstModel.SelectedItem;
            if (selectedItem is null)
            {
                return;
            }
            WpfPlot1.Plot.Clear();
            LoadPlot();
            string? value1 = selectedItem.ToString().Split('=')[2].Split(',')[0];
            string? value2 = selectedItem.ToString().Split('=')[3].Split(',')[0];
            double x = double.Parse(value1);

            double y = double.Parse(value2);

            var arrow = WpfPlot1.Plot.AddArrow(x, y, x, y + 0.1);
            arrow.Color = System.Drawing.Color.Red;
            arrow.ArrowheadWidth = 10;
            arrow.ArrowheadWidth = 10;
            WpfPlot1.Refresh();

        }
    }
}
