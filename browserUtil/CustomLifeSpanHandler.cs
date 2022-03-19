using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.WinForms;

namespace StreamerPlusApp.browserUtil
{

    public class CustomLifeSpanHandler : ILifeSpanHandler
    {
        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            return false;
        }

        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            if (chromiumWebBrowser != null)
            {
                chromiumWebBrowser.RequestHandler = new UA_RequestHandler();
                chromiumWebBrowser.LifeSpanHandler = new CustomLifeSpanHandler();
            }
        }

        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            
        }

        public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;
            if (chromiumWebBrowser != null)
            {
                chromiumWebBrowser.RequestHandler = new UA_RequestHandler();
                chromiumWebBrowser.LifeSpanHandler = new CustomLifeSpanHandler();
            }
            return false;
        }
    }
}
