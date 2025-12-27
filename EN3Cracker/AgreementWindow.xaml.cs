using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace EN3Cracker
{
    /// <summary>
    /// AgreementWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AgreementWindow : Window
    {
        bool isagree { get; set; }
        public AgreementWindow()
        {
            InitializeComponent();
            agreement.Selection.Load(new FileStream("./Assets/agreement.rtf", FileMode.Open), DataFormats.Rtf);
        }

        private void disagree_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void agree_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
