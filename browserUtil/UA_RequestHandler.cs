using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;

namespace StreamerPlusApp.browserUtil
{
    class UA_RequestHandler : CefSharp.Handler.RequestHandler
    {
        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return new CustomResourceRequestHandler();
        }
    }
    public class CustomResourceRequestHandler : CefSharp.Handler.ResourceRequestHandler
    {
        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            if (request == null || chromiumWebBrowser == null)
                return CefReturnValue.Continue;

            var headers = request.Headers;
            if (chromiumWebBrowser.Address != null &&
                (chromiumWebBrowser.Address.Contains(Urls.youtube["loginBase_google"]) ||
                chromiumWebBrowser.Address.Contains(Urls.youtube["loginBase_youtube"])))
            {
                headers["User-Agent"] = " ";
            }
            else
                headers["User-Agent"] = BrowserFlow.UA;


            request.Headers = headers;
            return CefReturnValue.Continue;
        }
    }

}


