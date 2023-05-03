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
    /// Interaction logic for HelpModalWindow.xaml
    /// </summary>
    public partial class HelpModalWindow : Window
    {
        public HelpModalWindow()
        {
            this.ResizeMode = ResizeMode.NoResize;
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
