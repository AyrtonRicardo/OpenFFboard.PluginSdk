using GameReaderCommon;
using SimHub.Plugins;
using System;
using System.Windows.Media;

namespace OpenFFBoard.PluginSdk
{
    [PluginDescription("OpenFFBoard plugin to communicate with openffboard firmware")]
    [PluginAuthor("Ayrton Ricardo")]
    [PluginName("OpenFFBoard companion plugin")]
    public class DataPlugin : IPlugin, IDataPlugin, IWPFSettingsV2
    {
        public DataPluginSettings Settings;
        public OpenFFBoard.Board OpenFFBoard;
        public string[] Boards = null;

        /// <summary>
        /// Instance of the current plugin manager
        /// </summary>
        public PluginManager PluginManager { get; set; }

        /// <summary>
        /// Gets the left menu icon. Icon must be 24x24 and compatible with black and white display.
        /// </summary>
        public ImageSource PictureIcon => this.ToIcon(Properties.Resources.sdkmenuicon);

        /// <summary>
        /// Gets a short plugin title to show in left menu. Return null if you want to use the title as defined in PluginName attribute.
        /// </summary>
        public string LeftMenuTitle => "OpenFFBoard companion";

        /// <summary>
        /// Called one time per game data update, contains all normalized game data,
        /// raw data are intentionally "hidden" under a generic object type (A plugin SHOULD NOT USE IT)
        ///
        /// This method is on the critical path, it must execute as fast as possible and avoid throwing any error
        ///
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <param name="data">Current game data, including current and previous data frame.</param>
        public void DataUpdate(PluginManager pluginManager, ref GameData data)
        {
            // Define the value of our property (declared in init)
            if (data.GameRunning)
            {
                SimHub.Logging.Current.Info("Game is running cool");
                if (data.OldData != null && data.NewData != null)
                {
                   if (data.OldData.SpeedKmh < Settings.SpeedWarningLevel && data.OldData.SpeedKmh >= Settings.SpeedWarningLevel)
                   {
                        //  Trigger an event
                       this.TriggerEvent("SpeedWarning");
                   }
                }
            }
        }

        /// <summary>
        /// Called at plugin manager stop, close/dispose anything needed here !
        /// Plugins are rebuilt at game change
        /// </summary>
        /// <param name="pluginManager"></param>
        public void End(PluginManager pluginManager)
        {
            // Save settings
            this.SaveCommonSettings("GeneralSettings", Settings);
        }

        /// <summary>
        /// Returns the settings control, return null if no settings control is required
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <returns></returns>
        public System.Windows.Controls.Control GetWPFSettingsControl(PluginManager pluginManager)
        {
            return new SettingsControlDemo(this);
        }

        public void ConnectToBoard(string comPort, int baudRate)
        {
            this.OpenFFBoard = new OpenFFBoard.Serial(comPort, baudRate);
            this.OpenFFBoard.Connect();
        }

        public bool IsConnected()
        {
            return OpenFFBoard != null && OpenFFBoard.IsConnected;
        }

        public void Disconnect()
        {
            if (OpenFFBoard == null || !OpenFFBoard.IsConnected)
            {
                return;
            }

            OpenFFBoard.Disconnect();
            OpenFFBoard = null;
        }

        /// <summary>
        /// Called once after plugins startup
        /// Plugins are rebuilt at game change
        /// </summary>
        /// <param name="pluginManager"></param>
        public void Init(PluginManager pluginManager)
        {
            SimHub.Logging.Current.Info("Starting plugin");
            Console.WriteLine("THIS HAS STARTED");

            // Load settings
            Settings = this.ReadCommonSettings<DataPluginSettings>("GeneralSettings", () => new DataPluginSettings());

            Boards = global::OpenFFBoard.Serial.GetBoards();

            if (Settings.ConnectTo != null)
            {
                ConnectToBoard(Settings.ConnectTo, Settings.BaudRate);
            }

            // Declare a property available in the property list, this gets evaluated "on demand" (when shown or used in formulas)
            //this.AttachDelegate(name: "CurrentDateTime", valueProvider: () => DateTime.Now);

            // Declare an event
            //this.AddEvent(eventName: "SpeedWarning");

            // Declare an action which can be called
            /* this.AddAction(
                actionName: "IncrementSpeedWarning",
                actionStart: (a, b) =>
                {
                    Settings.SpeedWarningLevel++;
                    SimHub.Logging.Current.Info("Speed warning changed");
                }); */

            // Declare an action which can be called, actions are meant to be "triggered" and does not reflect an input status (pressed/released ...)
            /* this.AddAction(
                actionName: "DecrementSpeedWarning",
                actionStart: (a, b) =>
                {
                    Settings.SpeedWarningLevel--;
                }); */

            // Declare an input which can be mapped, inputs are meant to be keeping state of the source inputs,
            // they won't trigger on inputs not capable of "holding" their state.
            // Internally they work similarly to AddAction, but are restricted to a "during" behavior
            /* this.AddInputMapping(
                inputName: "InputPressed",
                inputPressed: (a, b) => {/* One of the mapped input has been pressed   * /},
                inputReleased: (a, b) => {/* One of the mapped input has been released * /}
            ); */
        }
    }
}