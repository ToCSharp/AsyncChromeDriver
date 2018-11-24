using System;
using System.Reflection;
using System.IO;
using Zu.AsyncWebDriver;
using Zu.AsyncWebDriver.Remote;

//using AsyncWebDriver.SeleniumAdapter;

namespace Zu.AsyncChromeDriver.Tests.Environment
{
    public class EnvironmentManager
    {
        public static Browser CurrentTestingBrowser = (Browser)Enum.Parse(typeof(Browser), GetSettingValue("DriverName"));// Browser.Chrome;

        private static EnvironmentManager _instance;
        private Type _driverType;
        private Browser _browser;
        private Zu.Chrome.AsyncChromeDriver _asyncChromeDriver;
        private IWebDriver _driver;
        private UrlBuilder _urlBuilder;
        private TestWebServer _webServer;
        //RemoteSeleniumServer remoteServer;
        private string _remoteCapabilities;
        private string _driverStringArg;

        private EnvironmentManager()
        {
            string configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            try
            {
                string driverClassName = GetSettingValue("Driver");
                _driverStringArg = GetSettingValue("DriverStringArg");
                //string assemblyName = GetSettingValue("Assembly");
                //Assembly assembly = Assembly.Load(assemblyName);
                //driverType = assembly.GetType(driverClassName);
                _remoteCapabilities = GetSettingValue("RemoteCapabilities");
            }
            catch (Exception)
            {
            }

            _urlBuilder = new UrlBuilder();

            string currentDirectory = this.CurrentDirectory;
            DirectoryInfo info = new DirectoryInfo(currentDirectory);
            while (info != info.Root && string.Compare(info.Name, "AsyncChromeDriver" /*"build"*/, StringComparison.OrdinalIgnoreCase) != 0)
            {
                info = info.Parent;
            }

            //info = info.Parent;
            _webServer = new TestWebServer(info.FullName);
            bool autoStartRemoteServer = false;
            if (_browser == Browser.Remote)
            {
                autoStartRemoteServer = bool.Parse(GetSettingValue("AutoStartRemoteServer"));
            }

            //remoteServer = new RemoteSeleniumServer(info.FullName, autoStartRemoteServer);
        }

        ~EnvironmentManager()
        {
            //remoteServer.Stop();
            _webServer?.Stop();
            _driver?.Quit();
        }

        public static string GetSettingValue(string key)
        {
            string settingValue = string.Empty;
            try
            {
                settingValue = System.Configuration.ConfigurationManager.AppSettings.GetValues(key)[0];
            }
            catch (Exception)
            {
            }

            return settingValue;
        }

        public Browser Browser 
        {
            get { return _browser; }
        }

        public string CurrentDirectory
        {
            get
            {
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                string assemblyLocation = executingAssembly.Location;

                // If we're shadow copying,. fiddle with 
                // the codebase instead 
                if (AppDomain.CurrentDomain.ShadowCopyFiles)
                {
                    Uri uri = new Uri(executingAssembly.CodeBase);
                    assemblyLocation = uri.LocalPath;
                }

                string currentDirectory = Path.GetDirectoryName(assemblyLocation);
                return currentDirectory;
            }
        }
        
        public TestWebServer WebServer
        {
            get { return _webServer; }
        }

        //public RemoteSeleniumServer RemoteServer
        //{
        //    get { return remoteServer; }
        //}

        public string RemoteCapabilities
        {
            get { return _remoteCapabilities; }
        }

        public IWebDriver GetCurrentDriver()
        {
            if (_driver != null)
            { 
                return _driver; 
            }
            else 
            { 
                return CreateFreshDriver(); 
            }
        }

        public IWebDriver CreateDriverInstance()
        {
            _asyncChromeDriver = new Zu.Chrome.AsyncChromeDriver();
            _driver = new WebDriver(_asyncChromeDriver);
            return _driver;
            //if(!string.IsNullOrWhiteSpace(driverStringArg)) return (IWebDriver)Activator.CreateInstance(driverType, driverStringArg);
            //return (IWebDriver)Activator.CreateInstance(driverType);
        }

        public IWebDriver CreateFreshDriver()
        {
            CloseCurrentDriver();
            _driver = CreateDriverInstance();
            return _driver;
        }

        public void CloseCurrentDriver()
        {
            _driver?.Close();
            _driver = null;
        }

        public static EnvironmentManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EnvironmentManager();
                }

                return _instance;
            }
        }

        public UrlBuilder UrlBuilder
        {
            get
            {
                return _urlBuilder;
            }
        }

    }
}
