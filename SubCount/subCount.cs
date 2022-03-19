using CefSharp;
using CefSharp.Handler;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamerPlusApp
{
    public class SubCount : IDisposable
    {
#pragma warning disable CA1051 // Do not declare visible instance fields
        public Server localServer;
        public ChromiumWebBrowser browser;
        private string SubCount_detect_js;
#pragma warning restore CA1051 // Do not declare visible instance fields


        public SubCount()
        {
            SubCount_detect_js = @"
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
            BrowserSettings offscreen_setting = new BrowserSettings
            {
                DefaultEncoding = "UTF-8",
                WindowlessFrameRate = 1
            };

            this.browser = new CefSharp.OffScreen.ChromiumWebBrowser("https://studio.youtube.com/", offscreen_setting);
            this.browser.Size = new Size(1, 1);
            //will get refreshed and change the url to contain the SubCount
            this.browser.AddressChanged += new EventHandler<AddressChangedEventArgs>((object sender, AddressChangedEventArgs e) =>
            {
                if (e.Address.Contains("?"))
                {
                    string newSubCount = e.Address.Split('?')[1];
                    if (newSubCount != localServer.lastKnow_SubCount.ToString(new CultureInfo("en-US")) && this.localServer != null)
                        this.localServer.BroadcastSubCount2Webscokets(newSubCount);
                }
            });
            this.browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>((object sender, FrameLoadEndEventArgs e) =>
            {
                if (!e.Frame.IsMain)
                    this.browser.ExecuteScriptAsync(SubCount_detect_js);
            });
            this.localServer = new Server("11111");
            this.localServer.Start();
        }

        public void Dispose()
        {
            ((IDisposable)browser).Dispose();
            ((IDisposable)localServer).Dispose();
            GC.SuppressFinalize(this);
        }

        public void ReloadUrl()
        {
            this.browser.Load("https://studio.youtube.com/");
        }
    }

    #region server
    public class Server : IDisposable
    {
        private HttpListener _httpListener = new HttpListener();
        private bool isClosed;
        private List<WebSocketLite> userReciver;

#pragma warning disable CA1051 // Do not declare visible instance fields
        public int lastKnow_SubCount = -1;
#pragma warning restore CA1051 // Do not declare visible instance fields

        public Server(string atPort = "11111")
        {
            _httpListener.Prefixes.Add("http://localhost:" + atPort + "/"); // add prefix "http://localhost:11111/"
            userReciver = new List<WebSocketLite>();
        }

        public async void Start()
        {
            _httpListener.Start(); // start server (Run application as Administrator!)
            while (!this.isClosed) //if the server is open
            {
                HttpListenerContext client_request = await _httpListener.GetContextAsync().ConfigureAwait(true);
                try
                {
                    await ResponseThread(client_request).ConfigureAwait(true);
                }
                catch (Exception e)
                {
                    if (this.isClosed)
                        return;
                    MessageBox.Show(e.Message, "שגיאה");
                }
            }
        }
        public void Close()
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
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception) { }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        private async Task ResponseThread(HttpListenerContext client_request)
        {
            if (client_request.Request.IsWebSocketRequest)
                await HandleWebSocketReq(client_request).ConfigureAwait(true);
            else
                HandleHttpReq(client_request);
        }
        private void HandleHttpReq(HttpListenerContext client_request)
        {
            byte[] _responseArray = Encoding.UTF8.GetBytes(@"<html>" + GenHeader() + "\n" + GenBody() + "</html>"); // get the bytes to response
            client_request.Response.ContentEncoding = Encoding.UTF8;           // this doesnt work??
            client_request.Response.ContentType = "text/html; charset=utf-8"; //Fixxxx
            client_request.Response.OutputStream.Write(_responseArray, 0, _responseArray.Length); // write bytes to the output stream
            client_request.Response.KeepAlive = false; // set the KeepAlive bool to false
            client_request.Response.Close(); // close the connection
        }
        private async Task HandleWebSocketReq(HttpListenerContext client_request)
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

            ws.Send(SubCountJSON_msg());

            userReciver.RemoveAll(item => item == null || item.isDisposed);

            userReciver.Add(ws);

        }

        public void BroadcastSubCount2Webscokets(string count)
        {
            userReciver.RemoveAll(item => item == null);
            long SubCount = -1;
            bool canConvert = long.TryParse(count, out SubCount);
            this.lastKnow_SubCount = (int)SubCount;
            foreach (WebSocketLite ws_item in userReciver)
            {
                ws_item.Send(SubCountJSON_msg());
            }
        }

        private string SubCountJSON_msg()
        {
            return "{\"SubCount\":" + lastKnow_SubCount.ToString(new CultureInfo("en-US")) + "}";
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

        public void Dispose()
        {
            _httpListener = null;
            userReciver = null;
            GC.SuppressFinalize(this);
        }
    }
    public class WebSocketLite
    {
        WebSocket webSocket;
        public bool isDisposed;
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
            if (webSocket == null || this.isDisposed)
                return;
            byte[] responseBuffer = Encoding.ASCII.GetBytes(data);
            await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async void Dispose()
        {
            this.isDisposed = true;
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "server is closed", CancellationToken.None);
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
