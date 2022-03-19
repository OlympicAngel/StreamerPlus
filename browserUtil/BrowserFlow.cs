using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Drawing;
using StreamerPlusApp.browserUtil;
using System.Globalization;

namespace StreamerPlusApp
{
    public static class BrowserFlow
    {
        //public static string UA = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:90.0) Gecko/20100101 Firefox/90.0";
        public static string UA = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.74 Safari/537.36";
        static Dictionary<string, ChromiumWebBrowser> cef = new Dictionary<string, ChromiumWebBrowser>(){
            { "youtube", null },
            { "streamlabs", null }
        };
        static Dictionary<string, string> js = new Dictionary<string, string>(){
            { "youtube", null },
            { "streamlabs", null }
        };
        static Thread wait_15sec;
        static UA_RequestHandler rh = new UA_RequestHandler();
        static bool switching_user;


        public static Main mainFormRef { get; set; }

        public static void INI(Main mainRef, ChromiumWebBrowser youtube, ChromiumWebBrowser streamlabs)
        {
            css_js.INI();
            mainFormRef = mainRef;
            cef["youtube"] = youtube;
            cef["streamlabs"] = streamlabs;
            cef["youtube"].FrameLoadEnd += new System.EventHandler<CefSharp.FrameLoadEndEventArgs>(FrameLoadEnd);
            cef["streamlabs"].FrameLoadEnd += new System.EventHandler<CefSharp.FrameLoadEndEventArgs>(FrameLoadEnd);
            cef["youtube"].AddressChanged += new System.EventHandler<CefSharp.AddressChangedEventArgs>(OnPreLoad);
            cef["streamlabs"].AddressChanged += new System.EventHandler<CefSharp.AddressChangedEventArgs>(OnPreLoad);

            cef["streamlabs"].RequestHandler = rh;
            cef["youtube"].RequestHandler = rh;
            cef["youtube"].LifeSpanHandler = new CustomLifeSpanHandler();
            cef["streamlabs"].LifeSpanHandler = new CustomLifeSpanHandler();

            wait_15sec = new Thread(TimeoutThread);
            wait_15sec.Name = "Login Timout";
            wait_15sec.IsBackground = true;
            wait_15sec.Start();


            Safe.Invoke(() =>
            {
                mainFormRef.ToggleLoading(2);
                mainFormRef.loadingText.Text = "יוצר חיבור לגוגל..";
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

            using (CefSettings settings = new CefSettings
            {
                LogSeverity = LogSeverity.Disable,
                WindowlessRenderingEnabled = true,
                BrowserSubprocessPath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, Environment.Is64BitProcess ? "x64" : "x86", "CefSharp.BrowserSubprocess.exe"),
                BackgroundColor = Util.ColorToUInt(System.Drawing.Color.FromArgb(255, 56, 56, 56)),
                CachePath = path,
                UserDataPath = path + "userData/",
                Locale = "he",
                RemoteDebuggingPort = 6968,
                PersistSessionCookies = true,
                UserAgent = BrowserFlow.UA,
                
            })
            {
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
            }
            #endregion
        }

        static ToolTip t = new ToolTip();
        static void OnPreLoad(object sender, AddressChangedEventArgs e)
        {
            string address = e.Browser.FocusedFrame.Url;

            Safe.Invoke(() =>
            {
                t.AutoPopDelay = 60;
                t.SetToolTip(mainFormRef.loadingText, address);
                t.Active = true;
                t.AutomaticDelay = 0;
                t.InitialDelay = 0;
            });
            ChromiumWebBrowser browserRef = ((ChromiumWebBrowser)sender);

            if (address.Contains("/livestreaming") && address.Contains("/video/"))
            {
                Safe.Invoke(() => { mainFormRef.loadingText.Text = "מזהה לייב פעיל..."; });
                OnLiveStreamPage(browserRef);
                return;
            }
            //if the page is livestream studio BEFORE the live actully loads in
            if (address.Contains("livestreaming") && !address.Contains(Urls.youtube["minLoginURL"]))
            {
                OnRawLiveStreamPage();
                return;
            }
            //if current page is youtube AND it after a switch
            if (browserRef.Address == Urls.youtube["youtubeUrl"] && switching_user)
            {
                Safe.Invoke(() =>
                {
                    mainFormRef.ToggleLoading(2);
                    mainFormRef.DynamicLayOut();
                    mainFormRef.loadingText.Text = "מחליף משתמש..";

                });
                switching_user = false;
                browserRef.Load(Urls.youtube["dashboard"]);
            }
        }

        static void FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            ChromiumWebBrowser browserRef = ((ChromiumWebBrowser)sender);
            string browserName = (browserRef.Equals(cef["youtube"])) ? "youtube" : "streamlabs";
            HandleDelayedJS(browserName, e.Frame);
            browserRef.SetZoomLevel(Properties.Settings.Default.scale);//set the zoom level from settings

            if (!e.Frame.IsMain || !e.Frame.IsValid)
                return;

            //if current page is google account login
            if (browserRef.Address.Contains(Urls.youtube["minLoginURL"]))
            {
                OnGoogleLoginPage();
                return;
            }
            //if current page is account switcher
            if (browserRef.Address.Contains(Urls.youtube["select_account"]) || browserRef.Address.Contains(Urls.youtube["select_account_path"]))
            {
                OnAccountSwitchPage();
                return;
            }
            //if aspect ration doesnt match settings aspect
            if (mainFormRef.tempRatio != double.Parse(Properties.Settings.Default["heightRatio"].ToString(), new CultureInfo("en-US")))
            {
                Properties.Settings.Default.heightRatio = mainFormRef.tempRatio;
                Properties.Settings.Default.Save();
                Safe.Invoke(() => mainFormRef.DynamicLayOut());
            }

            //if current page is some youtube page AND it was load after a RE-login action
            if (browserRef.Address == Urls.youtube["youtubeUrl"] && mainFormRef.yt_relogin)
            {
                OnYoutubeReLoginPage(browserRef);
                return;
            }
            //if the page is livestream studio
            if (browserRef.Address.Contains("/livestreaming") && browserRef.Address.Contains("/video/"))
            {
                //OnLiveStreamPage(browserRef);
                return;
            }
            if (browserRef.Address.Contains(Urls.youtube["chat"]) && !browserRef.Address.Contains("LoginError"))
            {
                OnChatPage(browserRef);
                return;
            }
            if (browserRef.Address.Contains(Urls.youtube["chat"]) && browserRef.Address.Contains("LoginError"))
            {
                Safe.Invoke(() => { mainFormRef.ToggleLoading(1); });
                InjectCSS("loginError", browserRef);
                return;
            }
        }


