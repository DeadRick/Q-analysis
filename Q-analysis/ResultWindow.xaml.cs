using System;
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
        private DataTable matrix;


        public ResultWindow(SettingWindow settingWindow, DataTable matrix)
        {
            InitializeComponent();
            this.settingWindow = settingWindow;
            this.matrix = matrix;
        }

        private void check(object sender, RoutedEventArgs e)
        {
            this.Hide();
            settingWindow.Show();
        }
    }
}
