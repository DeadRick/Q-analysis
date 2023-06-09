﻿using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
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
        private bool isBinaryMatrix;
        private string directoryPath;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void InitializeSettingWindow()
        {
            InitializeComponent();
            //one
            dataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;   
            dataGrid.Visibility = Visibility.Hidden;
           
            this.MatrixSize = Enumerable.Range(1, 20).ToArray();
            this.DataContext = this;
            this.DefaultValue = 1;
            saveBtnBorder.BorderBrush.Opacity = 0.25;
            saveBtn.Background.Opacity = 0.25;
            projectName.Text = QAnalysisFunc.GetNameOfProject();
            PathInit();

        }

        public SettingWindow()
        {
            InitializeSettingWindow();
            PathInit();
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
            if (files.Length != 0)
            {
                string[] splitPath = files[0].Split('\\');
                projectName.Text = splitPath[splitPath.Length - 1].Split('.')[0];
            }
            PathInit();

        }

        public SettingWindow(bool v)
        {
            InitializeSettingWindow();
            if (v)
            {
                HideSlicingOptions();
            }
            PathInit();
        }

        private void PathInit()
        {
            string dir = Directory.GetCurrentDirectory() + "\\projects" + "\\" + projectName.Text;
            directoryPath = dir;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        private void HideSlicingOptions()
        {
            slicingText.Visibility = Visibility.Collapsed;
            slicingValue.Visibility = Visibility.Collapsed;
            slicingButtons.Visibility = Visibility.Collapsed;
        }

        public IList MatrixSize { get; private set; }
        public DataTable Matrix { get; set; }
        public DataTable OldMatrix { get; set; }

        public int DefaultValue { get; set; }
        ResultWindow resultWindow { get; set; }


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

            if (defaultValueOne.IsChecked == true)
            {
                DefaultValue = 1;
            } else
            {
                DefaultValue = 0;
            }
            for (var i = 0; i < sizeM; i++)
                Matrix.Columns.Add(new DataColumn("y" + (i + 1), typeof(int)));
            for (var i = 0; i < sizeN; i++)
            {
                var r = Matrix.NewRow();
                for (var c = 0; c < sizeM; c++)
                {
                    r[c] = DefaultValue;
                }
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
            if (defaultValueOne.IsChecked == true)
            {
                DefaultValue = 1;
            }
            else
            {
                DefaultValue = 0;
            }
            for (var i = 0; i < sizeM; i++)
                Matrix.Columns.Add(new DataColumn("y" + (i + 1), typeof(int)));
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
            this.Close();
        }

        private void ShowAlertText(string info, bool isWrong)
        {
            loadMessage.Visibility = Visibility.Visible;
            loadMessage.Text = info;
            if (isWrong)
            {
                loadMessage.Foreground = System.Windows.Media.Brushes.Tomato;
            }
            else
            {
                loadMessage.Foreground = System.Windows.Media.Brushes.LightGreen;
            }
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += (sender, e) =>
            {
                loadMessage.Visibility = Visibility.Hidden;
                timer.Stop();
            };
            timer.Start();
        }

        private async void SaveMatrixProcedure()
        {
            if (Matrix is null)
            {
                return;
            }
            //string dir = dir = Directory.GetCurrentDirectory() + "\\projects\\" + QAnalysisFunc.GetNameOfProject();
            string dir = Directory.GetCurrentDirectory() + "\\projects";
            string pathName;
            bool isSliced = false;
            if (slicingValue.Text != "")
            {
                int sliceValue;
                if (!int.TryParse(slicingValue.Text, out sliceValue))
                {
                    return;
                }
                // Saving matrix after slicing proedure
                pathName = dir  + "\\" + projectName.Text;
                isSliced = true;
            }
            else
            {
                pathName = dir  + "\\" + projectName.Text;
                directoryPath = pathName;
                if (!Directory.Exists(pathName))
                {
                    Directory.CreateDirectory(pathName);
                }
            }

 
            string name = projectName.Text.Split("_")[0];
            
            using StreamWriter file = new(pathName + "\\" + name + ".csv");
            var dt = (DataTable)(this.Matrix);

            if (isSliced)
            {
                using StreamWriter fileSlice = new(pathName + "\\" + name + "_" + slicingValue.Text + ".csv");
                MatrixSaving(dt, fileSlice);
            }
            else
            {
                using StreamWriter fileT = new(pathName + "\\" + name + "_T" + ".csv");
                var dtT = Transpose(dt);
                MatrixSaving(dtT, fileT);

            }

            MatrixSaving(dt, file);

            ShowAlertText("Matrix was successfully saved.", false);
            loadMessage.Visibility = Visibility.Visible;
        }

        private DataTable Transpose(DataTable dt)
        {
            DataTable dtNew = new DataTable();

            //adding columns    
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dtNew.Columns.Add(i.ToString());
            }


            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                dtNew.Columns[i + 1].ColumnName = "y" + (i + 1);
            }

            //Adding Row Data
            for (int k = 0; k < dt.Columns.Count; k++)
            {
                DataRow r = dtNew.NewRow();
                //r[0] = dt.Columns[k].ToString();
                for (int j = 0; j <= dt.Rows.Count - 1; j++)
                    r[j] = dt.Rows[j][k];
                dtNew.Rows.Add(r);
            }

            return dtNew;
        }

        private async void MatrixSaving(DataTable dt, StreamWriter file)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    int number;
                    string str;
                    // Null elements is Default value
                    if (dt.Rows[i].IsNull(j))
                    {
                        number = DefaultValue;
                        str = DefaultValue.ToString();
                    }
                    else
                    {
                        str = dt.Rows[i].Field<object>(j).ToString();
                        //number = dt.Rows[i].Field<int>(j);
                    }
                    if (j != dt.Columns.Count - 1)
                    {
                        await file.WriteAsync(str + ',');
                    }
                    else
                    {
                        await file.WriteAsync(str + "\n");
                    }
                }
            }
        }
        // Click event to save a matrix
        private async void SaveMatrix(object sender, RoutedEventArgs e)
        {
            SaveMatrixProcedure();
        }


        private void sliceButton(object sender, RoutedEventArgs e)
        {
            if (Matrix is null)
            {
                return;
            }
            int slice;
            if (slicingValue.Text != "" && slicingValue.Text is not null)
            {
                slice = int.Parse(slicingValue.Text); // #TODO Обработка исключений
                slicingParameter.Text = "Slicing parameter: θ = " + slice + ".";
            } else
            {
                return;
            }
            OldMatrix = Matrix.Clone();
            foreach (DataRow row in Matrix.Rows)
            {
                OldMatrix.ImportRow(row);
            }
            for (int i = 0; i < Matrix.Rows.Count; i++) 
            {
                for (int j = 0; j < Matrix.Columns.Count; j++)
                {
                    DataRow row = Matrix.Rows[i];
                    int element = Convert.ToInt32(row["y" + (j + 1)]);
                    if (element >= slice)
                    {
                        row["y" + (j + 1)] = 1;
                    } else
                    {
                        row["y" + (j + 1)] = 0;
                    }
                }
            }

            SaveMatrixProcedure();
    
            slicingValue.Text = "";
        }

        private void LoadFile(string[] file)
        {
            int sizeN = file.Length;
            int sizeM;
            if (sizeN == 0)
            {
                sizeM = 0;
            } else {
                sizeM = file[0].ToString().Split(',').Length;
            }

            nSize.Text = sizeN.ToString();
            mSize.Text = sizeM.ToString();

            UpdateMatrix(sizeN, sizeM, file);
        }

        // Load Matrix from the file
        private void LoadProcedure(OpenFileDialog ofd)
        {
            projectName.Text = ofd.FileName.Split('\\').Last().Split('.')[0];
            var file = File.ReadAllLines(ofd.FileName);
            LoadFile(file);
        }

        private void LoadProcedure(string file)
        {
            var fileData = File.ReadAllLines(file);
            PathInit();
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

        private void returnSliceButton(object sender, RoutedEventArgs e)
        {
            if (OldMatrix is null)
            {
                return;
            }
            Matrix.Clear();
            foreach (DataRow row in OldMatrix.Rows)
            {
                Matrix.ImportRow(row);
            }
        }

        private void acceptBtn(object sender, RoutedEventArgs e)
        {
            if (Matrix == null)
            {
                return;
            }
            if (isNotOneOrZero())
            {
                ShowAlertText("Incorrect matrix.", true);
                return;
            }
            if (resultWindow is null)
            {
                resultWindow = new ResultWindow(this, this.Matrix, projectName.Text, CastiEcc.IsChecked, DucksteinEcc.IsChecked );
            } else
            {
                resultWindow.Update(this.Matrix, CastiEcc.IsChecked, DucksteinEcc.IsChecked);
            }
        
            resultWindow.Show();
            this.Hide();
        }

        private bool isNotOneOrZero()
        {
            foreach (DataRow row in Matrix.Rows)
            {
                for (int i = 0; i < Matrix.Columns.Count; i++)
                {
                   
                    string value = row[i].ToString();
                    if (value == null)
                    {
                        return true;
                    }
                    if (value != "0" && value != "1")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void questionMarkBtn(object sender, RoutedEventArgs e)
        {
           HelpModalWindow hp = new("A binary matrix is a representation of a binary relation defined on the Cartesian product of X × Y, where  X = {x1, x2, ..., xn} and Y = {y1, y2, ..., ym}.");
            hp.Show();
        }

        private void DataGridLoading(object sender, DataGridRowEventArgs e)
        {
            string res = "x" + (e.Row.GetIndex() + 1);
            e.Row.Header = res;
        }

        private void checkedDefaultValue(object sender, RoutedEventArgs e)
        {
            if (this.defaultValueZero == null)
            {
                return;
            }
            this.defaultValueOne.IsChecked = false;
        }

        private void checkedDefaultValueOne(object sender, RoutedEventArgs e)
        {
            if (this.defaultValueOne == null)
            {
                return;
            }
            this.defaultValueZero.IsChecked = false;
        }

        private void ExitWindow(object sender, EventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            SaveMatrixProcedure();
            this.Close();
        }

        private void openSource(object sender, RoutedEventArgs e)
        {
                try
                {
                    Process.Start("explorer.exe", directoryPath);
            }
                catch (Exception ex)
                {
                    // Handle any exceptions that may occur while opening the directory
                    MessageBox.Show("Error opening directory: " + ex.Message);
                }
            
        }

    }
}
