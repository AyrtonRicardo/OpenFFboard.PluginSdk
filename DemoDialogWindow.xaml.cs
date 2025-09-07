using SimHub.Plugins.UI;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenFFBoard.PluginSdk
{
    /// <summary>
    /// Logique d'interaction pour DemoDialogWindow.xaml
    /// </summary>
    public partial class DemoDialogWindow : SHDialogContentBase
    {
        public DemoDialogWindow()
        {
            InitializeComponent();
            ShowOk = true;
            ShowCancel = true;
        }
    }
}
