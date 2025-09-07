using SimHub.Plugins.Styles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace OpenFFBoard.PluginSdk
{
    /// <summary>
    /// Logique d'interaction pour SettingsControlDemo.xaml
    /// </summary>
    public partial class SettingsControlDemo : UserControl
    {
        public DataPlugin Plugin { get; }

        public SettingsControlDemo()
        {
            InitializeComponent();
        }

        public string ConnectionString()
        {
            if (Plugin == null) return null;

            if (Plugin.OpenFFBoard.IsConnected) return "CONNECTED";

            return "NOT CONNECTED";
        }

        private void UpdateConnectedTo(string connectedTo)
        {
            if (Plugin.Settings.ConnectTo != connectedTo)
            {
                Plugin.Settings.ConnectTo = connectedTo;
            }
            ViewConnectedTo.Text = connectedTo;
        }

        public SettingsControlDemo(DataPlugin plugin) : this()
        {
            this.Plugin = plugin;
        }

        private async void StyledMessageBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var res = await SHMessageBox.Show("Message box", "Hello", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question);

            await SHMessageBox.Show(res.ToString());
        }

        private void DemoWindow_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var window = new DemoWindow();

            window.Show();
        }

        private async void DemodialogWindow_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialogWindow = new DemoDialogWindow();

            var res = await dialogWindow.ShowDialogWindowAsync(this);

            await SHMessageBox.Show(res.ToString());
        }

        private void ViewBoards_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Plugin == null)
            {
                return;
            }

            List<BoardText> boards = new List<BoardText>();
            foreach (var board in Plugin.Boards)
            {
                boards.Add(new BoardText() { Name = board, IsEnabled = Plugin.Settings.ConnectTo != null && Plugin.Settings.ConnectTo.Equals(board) } );
            }

            viewBoards.ItemsSource = boards;

            foreach (object o in viewBoards.Items)
            {
                if ((o is BoardText) && ((o as BoardText).IsEnabled))
                {
                    viewBoards.SelectedItem = o;
                    break;
                }
            }

            this.UpdateConnectedTo(SelectedBoard()?.Name);
        }

        private void ViewBoards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (BoardText bt in e.RemovedItems)
            {
                bt.IsEnabled = false;
            }

            if (viewBoards.SelectedItem != null)
            {
                (viewBoards.SelectedItem as BoardText).IsEnabled = true;
            }
        }

        private BoardText SelectedBoard()
        {
            if (viewBoards.SelectedItem != null && viewBoards.SelectedItem is BoardText)
            {
                return viewBoards.SelectedItem as BoardText;
            }
            return null;
        }

        private async void ViewSelectedCom_Connect(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Plugin == null)
            {
                return;
            }

            try
            {
                var selected = SelectedBoard();
                if (selected == null)
                {
                    var res = await SHMessageBox.Show("Message box", "No COM selected", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question);
                    await SHMessageBox.Show(res.ToString());
                    return;
                }

                Plugin.ConnectToBoard(SelectedBoard().Name, Plugin.Settings.BaudRate);
                this.UpdateConnectedTo(SelectedBoard().Name);
            }
            catch (Exception ex)
            {
                await SHMessageBox.Show(ex.ToString());
            }

        }

        private async void ViewSelectedCom_Disconnect(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Plugin == null)
            {
                return;
            }

            try
            {
                if (Plugin.OpenFFBoard == null)
                {
                    await SHMessageBox.Show("OpenFFBoard is not connected");
                    return;
                }

                Plugin.Disconnect();
            }
            catch (Exception ex)
            {
                await SHMessageBox.Show(ex.ToString());
            }

        }

        private async void SetProfileToCurrentGame_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Plugin == null || Plugin.PluginManager == null || Plugin.PluginManager.GameManager == null)
            {
                await SHMessageBox.Show("No data identified, cannot set profile.");
                return;
            }

            if (Plugin.PluginManager.GameManager.GameName() == "")
            {
                await SHMessageBox.Show("No game detected, cannot set profile");
                return;
            }

            if (!Plugin.IsConnected())
            {
                await SHMessageBox.Show("No board connected, cannot set profile");
                return;
            }

            var gameName = Plugin.PluginManager.GameManager.GameName();
            DebugResponse.Text = Plugin.OpenFFBoard.Main.GetId().ToString();

            ViewCurrentActiveProfile.Text = gameName;
        }
    }

    public class BoardText
    {
        public string Name { get; set; }
        private bool Enabled { get; set; }

        public bool IsEnabled
        {
            get { return Enabled; }
            set
            {
                Enabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}