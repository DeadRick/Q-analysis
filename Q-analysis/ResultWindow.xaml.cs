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
        private int[] myArray = new int[] {1, 2, 3, 4, 5 };
        private List<object> items = new List<object>();

        public void Update(DataTable oldMatrix)
        {
            items.Clear();
            qVector.ItemsSource = null;
            this.matrix = oldMatrix;
            qAnalysisProcedure();
        }
        private void qAnalysisProcedure() {
            int count = 0;

            for (int i = 0; i < matrix.Rows.Count; i++)
            {
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
                items.Add(new { Index = i, Value = count });
            }


            qVector.ItemsSource = items;

        }
        public ResultWindow(SettingWindow settingWindow, DataTable matrix)
        {
            InitializeComponent();
            this.settingWindow = settingWindow;
            this.matrix = matrix;

            //for (int i = 0; i < myArray.Length; i++)
            //{
            //    items.Add(new { Index = i, Value = myArray[i] });
            //}

            //qVector.ItemsSource = items;
            qAnalysisProcedure();
        }

        private void check(object sender, RoutedEventArgs e)
        {
            this.Hide();
            settingWindow.Show();
        }
    }
}
