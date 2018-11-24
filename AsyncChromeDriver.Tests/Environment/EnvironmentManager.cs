using System;
using System.Reflection;
using System.IO;
using Zu.AsyncWebDriver;
using Zu.AsyncWebDriver.Remote;

//using AsyncWebDriver.SeleniumAdapter;

namespace OpenQA.Selenium.Environment
{
    public class EnvironmentManager
    {
        public static Browser CurrentTestingBrowser = (Browser)Enum.Parse(typeof(Browser), GetSettingValue("DriverName"));// Browser.Chrome;

        private static EnvironmentManager instance;
        private Type driverType;
        private Browser browser;
        private Zu.Chrome.AsyncChromeDriver _asyncChromeDriver;
        private IWebDriver driver;
        private UrlBuilder urlBuilder;
        private TestWebServer webServer;
        //RemoteSeleniumServer remoteServer;
        private string remoteCapabilities;
        private string driverStringArg;

        private EnvironmentManager()
        {
            string configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            try
            {
                string driverClassName = GetSettingValue("Driver");
                driverStringArg = GetSettingValue("DriverStringArg");
                //string assemblyName = GetSettingValue("Assembly");
                //Assembly assembly = Assembly.Load(assemblyName);
                //driverType = assembly.GetType(driverClassName);
                remoteCapabilities = GetSettingValue("RemoteCapabilities");
            }
            catch (Exception)
            {
            }

            urlBuilder = new UrlBuilder();

            string currentDirectory = this.CurrentDirectory;
            DirectoryInfo info = new DirectoryInfo(currentDirectory);
            while (info != info.Root && string.Compare(info.Name, "AsyncChromeDriver" /*"build"*/, StringComparison.OrdinalIgnoreCase) != 0)
            {
                info = info.Parent;
            }

            //info = info.Parent;
            webServer = new TestWebServer(info.FullName);
            bool autoStartRemoteServer = false;
            if (browser == Browser.Remote)
            {
                autoStartRemoteServer = bool.Parse(GetSettingValue("AutoStartRemoteServer"));
            }

            //remoteServer = new RemoteSeleniumServer(info.FullName, autoStartRemoteServer);
        }

        ~EnvironmentManager()
        {
            //remoteServer.Stop();
            webServer?.Stop();
            driver?.Quit();
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
            get { return browser; }
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
            get { return webServer; }
        }

        //public RemoteSeleniumServer RemoteServer
        //{
        //    get { return remoteServer; }
        //}

        public string RemoteCapabilities
        {
            get { return remoteCapabilities; }
        }

        public IWebDriver GetCurrentDriver()
        {
            if (driver != null)
            { 
                return driver; 
            }
            else 
            { 
                return CreateFreshDriver(); 
            }
        }

        public IWebDriver CreateDriverInstance()
        {
            _asyncChromeDriver = new Zu.Chrome.AsyncChromeDriver();
            driver = new WebDriver(_asyncChromeDriver);
            return driver;
            //if(!string.IsNullOrWhiteSpace(driverStringArg)) return (IWebDriver)Activator.CreateInstance(driverType, driverStringArg);
            //return (IWebDriver)Activator.CreateInstance(driverType);
        }

        public IWebDriver CreateFreshDriver()
        {
            CloseCurrentDriver();
            driver = CreateDriverInstance();
            return driver;
        }

        public void CloseCurrentDriver()
        {
            driver?.Close();
            driver = null;
        }

        public static EnvironmentManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EnvironmentManager();
                }

                return instance;
            }
        }

        public UrlBuilder UrlBuilder
        {
            get
            {
                return urlBuilder;
            }
        }

    }
}
