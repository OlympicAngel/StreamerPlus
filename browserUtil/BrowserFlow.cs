using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Drawing;

namespace StreamerPlusApp
{
    public static class BrowserFlow
    {
        static Dictionary<string, ChromiumWebBrowser> cef = new Dictionary<string, ChromiumWebBrowser>(){
            { "youtube", null },
            { "streamlabs", null }
        };
        static Dictionary<string, string> js = new Dictionary<string, string>(){
            { "youtube", null },
            { "streamlabs", null }
        };
        static Thread wait_15sec;


        public static Main mainForm_ref;

        public static void INI(Main mainRef, ChromiumWebBrowser youtube, ChromiumWebBrowser streamlabs)
        {
            css_js.INI();
            mainForm_ref = mainRef;
            cef["youtube"] = youtube;
            cef["streamlabs"] = streamlabs;
            cef["youtube"].FrameLoadEnd += new System.EventHandler<CefSharp.FrameLoadEndEventArgs>(FrameLoadEnd);
            cef["streamlabs"].FrameLoadEnd += new System.EventHandler<CefSharp.FrameLoadEndEventArgs>(FrameLoadEnd);
            cef["youtube"].FrameLoadStart += new System.EventHandler<CefSharp.FrameLoadStartEventArgs>(FrameLoadStart);
            cef["streamlabs"].FrameLoadStart += new System.EventHandler<CefSharp.FrameLoadStartEventArgs>(FrameLoadStart);

            wait_15sec = new Thread(()=>{
                Thread.Sleep(25 * 1000);
                cef["youtube"].Load(Urls.youtube["chat"] + "LoginError");
                cef["streamlabs"].Load(Urls.streamlabs["events"]);
                MessageBox.Show("נראה שלוקח יותר מידי זמן להתחבר,\nמעביר אותך לדף הבית של סטרימר פלוס - יתכן שתצטרך להתחבר למשתמש שלך בהגדרות.","תקלה בהתחברות");
            });
            wait_15sec.Name = "Login Timout";
            wait_15sec.IsBackground = true;
            wait_15sec.Start();


            Safe.Invoke(() =>
            {
                mainForm_ref.ToggleLoading(2);
                mainForm_ref.loadingText.Text = "יוצר חיבור לגוגל..";
            });
            cef["streamlabs"].Load(Urls.youtube["dashboard"]);
        }

        public static void CefSettings()
        {
            #region CEF settings
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/StreamerPlusApp/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!Directory.Exists(path + "userData/"))
                Directory.CreateDirectory(path + "userData/");

