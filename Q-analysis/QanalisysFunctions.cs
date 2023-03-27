using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Q_analysis
{
    internal class QAnalysisFunc
    {

        internal static string GetNameOfProject() // #TODO Change THIS METHODE!
        {
            return File.ReadAllText(Environment.CurrentDirectory+ "\\qanalysis_info.txt");
        }
    }
}
