using System;
using System.Windows;

namespace Q_analysis
{
    /// <summary>
    /// Interaction logic for NewProjectWindow.xaml
    /// </summary>
    public partial class NewProjectWindow : Window
    {
        public bool isClosing = false;
        public NewProjectWindow()
        {
            InitializeComponent();
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            if (isClosing)
            {
                isClosing = true;
            }
            this.Close();
            MainWindow mv = new();
            mv.Show();
        }

        private void BackToMenu(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BinaryMatrixBtn(object sender, RoutedEventArgs e)
        {
            this.Hide();

            bool condition = true;
            SettingWindow settingWindow = new(true);
            settingWindow.Show();
        }
    }
}
