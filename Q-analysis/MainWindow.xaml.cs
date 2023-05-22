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

        private void aboutBtn(object sender, RoutedEventArgs e)
        {
            HelpModalWindow hmw = new("One of the most significant stages of system research is the stage of analysis. The success of the steps undertaken at this stage is considerably related to the “level of understanding” of the system S as a whole, its key elements, and their interrelations. Preliminary conclusions regarding the complexity of the system are determined to a large extent by the structure of the system. Specifically, by using Q-analysis approach (polyhedral dynamics) proposed by English mathematician Ronald Atkin in the early 1970s, we gain the opportunity to explore aspects of the structural complexity of the system that manifest through the connectivity of its elements. \r\n   This approach is based on representing the system as an abstract simplicial complex K (the starting point is the system’s S model in the form of a 0/1 adjacency matrix), which is formed by multidimensional facets (simplices of different dimensions). The presence of such multidimensional convex formations (points as 0-simplices, line segments being geometrical representations of 1-simplices, triangles with their interior area, tetrahedra, etc.) opens up the path to analyze their connectivity at different dimensional levels of K’s analysis. At that, corresponding simplices can be directly connected or connected to each other through chains of connectivity, i.e., intermediate links (simplices) of certain dimensions. As a result, a structural vector Q = (QN,...,Q1,Q0) of simplicial complex K (as a global characteristic of K), where the dimension of the complex is N, can be constructed, where each vector’s component Qi is the number of connectivity components formed by simplices at each dimensional level i of the complex’s analysis. The results of computations are directly used in analyzing the specific structural organization of K as a model of the system S under study. In addition, at the local level of K's analysis, each simplex of the complex is characterized by the eccentricity value, viz. the degree of 'nestedness' of the simplex in K's structure).  \r\n   The starting point for computations is the system’s S model as a subset of XxY in the form of 0/1 adjacency matrix V = [vij], where '0' expresses the presence of a connection between the corresponding elements x from set X and y as Y’s element. X = {x1,x2,...,xn} and Y = {y1,y2,...,ym} are two finite sets associated with S - for example, X can be a set of company employees, and Y can be the set of projects that the company is carrying out, and the fact that the corresponding element vij of the matrix V equals 1 means that employee xi participates in the realization of company’s project yj. The '1' elements in matrix V determine (along the rows of the matrix) corresponding simplices of the complex K and their dimensions.");
            hmw.Show();
        }
    }
}
