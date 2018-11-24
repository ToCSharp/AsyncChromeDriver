using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Zu.AsyncChromeDriver.Tests.Environment;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;
using Zu.WebBrowser.BrowserOptions;
using Cookie = Zu.WebBrowser.BasicTypes.Cookie;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class CookieImplementationTest : DriverTestFixture
    {
        private Random _random = new Random();
        private bool _isOnAlternativeHostName;
        private string _hostname;


        [SetUp]
        public async Task GoToSimplePageAndDeleteCookies()
        {
            await GotoValidDomainAndClearCookies("animals");
            await AssertNoCookiesArePresent();
        }

        [Test]
        public async Task ShouldGetCookieByName()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            string key = $"key_{new Random().Next()}";
            await ((IJavaScriptExecutor)driver).ExecuteScript("document.cookie = arguments[0] + '=set';", new CancellationToken(), key);

            Cookie cookie = await driver.Options().Cookies.GetCookieNamed(key);
            Assert.AreEqual("set", cookie.Value);
        }

        [Test]
        public async Task ShouldBeAbleToAddCookie()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            string key = GenerateUniqueKey();
            string value = "foo";
            Cookie cookie = new Cookie(key, value);
            await AssertCookieIsNotPresentWithName(key);

            await driver.Options().Cookies.AddCookie(cookie);

            await AssertCookieHasValue(key, value);
            Assert.That(await driver.Options().Cookies.AllCookies().Contains(cookie), "Cookie was not added successfully");
        }

        [Test]
        public async Task GetAllCookies()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            string key1 = GenerateUniqueKey();
            string key2 = GenerateUniqueKey();

            await AssertCookieIsNotPresentWithName(key1);
            await AssertCookieIsNotPresentWithName(key2);

            ReadOnlyCollection<Cookie> cookies = await driver.Options().Cookies.AllCookies();
            int count = cookies.Count;

            Cookie one = new Cookie(key1, "value");
            Cookie two = new Cookie(key2, "value");

            await driver.Options().Cookies.AddCookie(one);
            await driver.Options().Cookies.AddCookie(two);

            await driver.GoToUrl(simpleTestPage);
            cookies = await driver.Options().Cookies.AllCookies();
            Assert.AreEqual(count + 2, cookies.Count);

            Assert.That(cookies, Does.Contain(one));
            Assert.That(cookies, Does.Contain(two));
        }

        [Test]
        public async Task DeleteAllCookies()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            await ((IJavaScriptExecutor)driver).ExecuteScript("document.cookie = 'foo=set';");
            await AssertSomeCookiesArePresent();

            await driver.Options().Cookies.DeleteAllCookies();

            await AssertNoCookiesArePresent();
        }

        [Test]
        public async Task DeleteCookieWithName()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            string key1 = GenerateUniqueKey();
            string key2 = GenerateUniqueKey();

            await ((IJavaScriptExecutor)driver).ExecuteScript("document.cookie = arguments[0] + '=set';", new CancellationToken(), key1);
            await ((IJavaScriptExecutor)driver).ExecuteScript("document.cookie = arguments[0] + '=set';", new CancellationToken(), key2);

            await AssertCookieIsPresentWithName(key1);
            await AssertCookieIsPresentWithName(key2);

            await driver.Options().Cookies.DeleteCookieNamed(key1);

            await AssertCookieIsNotPresentWithName(key1);
            await AssertCookieIsPresentWithName(key2);
        }

        [Test]
        public async Task ShouldNotDeleteCookiesWithASimilarName()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            string cookieOneName = "fish";
            Cookie cookie1 = new Cookie(cookieOneName, "cod");
            Cookie cookie2 = new Cookie(cookieOneName + "x", "earth");
            IOptions options = driver.Options();
            await AssertCookieIsNotPresentWithName(cookie1.Name);

            await options.Cookies.AddCookie(cookie1);
            await options.Cookies.AddCookie(cookie2);

            await AssertCookieIsPresentWithName(cookie1.Name);

            await options.Cookies.DeleteCookieNamed(cookieOneName);

            Assert.That(await driver.Options().Cookies.AllCookies(), Does.Not.Contain(cookie1));
            Assert.That(await driver.Options().Cookies.AllCookies(), Does.Contain(cookie2));
        }

        [Test]
        public async Task AddCookiesWithDifferentPathsThatAreRelatedToOurs()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            string basePath = EnvironmentManager.Instance.UrlBuilder.Path;

            Cookie cookie1 = new Cookie("fish", "cod", "/" + basePath + "/animals");
            Cookie cookie2 = new Cookie("planet", "earth", "/" + basePath + "/");
            IOptions options = driver.Options();
            await options.Cookies.AddCookie(cookie1);
            await options.Cookies.AddCookie(cookie2);

            UrlBuilder builder = EnvironmentManager.Instance.UrlBuilder;
            await driver.GoToUrl(builder.WhereIs("animals"));

            ReadOnlyCollection<Cookie> cookies = await options.Cookies.AllCookies();
            await AssertCookieIsPresentWithName(cookie1.Name);
            await AssertCookieIsPresentWithName(cookie2.Name);

            await driver.GoToUrl(builder.WhereIs("simpleTest.html"));
            await AssertCookieIsNotPresentWithName(cookie1.Name);
        }

        [Test]
        public async Task CannotGetCookiesWithPathDifferingOnlyInCase()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            string cookieName = "fish";
            await driver.Options().Cookies.AddCookie(new Cookie(cookieName, "cod", "/Common/animals"));

            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("animals"));
            Assert.That(driver.Options().Cookies.GetCookieNamed(cookieName), Is.Null);
        }

        [Test]
        public async Task ShouldNotGetCookieOnDifferentDomain()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            string cookieName = "fish";
            await driver.Options().Cookies.AddCookie(new Cookie(cookieName, "cod"));
            await AssertCookieIsPresentWithName(cookieName);

            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereElseIs("simpleTest.html"));

            await AssertCookieIsNotPresentWithName(cookieName);
        }

        [Test]
        public async Task ShouldBeAbleToAddToADomainWhichIsRelatedToTheCurrentDomain()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            // Cookies cannot be set on domain names with less than 2 dots, so
            // localhost is out. If we are in that boat, bail the test.
            string hostName = EnvironmentManager.Instance.UrlBuilder.HostName;
            string[] hostNameParts = hostName.Split(new char[] { '.' });
            if (hostNameParts.Length < 3) {
                Assert.Ignore("Skipping test: Cookies can only be set on fully-qualified domain names.");
            }

            await AssertCookieIsNotPresentWithName("name");

            Regex replaceRegex = new Regex(".*?\\.");
            string shorter = replaceRegex.Replace(_hostname, ".", 1);
            Cookie cookie = new Cookie("name", "value", shorter, "/", GetTimeInTheFuture());

            await driver.Options().Cookies.AddCookie(cookie);

            await AssertCookieIsPresentWithName("name");
        }

        [Test]
        public async Task ShouldNotGetCookiesRelatedToCurrentDomainWithoutLeadingPeriod()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            string cookieName = "name";
            await AssertCookieIsNotPresentWithName(cookieName);

            Regex replaceRegex = new Regex(".*?\\.");
            string subdomain = replaceRegex.Replace(_hostname, "subdomain.", 1);
            Cookie cookie = new Cookie(cookieName, "value", subdomain, "/", GetTimeInTheFuture());

            string originalUrl = await driver.GetUrl();
            string subdomainUrl = originalUrl.Replace(_hostname, subdomain);
            await driver.GoToUrl(subdomainUrl);
            await driver.Options().Cookies.AddCookie(cookie);

            await driver.GoToUrl(originalUrl);
            await AssertCookieIsNotPresentWithName(cookieName);
        }

        [Test]
        public async Task ShouldBeAbleToIncludeLeadingPeriodInDomainName()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            // Cookies cannot be set on domain names with less than 2 dots, so
            // localhost is out. If we are in that boat, bail the test.
            string hostName = EnvironmentManager.Instance.UrlBuilder.HostName;
            string[] hostNameParts = hostName.Split(new char[] { '.' });
            if (hostNameParts.Length < 3) {
                Assert.Ignore("Skipping test: Cookies can only be set on fully-qualified domain names.");
            }

            await AssertCookieIsNotPresentWithName("name");

            // Replace the first part of the name with a period
            Regex replaceRegex = new Regex(".*?\\.");
            string shorter = replaceRegex.Replace(_hostname, ".", 1);
            Cookie cookie = new Cookie("name", "value", shorter, "/", DateTime.Now.AddSeconds(100000));

            await driver.Options().Cookies.AddCookie(cookie);

            await AssertCookieIsPresentWithName("name");
        }

        [Test]
        public async Task ShouldBeAbleToSetDomainToTheCurrentDomain()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            // Cookies cannot be set on domain names with less than 2 dots, so
            // localhost is out. If we are in that boat, bail the test.
            string hostName = EnvironmentManager.Instance.UrlBuilder.HostName;
            string[] hostNameParts = hostName.Split(new char[] { '.' });
            if (hostNameParts.Length < 3) {
                Assert.Ignore("Skipping test: Cookies can only be set on fully-qualified domain names.");
            }

            Uri url = new Uri(await driver.GetUrl());
            String host = url.Host + ":" + url.Port.ToString();

            Cookie cookie1 = new Cookie("fish", "cod", host, "/", null);
            IOptions options = driver.Options();
            await options.Cookies.AddCookie(cookie1);

            await driver.GoToUrl(javascriptPage);
            ReadOnlyCollection<Cookie> cookies = await options.Cookies.AllCookies();
            Assert.That(cookies, Does.Contain(cookie1));
        }

        [Test]
        public async Task ShouldWalkThePathToDeleteACookie()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            string basePath = EnvironmentManager.Instance.UrlBuilder.Path;

            Cookie cookie1 = new Cookie("fish", "cod");
            await driver.Options().Cookies.AddCookie(cookie1);
            int count = await driver.Options().Cookies.AllCookies().Count();

            await driver.GoToUrl(childPage);
            Cookie cookie2 = new Cookie("rodent", "hamster", "/" + basePath + "/child");
            await driver.Options().Cookies.AddCookie(cookie2);
            count = await driver.Options().Cookies.AllCookies().Count();

            await driver.GoToUrl(grandchildPage);
            Cookie cookie3 = new Cookie("dog", "dalmation", "/" + basePath + "/child/grandchild/");
            await driver.Options().Cookies.AddCookie(cookie3);
            count = await driver.Options().Cookies.AllCookies().Count();

            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("child/grandchild"));
            await driver.Options().Cookies.DeleteCookieNamed("rodent");
            count = await driver.Options().Cookies.AllCookies().Count();

            Assert.That(driver.Options().Cookies.GetCookieNamed("rodent"), Is.Null);

            ReadOnlyCollection<Cookie> cookies = await driver.Options().Cookies.AllCookies();
            Assert.That(cookies, Has.Count.EqualTo(2));
            Assert.That(cookies, Does.Contain(cookie1));
            Assert.That(cookies, Does.Contain(cookie3));

            await driver.Options().Cookies.DeleteAllCookies();
            await driver.GoToUrl(grandchildPage);
            await AssertNoCookiesArePresent();
        }

        [Test]
        public async Task ShouldIgnoreThePortNumberOfTheHostWhenSettingTheCookie()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            // Cookies cannot be set on domain names with less than 2 dots, so
            // localhost is out. If we are in that boat, bail the test.
            string hostName = EnvironmentManager.Instance.UrlBuilder.HostName;
            string[] hostNameParts = hostName.Split(new char[] { '.' });
            if (hostNameParts.Length < 3) {
                Assert.Ignore("Skipping test: Cookies can only be set on fully-qualified domain names.");
            }

            Uri uri = new Uri(await driver.GetUrl());
            string host = $"{uri.Host}:{uri.Port}";
            string cookieName = "name";
            await AssertCookieIsNotPresentWithName(cookieName);
            Cookie cookie = new Cookie(cookieName, "value", host, "/", null);
            await driver.Options().Cookies.AddCookie(cookie);
            await AssertCookieIsPresentWithName(cookieName);
        }

        [Test]
        public async Task CookieEqualityAfterSetAndGet()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            string url = EnvironmentManager.Instance.UrlBuilder.WhereElseIs("animals");

            await driver.GoToUrl(url);
            await driver.Options().Cookies.DeleteAllCookies();

            DateTime time = DateTime.Now.AddDays(1);
            Cookie cookie1 = new Cookie("fish", "cod", null, "/common/animals", time);
            IOptions options = driver.Options();
            await options.Cookies.AddCookie(cookie1);

            ReadOnlyCollection<Cookie> cookies = await options.Cookies.AllCookies();
            Cookie retrievedCookie = null;
            foreach (Cookie tempCookie in cookies) {
                if (cookie1.Equals(tempCookie)) {
                    retrievedCookie = tempCookie;
                    break;
                }
            }

            Assert.That(retrievedCookie, Is.Not.Null);
            //Cookie.equals only compares name, domain and path
            Assert.AreEqual(cookie1, retrievedCookie);
        }

        [Test]
        public async Task ShouldRetainCookieExpiry()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            string url = EnvironmentManager.Instance.UrlBuilder.WhereElseIs("animals");

            await driver.GoToUrl(url);
            await driver.Options().Cookies.DeleteAllCookies();

            // DateTime.Now contains milliseconds; the returned cookie expire date
            // will not. So we need to truncate the milliseconds.
            DateTime current = DateTime.Now;
            DateTime expireDate = new DateTime(current.Year, current.Month, current.Day, current.Hour, current.Minute, current.Second, DateTimeKind.Local).AddDays(1);

            Cookie addCookie = new Cookie("fish", "cod", "/common/animals", expireDate);
            IOptions options = driver.Options();
            await options.Cookies.AddCookie(addCookie);

            Cookie retrieved = await options.Cookies.GetCookieNamed("fish");
            Assert.That(retrieved, Is.Not.Null);
            Assert.AreEqual(addCookie.Expiry, retrieved.Expiry, "Cookies are not equal");
        }

        [Test]
        public async Task CanHandleSecureCookie()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIsSecure("animals"));

            Cookie addedCookie = new ReturnedCookie("fish", "cod", null, "/common/animals", null, true, false);
            await driver.Options().Cookies.AddCookie(addedCookie);

            await driver.Navigate().Refresh();

            Cookie retrieved = await driver.Options().Cookies.GetCookieNamed("fish");
            Assert.That(retrieved, Is.Not.Null);
        }

        [Test]
        public async Task ShouldRetainCookieSecure()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIsSecure("animals"));

            ReturnedCookie addedCookie = new ReturnedCookie("fish", "cod", string.Empty, "/common/animals", null, true, false);

            await driver.Options().Cookies.AddCookie(addedCookie);

            await driver.Navigate().Refresh();

            Cookie retrieved = await driver.Options().Cookies.GetCookieNamed("fish");
            Assert.That(retrieved, Is.Not.Null);
            Assert.That(retrieved.Secure, "Secure attribute not set to true");
        }

        [Test]
        public async Task CanHandleHttpOnlyCookie()
        {
            StringBuilder url = new StringBuilder(EnvironmentManager.Instance.UrlBuilder.WhereIs("cookie"));
            url.Append("?action=add");
            url.Append("&name=").Append("fish");
            url.Append("&value=").Append("cod");
            url.Append("&path=").Append("/common/animals");
            url.Append("&httpOnly=").Append("true");

            await driver.GoToUrl(url.ToString());

            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("animals"));
            Cookie retrieved = await driver.Options().Cookies.GetCookieNamed("fish");
            Assert.That(retrieved, Is.Not.Null);
        }

        [Test]
        public async Task ShouldRetainHttpOnlyFlag()
        {
            StringBuilder url = new StringBuilder(EnvironmentManager.Instance.UrlBuilder.WhereElseIs("cookie"));
            url.Append("?action=add");
            url.Append("&name=").Append("fish");
            url.Append("&value=").Append("cod");
            url.Append("&path=").Append("/common/animals");
            url.Append("&httpOnly=").Append("true");

            await driver.GoToUrl(url.ToString());

            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereElseIs("animals"));

            Cookie retrieved = await driver.Options().Cookies.GetCookieNamed("fish");
            Assert.That(retrieved, Is.Not.Null);
            Assert.That(retrieved.IsHttpOnly, "HttpOnly attribute not set to true");
        }

        [Test]
        public async Task SettingACookieThatExpiredInThePast()
        {
            string url = EnvironmentManager.Instance.UrlBuilder.WhereElseIs("animals");

            await driver.GoToUrl(url);
            await driver.Options().Cookies.DeleteAllCookies();

            DateTime expires = DateTime.Now.AddSeconds(-1000);
            Cookie cookie = new Cookie("expired", "yes", "/common/animals", expires);
            IOptions options = driver.Options();
            await options.Cookies.AddCookie(cookie);

            cookie = await options.Cookies.GetCookieNamed("expired");
            Assert.That(cookie, Is.Null, "Cookie expired before it was set, so nothing should be returned: " + cookie);
        }

        [Test]
        public async Task CanSetCookieWithoutOptionalFieldsSet()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            string key = GenerateUniqueKey();
            string value = "foo";
            Cookie cookie = new Cookie(key, value);
            await AssertCookieIsNotPresentWithName(key);

            await driver.Options().Cookies.AddCookie(cookie);

            await AssertCookieHasValue(key, value);
        }

        [Test]
        public async Task DeleteNotExistedCookie()
        {
            String key = GenerateUniqueKey();
            await AssertCookieIsNotPresentWithName(key);

            await driver.Options().Cookies.DeleteCookieNamed(key);
        }

        [Test]
        public async Task DeleteAllCookiesDifferentUrls()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            Cookie cookie1 = new Cookie("fish1", "cod", EnvironmentManager.Instance.UrlBuilder.HostName, null, null);
            Cookie cookie2 = new Cookie("fish2", "tune", EnvironmentManager.Instance.UrlBuilder.AlternateHostName, null, null);

            string url1 = EnvironmentManager.Instance.UrlBuilder.WhereIs("");
            string url2 = EnvironmentManager.Instance.UrlBuilder.WhereElseIs("");

            IOptions options = driver.Options();

            await options.Cookies.AddCookie(cookie1);
            await AssertCookieIsPresentWithName(cookie1.Name);

            await driver.GoToUrl(url2);
            await options.Cookies.AddCookie(cookie2);
            await AssertCookieIsNotPresentWithName(cookie1.Name);
            await AssertCookieIsPresentWithName(cookie2.Name);

            await driver.GoToUrl(url1);
            await AssertCookieIsPresentWithName(cookie1.Name);
            await AssertCookieIsNotPresentWithName(cookie2.Name);

            await options.Cookies.DeleteAllCookies();
            await AssertCookieIsNotPresentWithName(cookie1.Name);

            await driver.GoToUrl(url2);
            await AssertCookieIsPresentWithName(cookie2.Name);
        }

        //------------------------------------------------------------------
        // Tests below here are not included in the Java test suite
        //------------------------------------------------------------------
        [Test]
        public async Task CanSetCookiesOnADifferentPathOfTheSameHost()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            string basePath = EnvironmentManager.Instance.UrlBuilder.Path;
            Cookie cookie1 = new Cookie("fish", "cod", "/" + basePath + "/animals");
            Cookie cookie2 = new Cookie("planet", "earth", "/" + basePath + "/galaxy");

            IOptions options = driver.Options();
            ReadOnlyCollection<Cookie> count = await options.Cookies.AllCookies();

            await options.Cookies.AddCookie(cookie1);
            await options.Cookies.AddCookie(cookie2);

            string url = EnvironmentManager.Instance.UrlBuilder.WhereIs("animals");
            await driver.GoToUrl(url);
            ReadOnlyCollection<Cookie> cookies = await options.Cookies.AllCookies();

            Assert.That(cookies, Does.Contain(cookie1));
            Assert.That(cookies, Does.Not.Contain(cookie2));

            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("galaxy"));
            cookies = await options.Cookies.AllCookies();
            Assert.That(cookies, Does.Not.Contain(cookie1));
            Assert.That(cookies, Does.Contain(cookie2));
        }

        [Test]
        public async Task ShouldNotBeAbleToSetDomainToSomethingThatIsUnrelatedToTheCurrentDomain()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            Cookie cookie1 = new Cookie("fish", "cod");
            IOptions options = driver.Options();
            await options.Cookies.AddCookie(cookie1);

            string url = EnvironmentManager.Instance.UrlBuilder.WhereElseIs("simpleTest.html");
            await driver.GoToUrl(url);

            Assert.That(options.Cookies.GetCookieNamed("fish"), Is.Null);
        }

        [Test]
        public async Task GetCookieDoesNotRetriveBeyondCurrentDomain()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            Cookie cookie1 = new Cookie("fish", "cod");
            IOptions options = driver.Options();
            await options.Cookies.AddCookie(cookie1);

            String url = EnvironmentManager.Instance.UrlBuilder.WhereElseIs("");
            await driver.GoToUrl(url);

            ReadOnlyCollection<Cookie> cookies = await options.Cookies.AllCookies();
            Assert.That(cookies, Does.Not.Contain(cookie1));
        }

        [Test]
        public async Task ShouldAddCookieToCurrentDomainAndPath()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            // Cookies cannot be set on domain names with less than 2 dots, so
            // localhost is out. If we are in that boat, bail the test.
            string hostName = EnvironmentManager.Instance.UrlBuilder.HostName;
            string[] hostNameParts = hostName.Split(new char[] { '.' });
            if (hostNameParts.Length < 3) {
                Assert.Ignore("Skipping test: Cookies can only be set on fully-qualified domain names.");
            }

            await driver.GoToUrl(macbethPage);
            IOptions options = driver.Options();
            Cookie cookie = new Cookie("Homer", "Simpson", _hostname, "/" + EnvironmentManager.Instance.UrlBuilder.Path, null);
            await options.Cookies.AddCookie(cookie);
            ReadOnlyCollection<Cookie> cookies = await options.Cookies.AllCookies();
            Assert.That(cookies.Contains(cookie), "Valid cookie was not returned");
        }

        [Test]
        public async Task ShouldNotShowCookieAddedToDifferentDomain()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                Assert.Ignore("Not on a standard domain for cookies (localhost doesn't count).");
            }

            await driver.GoToUrl(macbethPage);
            IOptions options = driver.Options();
            Cookie cookie = new Cookie("Bart", "Simpson", EnvironmentManager.Instance.UrlBuilder.HostName + ".com", EnvironmentManager.Instance.UrlBuilder.Path, null);
            Assert.That(() => options.Cookies.AddCookie(cookie), Throws.InstanceOf<WebDriverException>().Or.InstanceOf<InvalidOperationException>());
            ReadOnlyCollection<Cookie> cookies = await options.Cookies.AllCookies();
            Assert.That(cookies, Does.Not.Contain(cookie), "Invalid cookie was returned");
        }

        [Test]
        public async Task ShouldNotShowCookieAddedToDifferentPath()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            // Cookies cannot be set on domain names with less than 2 dots, so
            // localhost is out. If we are in that boat, bail the test.
            string hostName = EnvironmentManager.Instance.UrlBuilder.HostName;
            string[] hostNameParts = hostName.Split(new char[] { '.' });
            if (hostNameParts.Length < 3) {
                Assert.Ignore("Skipping test: Cookies can only be set on fully-qualified domain names.");
            }

            await driver.GoToUrl(macbethPage);
            IOptions options = driver.Options();
            Cookie cookie = new Cookie("Lisa", "Simpson", EnvironmentManager.Instance.UrlBuilder.HostName, "/" + EnvironmentManager.Instance.UrlBuilder.Path + "IDoNotExist", null);
            await options.Cookies.AddCookie(cookie);
            ReadOnlyCollection<Cookie> cookies = await options.Cookies.AllCookies();
            Assert.That(cookies, Does.Not.Contain(cookie), "Invalid cookie was returned");
        }

        [Test]
        public async Task ShouldThrowExceptionWhenAddingCookieToNonExistingDomain()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            await driver.GoToUrl(macbethPage);
            await driver.GoToUrl("http://nonexistent-origin.seleniumhq-test.test");
            IOptions options = driver.Options();
            Cookie cookie = new Cookie("question", "dunno");
            //Assert.That(() => options.Cookies.AddCookie(cookie), Throws.InstanceOf<InvalidCookieDomainException>().Or.InstanceOf<InvalidOperationException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await options.Cookies.AddCookie(cookie),
                exception => Assert.AreEqual("InvalidCookieDomainException", exception.Error));
        }

        [Test]
        public async Task ShouldReturnNullBecauseCookieRetainsExpiry()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            string url = EnvironmentManager.Instance.UrlBuilder.WhereElseIs("animals");
            await driver.GoToUrl(url);

            await driver.Options().Cookies.DeleteAllCookies();

            Cookie addCookie = new Cookie("fish", "cod", "/common/animals", DateTime.Now.AddHours(-1));
            IOptions options = driver.Options();
            await options.Cookies.AddCookie(addCookie);

            Cookie retrieved = await options.Cookies.GetCookieNamed("fish");
            Assert.That(retrieved, Is.Null);
        }

        [Test]
        public async Task ShouldAddCookieToCurrentDomain()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            await driver.GoToUrl(macbethPage);
            IOptions options = driver.Options();
            Cookie cookie = new Cookie("Marge", "Simpson", "/");
            await options.Cookies.AddCookie(cookie);
            ReadOnlyCollection<Cookie> cookies = await options.Cookies.AllCookies();
            Assert.That(cookies.Contains(cookie), "Valid cookie was not returned");
        }

        [Test]
        public async Task ShouldDeleteCookie()
        {
            if (!CheckIsOnValidHostNameForCookieTests()) {
                return;
            }

            await driver.GoToUrl(macbethPage);
            IOptions options = driver.Options();
            Cookie cookieToDelete = new Cookie("answer", "42");
            Cookie cookieToKeep = new Cookie("canIHaz", "Cheeseburguer");
            await options.Cookies.AddCookie(cookieToDelete);
            await options.Cookies.AddCookie(cookieToKeep);
            ReadOnlyCollection<Cookie> cookies = await options.Cookies.AllCookies();
            await options.Cookies.DeleteCookie(cookieToDelete);
            ReadOnlyCollection<Cookie> cookies2 = await options.Cookies.AllCookies();
            Assert.That(cookies2, Does.Not.Contain(cookieToDelete), "Cookie was not deleted successfully");
            Assert.That(cookies2.Contains(cookieToKeep), "Valid cookie was not returned");
        }

        //////////////////////////////////////////////
        // Support functions
        //////////////////////////////////////////////

        private async Task GotoValidDomainAndClearCookies(string page)
        {
            _hostname = null;
            String hostname = EnvironmentManager.Instance.UrlBuilder.HostName;
            if (IsValidHostNameForCookieTests(hostname)) {
                _isOnAlternativeHostName = false;
                _hostname = hostname;
            }

            hostname = EnvironmentManager.Instance.UrlBuilder.AlternateHostName;
            if (_hostname == null && IsValidHostNameForCookieTests(hostname)) {
                _isOnAlternativeHostName = true;
                _hostname = hostname;
            }

            await GoToPage(page);

            await driver.Options().Cookies.DeleteAllCookies();
        }

        private bool CheckIsOnValidHostNameForCookieTests()
        {
            bool correct = _hostname != null && IsValidHostNameForCookieTests(_hostname);
            if (!correct) {
                Console.WriteLine("Skipping test: unable to find domain name to use");
            }

            return correct;
        }

        private async Task GoToPage(String pageName)
        {
            await driver.GoToUrl(_isOnAlternativeHostName ? EnvironmentManager.Instance.UrlBuilder.WhereElseIs(pageName) : EnvironmentManager.Instance.UrlBuilder.WhereIs(pageName));
        }

        private async Task GoToOtherPage(String pageName)
        {
            await driver.GoToUrl(_isOnAlternativeHostName ? EnvironmentManager.Instance.UrlBuilder.WhereIs(pageName) : EnvironmentManager.Instance.UrlBuilder.WhereElseIs(pageName));
        }

        private bool IsValidHostNameForCookieTests(string hostname)
        {
            // TODO(JimEvan): Some coverage is better than none, so we
            // need to ignore the fact that localhost cookies are problematic.
            // Reenable this when we have a better solution per DanielWagnerHall.
            // ChromeDriver2 has trouble with localhost. IE and Firefox don't.
            // return !IsIpv4Address(hostname) && "localhost" != hostname;
            bool isLocalHostOkay = "localhost" != hostname;

            return !IsIpv4Address(hostname) && isLocalHostOkay;
        }

        private static bool IsIpv4Address(string addrString)
        {
            return Regex.IsMatch(addrString, "\\d{1,3}(?:\\.\\d{1,3}){3}");
        }

        private string GenerateUniqueKey()
        {
            return $"key_{_random.Next()}";
        }

        private async Task<string> GetDocumentCookieOrNull()
        {
            IJavaScriptExecutor jsDriver = driver as IJavaScriptExecutor;
            if (jsDriver == null) {
                return null;
            }
            try {
                return (string)await jsDriver.ExecuteScript("return document.cookie");
            } catch (InvalidOperationException) {
                return null;
            }
        }

        private async Task AssertNoCookiesArePresent()
        {
            Assert.That(await driver.Options().Cookies.AllCookies().Count(), Is.EqualTo(0), "Cookies were not empty");
            string documentCookie = await GetDocumentCookieOrNull();
            if (documentCookie != null) {
                Assert.AreEqual(string.Empty, documentCookie, "Cookies were not empty");
            }
        }

        private async Task AssertSomeCookiesArePresent()
        {
            Assert.That(await driver.Options().Cookies.AllCookies().Count(), Is.Not.EqualTo(0), "Cookies were empty");
            String documentCookie = await GetDocumentCookieOrNull();
            if (documentCookie != null) {
                Assert.AreNotEqual(string.Empty, documentCookie, "Cookies were empty");
            }
        }

        private async Task AssertCookieIsNotPresentWithName(string key)
        {
            Assert.That(driver.Options().Cookies.GetCookieNamed(key), Is.Null, "Cookie was present with name " + key);
            string documentCookie = await GetDocumentCookieOrNull();
            if (documentCookie != null) {
                Assert.That(documentCookie, Does.Not.Contain(key + "="));
            }
        }

        private async Task AssertCookieIsPresentWithName(string key)
        {
            Assert.That(driver.Options().Cookies.GetCookieNamed(key), Is.Not.Null, "Cookie was present with name " + key);
            string documentCookie = await GetDocumentCookieOrNull();
            if (documentCookie != null) {
                Assert.That(documentCookie, Does.Contain(key + "="));
            }
        }

        private async Task AssertCookieHasValue(string key, string value)
        {
            Assert.AreEqual(value, driver.Options().Cookies.GetCookieNamed(key).Result.Value, "Cookie had wrong value");
            string documentCookie = await GetDocumentCookieOrNull();
            if (documentCookie != null) {
                Assert.That(documentCookie, Does.Contain(key + "=" + value));
            }
        }

        private DateTime GetTimeInTheFuture()
        {
            return DateTime.Now.Add(TimeSpan.FromMilliseconds(100000));
        }
    }
}