        #region js/css injection
        public static void InjectJS(string script, ChromiumWebBrowser browser, bool onlyNow = false)
        {
            if (script != null && !script.Contains(" ") && css_js.js.ContainsKey(script))
                script = css_js.js[script];

            if (browser != null && browser.CanExecuteJavascriptInMainFrame)
            {
                browser.ExecuteScriptAsyncWhenPageLoaded(script);
            }
            else if (onlyNow == false)
            {
                string browserName = (browser != null && browser.Equals(cef["youtube"])) ? "youtube" : "streamlabs";
                js[browserName] = script;
            }
        }
        public static void HandleDelayedJS(string browserName, IFrame frame)
        {
            string browsers_script = js[browserName];
            if (!string.IsNullOrEmpty(browsers_script))
                if (frame !=null && frame.IsMain)
                {
                    frame.ExecuteJavaScriptAsync(browsers_script);
                    js[browserName] = "";
                }
        }
        public static void InjectCSS(string css, ChromiumWebBrowser browser)
        {
            if (css == null || browser == null)
                return;
            if (!css.Contains(" ") && css_js.css.ContainsKey(css))
                css = css_js.css[css];

            string css_as_script = @"var wrap = document.createElement('style');
            wrap.innerHTML = '" + css.Replace(Environment.NewLine, "") + @"';
            document.getElementsByTagName('head')[0].appendChild(wrap)";
            InjectJS(css_as_script, browser);
        }
        #endregion

