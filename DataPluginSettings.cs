namespace OpenFFBoard.PluginSdk
{
    /// <summary>
    /// Settings class, make sure it can be correctly serialized using JSON.net
    /// </summary>
    public class DataPluginSettings
    {
        public int SpeedWarningLevel = 100;
        public int BaudRate = 115200;
        public string ConnectTo = null;
    }
}