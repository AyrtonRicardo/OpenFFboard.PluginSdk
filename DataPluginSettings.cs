namespace OpenFFBoardPlugin
{
    /// <summary>
    /// Settings class, make sure it can be correctly serialized using JSON.net
    /// </summary>
    public class DataPluginSettings
    {
        public int BaudRate = 115200;
        public string ConnectTo = null;
        public bool AutoConnectOnStartup = false;
        public string ProfileJsonPath = null;
    }
}