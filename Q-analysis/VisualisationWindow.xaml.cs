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
        QVectorFix qv;

        public VisualisationWindow()
        {
            InitializeComponent();
            double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            WpfPlot1.Plot.AddScatter(dataX, dataY);
            WpfPlot1.Refresh();
        }

        public VisualisationWindow(QVectorFix qv)
        {
            InitializeComponent();
            this.qv = qv;
       
            double[] dataX = qv.QVectorDoubleSize();
            double[] dataY = qv.QVectorDouble();
            WpfPlot1.Plot.AddBar(dataY);
            WpfPlot1.Plot.Title(qv.QVectorString().ToString());
            WpfPlot1.Refresh();
        }
    }
}
