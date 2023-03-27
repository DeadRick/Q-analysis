using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Q_analysis
{
    /// <summary>
    /// Interaction logic for NameProjectWindow.xaml
    /// </summary>
    public partial class NameProjectWindow : Window
    {
        public int clickCnt = 0;
        public NameProjectWindow()
        {
            InitializeComponent();
        }

        private async Task saveProjectNameAsync()
        {

            string fileName = "qanalysis_info.txt";
            using StreamWriter file = new(fileName);
            await file.WriteAsync(projectName.Text);
        }

        private void OnClickName(object sender, RoutedEventArgs e)
        {
            Regex r = new(@"^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$");
            string dir = Directory.GetCurrentDirectory() + "\\projects";
            string prjName = projectName.Text;
            string pathName = dir + "\\" + prjName;

            if (r.IsMatch(pathName) && (prjName.Length <= 250))
            {
                if (Directory.Exists(pathName) && clickCnt < 4)
                {
                    clickCnt += 1; //#TODO Delete debug mode
                    warningText.Foreground = Brushes.Red;
                    warningText.Text = "Project with this name already exists!";
                    return;
                } 
      
                Directory.CreateDirectory(pathName);
                _ = saveProjectNameAsync();
                

                this.DialogResult = true;

            } else
            {
                projectName.BorderBrush = Brushes.PaleVioletRed;
                warningText.Foreground = Brushes.Red;
                warningText.Text = "Incorrect file name!";
            }

        }
    }
}
