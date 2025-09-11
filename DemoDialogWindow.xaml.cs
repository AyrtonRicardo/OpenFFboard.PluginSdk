using SimHub.Plugins.UI;

namespace OpenFFBoardPlugin
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