            var settings = new CefSettings
            {
                WindowlessRenderingEnabled = true,
                BrowserSubprocessPath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, Environment.Is64BitProcess ? "x64" : "x86", "CefSharp.BrowserSubprocess.exe"),
                BackgroundColor = Util.ColorToUInt(System.Drawing.Color.FromArgb(255, 56, 56, 56)),
                CachePath = path,
                UserDataPath = path + "userData/",
                Locale = "he",
                ProductVersion = "1.0.0",
                RemoteDebuggingPort = 6968,
                PersistSessionCookies = true,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.141 Safari/537.36 Edg/81.0.416.72"
            };
            settings.CefCommandLineArgs.Add("--js-flags", "--max_old_space_size=500");
            settings.CefCommandLineArgs.Add("--multi-threaded-message-loop", "1");
            settings.CefCommandLineArgs.Remove("process-per-tab");
            settings.CefCommandLineArgs.Add("enable-media-stream", "0");
            settings.CefCommandLineArgs.Add("disable-gpu-vsync", "1");
            settings.CefCommandLineArgs.Add("mute-audio", "true");
            settings.CefCommandLineArgs.Add("disable-3d-apis", "1");
            settings.CefCommandLineArgs.Add("renderer-process-limit", "10");
            settings.SetOffScreenRenderingBestPerformanceArgs();
            CefSharpSettings.SubprocessExitIfParentProcessClosed = true;
            CefSharpSettings.ShutdownOnExit = true;
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            if (resolution.Width >= 1440 || resolution.Height >= 1440)
            {
                Cef.EnableHighDPISupport();
            }
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
            #endregion
        }

        static void FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            string address = e.Browser.FocusedFrame.Url;
            if (address.Contains("/livestreaming") && address.Contains("/video/"))
            {
                Safe.Invoke(() => { mainForm_ref.loadingText.Text = "מזהה לייב פעיל..."; });
                return;
            }
            //if the page is livestream studio BEFORE the live actully loads in
            if (address.Contains("livestreaming"))
            {
                OnRawLiveStreamPage();
                return;
            }
        }

        static void FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            ChromiumWebBrowser browserRef = ((ChromiumWebBrowser)sender);
            string browser_name = (browserRef.Equals(cef["youtube"])) ? "youtube" : "streamlabs";
            Handle_delayed_JS(browser_name, e.Frame);
            browserRef.SetZoomLevel(Properties.Settings.Default.scale);//set the zoom level from settings

            //if current page is google account login
            if (browserRef.Address.Contains(Urls.youtube["minLoginURL"]))
            {
                OnGoogleLoginPage();
                return;
            }
            //if aspect ration doesnt match settings aspect
            if (mainForm_ref.tempRatio != double.Parse(Properties.Settings.Default["heightRatio"].ToString()))
            {
                Properties.Settings.Default.heightRatio = mainForm_ref.tempRatio;
                Properties.Settings.Default.Save();
                //Safe.Invoke(() => mainForm_ref.DynamicLayOut());
            }
            //if current page is some youtube page AND it was load after a RE-login action
            if (browserRef.Address == Urls.youtube["youtubeUrl"] && mainForm_ref.yt_relogin)
            {
                OnYoutubeReLoginPage(browserRef);
                return;
            }
            //if the page is livestream studio
            if (browserRef.Address.Contains("/livestreaming") && browserRef.Address.Contains("/video/"))
            {
                OnLiveStreamPage(browserRef);
                return;
            }
            if (browserRef.Address.Contains(Urls.youtube["chat"]) && !browserRef.Address.Contains("LoginError"))
            {
                OnChatPage(browserRef);
                return;
            }
            if (browserRef.Address.Contains(Urls.youtube["chat"]) && browserRef.Address.Contains("LoginError"))
            {
                Safe.Invoke(() => { mainForm_ref.ToggleLoading(1); });
                Inject_CSS("loginError", browserRef);
                return;
            }
        }


        #region js/css injection
        public static void Inject_JS(string script, ChromiumWebBrowser browser, bool onlyNow = false)
        {
            if (!script.Contains(" ") && css_js.js.ContainsKey(script))
                script = css_js.js[script];

            if (browser.CanExecuteJavascriptInMainFrame)
            {
                browser.ExecuteScriptAsyncWhenPageLoaded(script);
            }
            else if(onlyNow == false)
            {
                string browser_name = (browser.Equals(cef["youtube"])) ? "youtube" : "streamlabs";
                js[browser_name] = script;
            }
        }
        public static void Handle_delayed_JS(string browser_name,IFrame frame)
        {
            string browsers_script = js[browser_name];
            if (!string.IsNullOrEmpty(browsers_script))
                if (frame.IsMain)
                {
                    frame.ExecuteJavaScriptAsync(browsers_script);
                    js[browser_name] = "";
                }
        }
        public static void Inject_CSS(string css, ChromiumWebBrowser browser)
        {
            if (css == null || browser == null)
                return;
            if (!css.Contains(" ") && css_js.css.ContainsKey(css))
                css = css_js.css[css];

            string css_as_script = @"var wrap = document.createElement('style');
            wrap.innerHTML = '" + css.Replace(Environment.NewLine, "") + @"';
            document.getElementsByTagName('head')[0].appendChild(wrap)";
            Inject_JS(css_as_script, browser);
        }
        #endregion

        #region per page events
        static void OnGoogleLoginPage()
        {
            if(wait_15sec.IsAlive)
            {
                wait_15sec.Abort();
            }
            mainForm_ref.tempRatio = double.Parse(Properties.Settings.Default["heightRatio"].ToString());
            if (mainForm_ref.tempRatio == 0)
                mainForm_ref.tempRatio = 0.7;
            Properties.Settings.Default.heightRatio = 0;
            Properties.Settings.Default.Save();
            if(!mainForm_ref.yt_relogin)
            {
                Safe.Invoke(() =>
                {
                    mainForm_ref.ToggleLoading(1);
                    mainForm_ref.DynamicLayOut();
                });
            }
        }
        static void OnYoutubeReLoginPage(ChromiumWebBrowser browser)
        {
            Inject_JS("reLoginWait", browser);
            mainForm_ref.yt_relogin = false; //reset the re login blocking state
        }
        static void OnRawLiveStreamPage()
        {
            if (wait_15sec.IsAlive)
            {
                wait_15sec.Abort();
            }
            Safe.Invoke(() =>
            {
                mainForm_ref.ToggleLoading(2);
                mainForm_ref.loadingText.Text = "טוען משתמש...";
            });
        }
        static void OnLiveStreamPage(ChromiumWebBrowser browser)
        {
            mainForm_ref.subCount.ReloadUrl();
            string address = browser.Address;
            string liveID = "";
            if (address.Contains("video/"))
            {
                address = address.Split(new string[] { "video/" }, StringSplitOptions.RemoveEmptyEntries)[1];
                if (address.Contains("/livestreaming"))
                    liveID = address.Split(new string[] { "/livestreaming" }, StringSplitOptions.RemoveEmptyEntries)[0];
            }

            if (string.IsNullOrEmpty(liveID))
            {
                MessageBox.Show("המערכת לא הצליחה לזהות באופן אוטומטי את מזהה הלייב!\nכרגע הצאט לא יפעל - רצוי מאוד להכנס להגדרות ולהזין ידנית את מזהה הלייב!", "זהוי נכשל");
                liveID = "LoginError";
                mainForm_ref.chatID = "";
            }
            else
                mainForm_ref.chatID = liveID;


            Safe.Invoke(() =>
            {
                cef["streamlabs"].Load(Urls.streamlabs["events"]);
                cef["youtube"].Load(Urls.youtube["chat"] + liveID);
            });


        }
        static void OnChatPage(ChromiumWebBrowser browser)
        {
            Safe.Invoke(() => { mainForm_ref.ToggleLoading(1); });
            Inject_CSS("chat", browser);
        }
        #endregion
    }

    public static class FastBrowser
    {
        public static void ActionAt(string url, string js)
        {
            BrowserSettings offscreen_setting = new BrowserSettings();
            offscreen_setting.DefaultEncoding = "UTF-8";
            offscreen_setting.WindowlessFrameRate = 5;
            CefSharp.OffScreen.ChromiumWebBrowser tempBrowser = new CefSharp.OffScreen.ChromiumWebBrowser(url, offscreen_setting);
            tempBrowser.Size = new Size(10, 10);
            tempBrowser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>((Object sender, FrameLoadEndEventArgs e) =>
            {
                if (e.Url == Urls.olympicangel["event_end"])
                {
                    Safe.Invoke(() => {
                        tempBrowser.Dispose();
                    });
                    GC.Collect();
                    return;
                }

                if (e.Frame.IsMain)
                {
                    if (!js.Contains(" ") && css_js.js.ContainsKey(js))
                        js = css_js.js[js];

                    if (tempBrowser.CanExecuteJavascriptInMainFrame)
                    {
                        tempBrowser.ExecuteScriptAsyncWhenPageLoaded(js);
                    }
                }
            });
            //BrowserFlow.mainForm_ref.Controls.Add(tempBrowser);

        }
    }
}
