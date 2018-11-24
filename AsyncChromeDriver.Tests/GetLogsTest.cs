using System.Collections.Generic;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.BasicTypes;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class GetLogsTest : DriverTestFixture
    {
        private IWebDriver localDriver;

        [TearDown]
        public async Task QuitDriver()
        {
            if (localDriver != null) {
                await localDriver.Quit();
                localDriver = null;
            }
        }

        [Test]
        public async Task LogBufferShouldBeResetAfterEachGetLogCall()
        {

            ReadOnlyCollection<string> logTypes = await driver.Options().Logs.AvailableLogTypes();
            foreach (string logType in logTypes) {
                await driver.GoToUrl(simpleTestPage);
                ReadOnlyCollection<LogEntry> firstEntries = await driver.Options().Logs.GetLog(logType);
                if (firstEntries.Count > 0) {
                    ReadOnlyCollection<LogEntry> secondEntries = await driver.Options().Logs.GetLog(logType);
                    Assert.That(HasOverlappingLogEntries(firstEntries, secondEntries), Is.False,
                        $"There should be no overlapping log entries in consecutive get log calls for {logType} logs");
                }
            }
        }

        [Test]
        public async Task DifferentLogsShouldNotContainTheSameLogEntries()
        {
            await driver.GoToUrl(simpleTestPage);
            Dictionary<string, ReadOnlyCollection<LogEntry>> logTypeToEntriesDictionary = new Dictionary<string, ReadOnlyCollection<LogEntry>>();
            ReadOnlyCollection<string> logTypes = await driver.Options().Logs.AvailableLogTypes();
            foreach (string logType in logTypes) {
                logTypeToEntriesDictionary.Add(logType, await driver.Options().Logs.GetLog(logType));
            }

            foreach (string firstLogType in logTypeToEntriesDictionary.Keys) {
                foreach (string secondLogType in logTypeToEntriesDictionary.Keys) {
                    if (firstLogType != secondLogType) {
                        Assert.That(HasOverlappingLogEntries(logTypeToEntriesDictionary[firstLogType], logTypeToEntriesDictionary[secondLogType]), Is.False,
                            $"Two different log types ({firstLogType}, {secondLogType}) should not contain the same log entries");
                    }
                }
            }
        }

        [Test]
        public async Task TurningOffLogShouldMeanNoLogMessages()
        {
            ReadOnlyCollection<string> logTypes = await driver.Options().Logs.AvailableLogTypes();
            foreach (string logType in logTypes) {
                await CreateWebDriverWithLogging(logType, LogLevel.Off);
                ReadOnlyCollection<LogEntry> entries = await localDriver.Options().Logs.GetLog(logType);
                Assert.AreEqual(0, entries.Count,
                    $"There should be no log entries for log type {logType} when logging is turned off.");
                await QuitDriver();
            }
        }

        private async Task CreateWebDriverWithLogging(string logType, LogLevel logLevel)
        {
            //if (TestUtilities.IsChrome(driver)) {
            //    ChromeDriverService service = ChromeDriverService.CreateDefaultService(EnvironmentManager.Instance.DriverServiceDirectory);
            //    ChromeOptions options = new ChromeOptions();
            //    options.SetLoggingPreference(logType, logLevel);
            //    localDriver = new ChromeDriver(service, options);
            //}

            await localDriver.GoToUrl(simpleTestPage);
        }

        private bool HasOverlappingLogEntries(ReadOnlyCollection<LogEntry> firstLog, ReadOnlyCollection<LogEntry> secondLog)
        {
            foreach (LogEntry firstEntry in firstLog) {
                foreach (LogEntry secondEntry in secondLog) {
                    if (firstEntry.Level == secondEntry.Level && firstEntry.Message == secondEntry.Message && firstEntry.Timestamp == secondEntry.Timestamp) {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
