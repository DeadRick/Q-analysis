using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Q_analysis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string dir = Directory.GetCurrentDirectory() + "\\projects";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        private void NewProjectName(object sender, RoutedEventArgs e)
        {
            NameProjectWindow newPrj = new() { Owner = this };
            Nullable<bool> dialogRes = newPrj.ShowDialog();
            if (dialogRes == true)
            {
                this.Hide();
                
                NewProjectWindow window = new();
                window.Show();
            }
            
        }

        private void BtnExit(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void ExitWindow(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();

        }

        private void openProjectBtn(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedPath = dialog.SelectedPath;
                this.Hide();
                SettingWindow settingWindow = new(selectedPath);
                settingWindow.Show(); // #TODO System.InvalidOperationException: 'Items collection must be empty before using ItemsSource.'

            }
        }
    }
}