        #region per page events
        static void OnGoogleLoginPage()
        {
            if (wait_15sec.IsAlive)
            {
                wait_15sec.Abort();
            }
            mainFormRef.tempRatio = double.Parse(Properties.Settings.Default["heightRatio"].ToString(), new CultureInfo("en-US"));
            if (mainFormRef.tempRatio == 0)
                mainFormRef.tempRatio = 0.7;
            Properties.Settings.Default.heightRatio = 0;
            Properties.Settings.Default.Save();
            if (!mainFormRef.yt_relogin)
            {
                Safe.Invoke(() =>
                {
                    mainFormRef.ToggleLoading(1);
                    mainFormRef.DynamicLayOut();
                });
            }
        }
        static void OnYoutubeReLoginPage(ChromiumWebBrowser browser)
        {
            InjectJS("reLoginWait", browser);
            mainFormRef.yt_relogin = false; //reset the re login blocking state
        }
        static void OnRawLiveStreamPage()
        {
            if (!wait_15sec.IsAlive)
            {
                wait_15sec = new Thread(TimeoutThread);
                wait_15sec.Name = "Login Timout";
                wait_15sec.IsBackground = true;
            }


            Safe.Invoke(() =>
            {
                mainFormRef.ToggleLoading(2);
                mainFormRef.loadingText.Text = "טוען דשבורד...";
            });


            InjectJS(css_js.js["invalidUser"].Replace("{url}", Urls.youtube["select_account"]), cef["streamlabs"]);
        }
        static void OnAccountSwitchPage()
        {
            if (wait_15sec.IsAlive)
            {
                wait_15sec.Abort();
            }

            switching_user = true;
            InjectCSS(css_js.css["switch_user"], cef["streamlabs"]);
            mainFormRef.tempRatio = double.Parse(Properties.Settings.Default["heightRatio"].ToString(), new CultureInfo("en-US"));
            if (mainFormRef.tempRatio == 0)
                mainFormRef.tempRatio = 0.7;
            Properties.Settings.Default.heightRatio = 0;
            Properties.Settings.Default.Save();
            Safe.Invoke(() =>
            {
                mainFormRef.ToggleLoading(1);
                mainFormRef.DynamicLayOut();
            });
        }
        static void OnLiveStreamPage(ChromiumWebBrowser browser)
        {
            if (wait_15sec.IsAlive)
            {
                wait_15sec.Abort();
            }

            mainFormRef.SubCount.ReloadUrl();
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
                mainFormRef.chatID = "";
            }
            else
                mainFormRef.chatID = liveID;

            Thread.Sleep(500);
            Safe.Invoke(() =>
            {
                mainFormRef.loadingText.Text = "טוען צאט...";
                cef["streamlabs"].Load(Urls.streamlabs["events"]);
                cef["youtube"].Load(Urls.youtube["chat"] + liveID);
            });


        }
        static void OnChatPage(ChromiumWebBrowser browser)
        {
            Safe.Invoke(() => { mainFormRef.ToggleLoading(1); });
            InjectCSS("chat", browser);
        }
        #endregion

        static void TimeoutThread()
        {
            Thread.Sleep(20 * 1000);
            cef["youtube"].Load(Urls.youtube["chat"] + "LoginError");
            cef["streamlabs"].Load(Urls.streamlabs["events"]);
            MessageBox.Show("נראה שלוקח יותר מידי זמן להתחבר,\nמעביר אותך לדף הבית של סטרימר פלוס - יתכן שתצטרך להתחבר למשתמש שלך בהגדרות.", "תקלה בהתחברות");

        }
    }

    public static class FastBrowser
    {
        public static void ActionAt(string urlz, string js)
        {
            using (BrowserSettings offscreen_setting = new BrowserSettings())
            {
                offscreen_setting.DefaultEncoding = "UTF-8";
                offscreen_setting.WindowlessFrameRate = 5;
                using (CefSharp.OffScreen.ChromiumWebBrowser tempBrowser = new CefSharp.OffScreen.ChromiumWebBrowser(urlz, offscreen_setting))
                {
                    tempBrowser.RequestHandler = new UA_RequestHandler();
                    tempBrowser.LifeSpanHandler = new CustomLifeSpanHandler();
                    tempBrowser.Size = new Size(10, 10);
                    tempBrowser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>((Object sender, FrameLoadEndEventArgs e) =>
                    {
                        if (!e.Frame.IsValid)
                            return;

                        if (e.Url == Urls.olympicangel["event_end"])
                        {
                            Safe.Invoke(() =>
                            {
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
                }
            }
        }
    }
}
