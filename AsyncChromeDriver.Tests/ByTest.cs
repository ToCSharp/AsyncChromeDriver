using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Zu.AsyncWebDriver;
using Zu.AsyncWebDriver.Internal;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class ByTest
    {
        [Test]
        public async Task ShouldUseFindsByNameToLocateElementsByName() 
        {
            var mockDriver = new Mock<IAllDriver>();
            var mockElement = new Mock<IWebElement>();
            List<(Task<IWebElement>, string, CancellationToken)> queue = new List<(Task<IWebElement>, string, CancellationToken)>();

            mockDriver.Setup(_ => _.FindElementByName(It.Is<string>(x => x == "cheese"), new CancellationToken())).ReturnsAsync(mockElement.Object);

            By by = By.Name("cheese");
            var element = await by.FindElement(mockDriver.Object);

            Assert.AreEqual(mockElement.Object, element);
            mockDriver.Verify(x => x.FindElementByName("cheese", new CancellationToken()), Times.Once);
        }

        public async Task ShouldUseXPathToFindByNameIfDriverDoesNotImplementFindsByName()
        {
            var mockDriver = new Mock<IOnlyXPath>();
            var mockElement = new Mock<IWebElement>();

            mockDriver.Setup(_ => _.FindElementByXPath(It.Is<string>(x => x == "//*[@name='cheese']"), new CancellationToken())).ReturnsAsync(mockElement.Object);

            By by = By.Name("cheese");
            var element = await by.FindElement(mockDriver.Object);
            Assert.AreEqual(mockElement.Object, element);
        }

        public interface IAllDriver : IFindsById, IFindsByLinkText, IFindsByName, IFindsByXPath, ISearchContext
        {
        }

        public interface IOnlyXPath : IFindsByXPath, ISearchContext
        {
        }
    }
}
