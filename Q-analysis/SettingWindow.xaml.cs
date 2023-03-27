﻿using Microsoft.Win32;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace Q_analysis
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window, INotifyPropertyChanged
    {
        private string path;
        public event PropertyChangedEventHandler? PropertyChanged;

        private void InitializeSettingWindow()
        {
            InitializeComponent();
            //one
            dataGrid.Visibility = Visibility.Hidden;
            this.MatrixSize = Enumerable.Range(1, 20).ToArray();
            this.DataContext = this;
            this.DefaultValue = 1;
            saveBtnBorder.BorderBrush.Opacity = 0.25;
            saveBtn.Background.Opacity = 0.25;
            projectNmae.Text = QAnalysisFunc.GetNameOfProject();
        }

        public SettingWindow()
        {
            InitializeSettingWindow();
        }

        public SettingWindow(string path)
        {
            InitializeSettingWindow();
            matrixMessage.Visibility = Visibility.Hidden;
            this.path = path;
            string[] files = Directory.GetFiles(path, "*.csv");
            if (files.Length != 0)
            {
                LoadProcedure(files[0]);
            }

        }

        public IList MatrixSize { get; private set; }
        public DataTable Matrix { get; set; }

        public int DefaultValue { get; set; }



        private void UpdateSize(object sender, RoutedEventArgs e)
        {
            var sizeN = nSize.Text;
            var sizeM = mSize.Text;
            int nSizeMatrix, mSizeMatrix;
            int.TryParse(sizeN, out nSizeMatrix); //#TODO: Try Parse. Incorrect values
            int.TryParse(sizeM, out mSizeMatrix); //#TODO: Try Parse. Incorrect values
            this.UpdateMatrix(nSizeMatrix, mSizeMatrix);
      
        }

        void UpdateMatrix(int sizeN, int sizeM)
        {
            if (dataGrid.Visibility == Visibility.Hidden)
            {
                dataGrid.Visibility = Visibility.Visible;
                saveBtnBorder.BorderBrush.Opacity = 1;
                saveBtn.Background.Opacity = 1;
            }
            this.Matrix = new DataTable();
            this.DefaultValue = int.Parse(defaultValue.Text); // #TODO: Parse. Incorrect value
            for (var i = 0; i < sizeM; i++)
                Matrix.Columns.Add(new DataColumn("c" + i, typeof(int)));
            for (var i = 0; i < sizeN; i++)
            {
                var r = Matrix.NewRow();
                for (var c = 0; c < sizeM; c++)
                    r[c] = DefaultValue;
                Matrix.Rows.Add(r);
            }
            // this.Matrix = dt.DefaultView;
            matrixMessage.Visibility = Visibility.Hidden;
            PropertyChanged(this, new PropertyChangedEventArgs("Matrix"));
        }
            
        void UpdateMatrix(int sizeN, int sizeM, string[] arr)
        {
            if (dataGrid.Visibility == Visibility.Hidden)
            {
                dataGrid.Visibility = Visibility.Visible;
                saveBtnBorder.BorderBrush.Opacity = 1;
                saveBtn.Background.Opacity = 1;
            }
            this.Matrix = new DataTable();
            this.DefaultValue = int.Parse(defaultValue.Text); // #TODO: Parse. Incorrect value
            for (var i = 0; i < sizeM; i++)
                Matrix.Columns.Add(new DataColumn("c" + i, typeof(int)));
            for (var i = 0; i < sizeN; i++)
            {
                var r = Matrix.NewRow();
                for (var c = 0; c < sizeM; c++)
                {
                    int num;
                    var arrSplit = arr[i].Split(',');
                    int.TryParse(arrSplit[c], out num);
                    r[c] = num;
                }
                Matrix.Rows.Add(r);
            }
            if (PropertyChanged != null)
            {
                matrixMessage.Visibility = Visibility.Hidden;
                PropertyChanged(this, new PropertyChangedEventArgs("Matrix"));
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new();
              
            mw.Show();
            this.Close();
        }

        // Click event to save a matrix
        private async void SaveMatrix(object sender, RoutedEventArgs e)
        {
            // #TODO Change to the current project
            // #TODO Add HINT for unvisible matrix
            if (Matrix is null)
            {
                return;
            }
            string dir = Directory.GetCurrentDirectory() + "\\projects\\" + QAnalysisFunc.GetNameOfProject();
            using StreamWriter file = new(dir + "\\" + QAnalysisFunc.GetNameOfProject() + ".csv");
            var dt = (DataTable)(this.Matrix);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    int number;
                    // Null elements is Default value
                    if (dt.Rows[i].IsNull(j))
                    {
                        number = DefaultValue;
                    }
                    else
                    {
                        number = dt.Rows[i].Field<int>(j);
                    }
                    if (j != dt.Columns.Count - 1)
                    {
                        await file.WriteAsync(number.ToString() + ',');
                    }
                    else
                    {
                        await file.WriteAsync(number.ToString() + "\n");
                    }
                }
            }
        }

        private void LoadFile(string[] file)
        {
            int sizeN = file.Length;
            int sizeM = file[0].ToString().Split(',').Length;

            nSize.Text = sizeN.ToString();
            mSize.Text = sizeM.ToString();

            UpdateMatrix(sizeN, sizeM, file);
        }

        // Load Matrix from the file
        private void LoadProcedure(OpenFileDialog ofd)
        {
            var file = File.ReadAllLines(ofd.FileName);
            LoadFile(file);
        }

        private void LoadProcedure(string file)
        {
            var fileData = File.ReadAllLines(file);
            LoadFile(fileData);
        }

        // Click event for loading files
        private void loadBtn(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new();
            if(ofd.ShowDialog() == true)
            {
                LoadProcedure(ofd);
            }

            
        }
    }
}
