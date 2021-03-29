using CefSharp;
using CefSharp.Handler;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace StreamerPlusApp
{
    public class subCount
    {
        public Server localServer;
        public ChromiumWebBrowser browser;
        private string subcount_detect_js;

        public subCount()
        {
            subcount_detect_js = @"
var detector = 'div.metric-value-big.style-scope.ytcd-channel-facts-item';// per refresh selector
var timoutSec = 15 * 1000;
var loadOnce = false;
function FullyLoadedCheckAndAction()
{
    var elemntContainer = document.querySelector(detector);
    if(elemntContainer != null)
    { 
        var rawHtml = elemntContainer.innerHTML;
        var res = rawHtml.replace(/\D/g,'');
        res = Number(res);
        //change url
        window.history.pushState(null, null, '?' + res);

        console.log('Update');
        setTimeout(()=>{location.reload();}, timoutSec);
    }
    else
    {
            TimeOutCall();
    }
}

function TimeOutCall()
{
    setTimeout(FullyLoadedCheckAndAction, 300);
}

(function() {
    if(loadOnce == false)
    {
        loadOnce = true;
        TimeOutCall();
    }
})();";
            BrowserSettings offscreen_setting = new BrowserSettings();
            offscreen_setting.DefaultEncoding = "UTF-8";
            offscreen_setting.WindowlessFrameRate = 1;
            offscreen_setting.WebSecurity = CefState.Disabled;

            this.browser = new CefSharp.OffScreen.ChromiumWebBrowser("https://studio.youtube.com/", offscreen_setting);
            //this.browser.Size = new Size(1, 1);
            //will get refreshed and change the url to contain the subcount
            this.browser.AddressChanged += new EventHandler<AddressChangedEventArgs>((object sender, AddressChangedEventArgs e) =>
            {
                if (e.Address.Contains("?"))
                {
                    string newSubcount = e.Address.Split('?')[1];
                    if(newSubcount != localServer.lastKnow_subcount.ToString())
                        this.localServer.BroadcastSubcount2Webscokets(newSubcount);
                }
            });
            this.browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>((object sender, FrameLoadEndEventArgs e) =>
            {
                if (!e.Frame.IsMain)
                    this.browser.ExecuteScriptAsync(subcount_detect_js);
            });
            this.localServer = new Server("11111");
            this.localServer.Start();
        }

        public void ReloadUrl()
        {
            this.browser.Load("https://studio.youtube.com/");
        }
    }

    #region server
    public class Server
    {
        private HttpListener _httpListener = new HttpListener();
        private Thread _responseThread;
        private bool isClosed = false;
        private List<WebSocketLite> userReciver;

        public int lastKnow_subcount = -1;

        public Server(string atPort = "11111")
        {
            _httpListener.Prefixes.Add("http://localhost:" + atPort + "/"); // add prefix "http://localhost:11111/"
            userReciver = new List<WebSocketLite>();
        }

        public void Start()
        {
            _httpListener.Start(); // start server (Run application as Administrator!)

            _responseThread = new Thread(ResponseThread);
            _responseThread.Start(); // start the response thread
        }
        public async void Close()
        {
            if (isClosed)
                return;
            this.isClosed = true;
            foreach (WebSocketLite ws_item in userReciver)
            {
                ws_item.Dispose();
            }
            try
            {
                _httpListener.Abort();
            }
            catch (Exception e) { }
        }

        private void ResponseThread()
        {
            while (this.isClosed == false)
            {
                try
                {
                    HttpListenerContext client_request = _httpListener.GetContext(); // get a context
                                                                                     // Now, you'll find the request URL in context.Request.Url
                    if (client_request.Request.IsWebSocketRequest)
                        this.HandleWebSocketReq(client_request);
                    else
                        this.HandleHttpReq(client_request);
                }
                catch (Exception e)
                {
                    if (this.isClosed)
                        return;
                }
            }
        }
        private void HandleHttpReq(HttpListenerContext client_request)
        {
            //if (browser != null && browser.IsBrowserInitialized)
            //    browser.Reload();

            byte[] _responseArray = Encoding.UTF8.GetBytes(@"<html>" + GenHeader() + "\n" + GenBody() + "</html>"); // get the bytes to response


            client_request.Response.ContentEncoding = Encoding.UTF8;           // this doesnt work??
            client_request.Response.ContentType = "text/html; charset=utf-8"; //Fixxxx
            client_request.Response.OutputStream.Write(_responseArray, 0, _responseArray.Length); // write bytes to the output stream
            client_request.Response.KeepAlive = false; // set the KeepAlive bool to false
            client_request.Response.Close(); // close the connection
        }
        private async void HandleWebSocketReq(HttpListenerContext client_request)
        {
            WebSocketContext webSocketContext = null;

            //accept weobsocket connection
            string protocol = client_request.Request.Headers.Get("Sec-WebSocket-Protocol");
            try { webSocketContext = await client_request.AcceptWebSocketAsync(subProtocol: protocol); }
            catch (Exception e)
            {//if error send error header and exit
                client_request.Response.StatusCode = 500;
                client_request.Response.StatusDescription = e.ToString();
                client_request.Response.Close();
                return;
            }
            WebSocket webSocket = webSocketContext.WebSocket;//get the web socket itself
            WebSocketLite ws = new WebSocketLite(webSocket);
            ws.OnMessage += new EventHandler<OnMessageEventArgs>((object sender, OnMessageEventArgs e) =>
            {
                WebSocketLite caller = (WebSocketLite)sender;
                caller.Send("{\"error\":\"you cannot send data to the server! only get it..\"}");
            });

            ws.Send(SubcountJSON_msg());

            userReciver.RemoveAll(item => item == null);
            if (!userReciver.Contains(ws))
            {
                userReciver.Add(ws);
            }

        }

        public void BroadcastSubcount2Webscokets(string count)
        {
            userReciver.RemoveAll(item => item == null);
            long subcount = -1;
            bool canConvert = long.TryParse(count, out subcount);
            this.lastKnow_subcount = (int)subcount;
            foreach (WebSocketLite ws_item in userReciver)
            {
                ws_item.Send(SubcountJSON_msg());
            }
        }

        private string SubcountJSON_msg()
        {
            return "{\"subcount\":" + this.lastKnow_subcount.ToString() + "}";
        }

        #region generator html
        public string GenHeader()
        {
            string str = @"<head><title>כמות רשומים - נסיוני</title><meta charset='UTF - 8'>" + GenCSS() + GenScript() + @"</head>";
            return str;
        }

        public string GenCSS()
        {

            return "<style>" + System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\SubCount\style.css") + "</style>";

        }

        public string GenScript()
        {
            return "<script id='remove'>" + System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\SubCount\script.js") + "</script>";
        }

        public string GenBody()
        {
            return System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\SubCount\body.html");
        }
        #endregion
    }
    public class WebSocketLite
    {
        WebSocket webSocket;
        public string Protocol
        {
            get
            {
                return webSocket.SubProtocol;
            }
        }
        public WebSocketLite(WebSocket ws)
        {
            this.webSocket = ws;
            this.HandleConnection();
        }
        private async void HandleConnection()
        {
            byte[] receiveBuffer = new byte[1024];//this will hold the data we receive from the websocket
            while (webSocket.State == WebSocketState.Open)
            {
                //gets the data from the websocket req
                WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                if (receiveResult.MessageType == WebSocketMessageType.Text)
                {
                    this.WebSocketResponse(webSocket, receiveBuffer);
                }
                else if (receiveResult.MessageType == WebSocketMessageType.Close)//if its req to close connection
                { await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None); }
                else
                { await webSocket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "", CancellationToken.None); }


            }
            if (webSocket != null)
                this.Dispose();
        }

        private async void WebSocketResponse(WebSocket webSocket, byte[] receiveBuffer)
        {
            string client_msg = Encoding.ASCII.GetString(receiveBuffer);
            this.OnMessageTrigger(client_msg);
        }

        protected virtual void OnMessageTrigger(string msg)
        {
            OnMessageEventArgs args = new OnMessageEventArgs();
            args.msg = msg;
            EventHandler<OnMessageEventArgs> handler = this.OnMessage;
            if (handler != null)
            {
                handler(this, args);
            }
        }
        public event EventHandler<OnMessageEventArgs> OnMessage;

        public async void Send(string data)
        {
            if (webSocket == null)
                return;
            byte[] responseBuffer = Encoding.ASCII.GetBytes(data);
            await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async void Dispose()
        {
            await this.webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "server is closed", CancellationToken.None);
            this.webSocket.Abort();
            this.webSocket.Dispose();
        }
    }
    public class OnMessageEventArgs : EventArgs
    {
        public string msg { get; set; }
    }
    #endregion
}
