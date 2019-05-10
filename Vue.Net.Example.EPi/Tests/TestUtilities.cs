using System.IO;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;
using Moq;

namespace Vue.Net.Tests.EPi
{
    internal static class TestUtilities
    {
        public static HtmlHelper<T> CreateHtmlHelper<T>(ViewDataDictionary viewData = null)
        {
            if (viewData == null)
            {
                viewData = new ViewDataDictionary();
            }
            var cc = new Mock<ControllerContext>(
                new Mock<HttpContextBase>().Object,
                new RouteData(),
                new Mock<ControllerBase>().Object);

            var mockViewContext = new Mock<ViewContext>(
                cc.Object,
                new Mock<IView>().Object,
                viewData,
                new TempDataDictionary(),
                TextWriter.Null);

            var mockViewDataContainer = new Mock<IViewDataContainer>();

            mockViewDataContainer.Setup(v => v.ViewData).Returns(viewData);

            return new HtmlHelper<T>(mockViewContext.Object, mockViewDataContainer.Object);
        }
    }
}
