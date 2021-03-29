using CefSharp;
using CefSharp.WinForms;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Web;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;

namespace StreamerPlusApp
{
    public partial class Main : Form
    {
        public string chatID = "";
        public bool yt_relogin = false;
        int blurMapRenderCount = 0;
        public double tempRatio = 0.7;
        Image blurImage;
        bool isSettingMod = false;
        public subCount subCount;
        
        private readonly int constSpacing = 8;
        private const int cCaption = 20;   // Caption bar height;

        public Main() : base()
        {
            Util.CheckForUpdates(this);//check for updates from server (current version defined at Util)
            Util.Argreement(this);//if user didnt agreed to last agreement prompt him to
            BrowserFlow.CefSettings();

            subCount = new subCount();

            InitializeComponent();
            this.LoadingPanel.Visible = true;
            this.LoadingPanel.Dock = DockStyle.Fill;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.ResizeRedraw = true;
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            this.Location = new Point(Math.Max(0, Math.Min(resolution.Width - this.Width, Properties.Settings.Default.posX))
                                 , Math.Max(0, Math.Min(resolution.Height - this.Height, Properties.Settings.Default.posY)));
            double zoom = double.Parse(Properties.Settings.Default["scale"].ToString());
            double pos = (zoom * (-1)) * 2;
            this.trackBar1.Value = (int)pos;
            DynamicLayOut();

            this.Text = "סטרימר פלוס - " + string.Format("{0:N1}.0", Util.version);

            BrowserFlow.INI(this, this.ChatBrowser, this.StreamlabsBrowser);

        }

        #region resize
        private void Main_ResizeEnd(Object sender, EventArgs e)
        {
            //f.Show();
            if (isSettingMod)
                this.tabControl1.Show();

        }
        private void Main_Resize(object sender, EventArgs e)
        {
            if (this.tabControl1.Visible)
                this.tabControl1.Hide();
            this.Invalidate();
            DynamicLayOut();
            this.Invalidate();
        }
        #endregion

        #region draw Logic

        public void ToggleLoading(int toState = 0)
        {
            //toState = 2 - to show
            //toState = 1 - to hide
            //toState = 0 - auto
            bool isShown = this.LoadingPanel.Visible;
            if(toState == 2 || (!isShown && toState == 0))
            {
                //show
                this.LoadingPanel.Visible = true;
                this.LoadingPanel.BringToFront();
            }
            if (toState == 1 || (isShown && toState == 0))
            {
                //hide
                this.LoadingPanel.Visible = false;
                this.LoadingPanel.SendToBack();
            }
            DynamicLayOut();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle rc;
            if (!isSettingMod)
            {
                rc = new Rectangle(this.ChatBrowser.Location.X - 1, this.ChatBrowser.Location.Y - 1, this.ChatBrowser.Width + 2, this.ChatBrowser.Height + 2);
                e.Graphics.FillRectangle(Brushes.Red, rc);//draws req around chat
                rc = new Rectangle(this.StreamlabsBrowser.Location.X - 1, this.StreamlabsBrowser.Location.Y - 1, this.StreamlabsBrowser.Width + 2, this.StreamlabsBrowser.Height + 2);
                e.Graphics.FillRectangle(Brushes.Red, rc);//draes req around events
            }
            else
            {
                RectangleF destinationRect = new RectangleF(0, 0, this.Width, this.Height);
                e.Graphics.DrawImage(blurImage, destinationRect);//draws the blur images of the chat in Settings

            }

            rc = new Rectangle(0, -10, this.ClientSize.Width, cCaption);
            e.Graphics.FillRectangle(Brushes.Red, rc);//draw red top bar

        }

        public void ShowSettingsScreen()
        {
            this.Invalidate();
            blurImage = Util.PrintScreen(this);


            isSettingMod = true;

            this.ChatBrowser.Hide();
            this.StreamlabsBrowser.Hide();
            this.devider.Hide();

            this.fastTitle.Show();
            this.OpenLiveButton.Show();
            this.streamlabsButton.Show();
            this.loginYoutube.Show();
            this.streamlabsLogout.Show();
            this.ReloadBrowsers.Show();
            this.tabControl1.Show();
            this.button2.Show();

            this.backButton.Show();
            this.backButton.BringToFront();

            DynamicLayOut();
        }
        public void HideSettingsScreen()
        {
            this.Invalidate();

            blurImage = null;
            isSettingMod = false;

            this.ChatBrowser.Show();
            this.StreamlabsBrowser.Show();
            this.devider.Show();

            this.fastTitle.Hide();
            this.OpenLiveButton.Hide();
            this.streamlabsButton.Hide();
            this.loginYoutube.Hide();
            this.streamlabsLogout.Hide();
            this.ReloadBrowsers.Hide();
            this.tabControl1.Hide();
            this.button2.Hide();

            this.backButton.Hide();
            this.backButton.BringToFront();
        }
        bool isDragging;
        private void Devider_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                int inBoxPosY = Cursor.Position.Y - this.Location.Y + (this.ClientRectangle.Height - this.Height);
                double newRatio = ((double)inBoxPosY - cCaption) / ((double)this.ClientRectangle.Height - cCaption - constSpacing * 2);
                newRatio = Math.Min(newRatio, 0.85);
                newRatio = Math.Max(newRatio, 0.35);
                Properties.Settings.Default["heightRatio"] = newRatio;
                this.Invalidate();
                DynamicLayOut();
            }
        }
        private void Devider_MouseUp(object sender, MouseEventArgs e)
        {
            this.Invalidate();
            isDragging = false;
            Properties.Settings.Default.Save();
        }
        private void Devider_MouseDown(object sender, MouseEventArgs e)
        {
            if (Properties.Settings.Default.heightRatio != 0 || Properties.Settings.Default.heightRatio != 0.0)
                isDragging = true;
        }
        public void DynamicLayOut()
        {
            int relativeWidth = this.ClientRectangle.Width, relativeHeight = this.ClientRectangle.Height;
            if (Properties.Settings.Default.heightRatio == 0 && this.tempRatio == 0)
            {
                Properties.Settings.Default.heightRatio = 0.7;
                Properties.Settings.Default.Save();
            }

            double constRatio = Properties.Settings.Default.heightRatio;
            this.ChatBrowser.Margin = new Padding(constSpacing);
            this.ChatBrowser.Location = new Point(constSpacing, constSpacing + cCaption);
            this.ChatBrowser.Width = relativeWidth - constSpacing * 2;
            this.ChatBrowser.Height = (int)((relativeHeight - cCaption - constSpacing * 5) * constRatio);

            this.StreamlabsBrowser.Margin = new Padding(constSpacing);
            this.StreamlabsBrowser.Location = new Point(constSpacing, (int)((relativeHeight - cCaption - constSpacing * 5) * constRatio + constSpacing * 4 + cCaption));
            this.StreamlabsBrowser.Width = relativeWidth - constSpacing * 2;
            this.StreamlabsBrowser.Height = (int)((relativeHeight - cCaption - constSpacing * 5) * (1 - constRatio));

            this.devider.Location = new Point(constSpacing, (int)((relativeHeight - cCaption - constSpacing * 5) * constRatio) + constSpacing + cCaption + 3);
            this.devider.Width = relativeWidth - constSpacing * 2;
            this.devider.Height = 15;


            this.SettingsImage.Location = new Point(relativeWidth / 2 - this.SettingsImage.Width / 2, this.SettingsImage.Location.Y);

            this.backButton.Location = new Point(relativeWidth / 2 - this.backButton.Width / 2, this.backButton.Location.Y);

            this.fastTitle.Location = new Point((int)constRatio, cCaption + 2 + relativeHeight / 50);
            this.fastTitle.Width = relativeWidth;
            this.fastTitle.Height = Math.Max(Math.Min(60, Height / 10), 30);

            int maxButtonSize = Math.Min((relativeWidth / 3) - constSpacing * 4, this.fastTitle.Height * 4);

            this.OpenLiveButton.Location = new Point(constSpacing * 4, this.fastTitle.Location.Y + this.fastTitle.Height + constSpacing);
            this.OpenLiveButton.Width = this.OpenLiveButton.Height = maxButtonSize;

            this.streamlabsButton.Location = new Point(relativeWidth - constSpacing * 4 - maxButtonSize, this.fastTitle.Location.Y + this.fastTitle.Height + constSpacing);
            this.streamlabsButton.Width = this.streamlabsButton.Height = this.button2.Width = this.button2.Height = maxButtonSize;

            this.button2.Location = new Point(relativeWidth / 2 - maxButtonSize / 2, this.OpenLiveButton.Location.Y);


            int flatButtonX = Math.Min(constSpacing * 4, maxButtonSize + constSpacing);

            int flatButtonY = streamlabsButton.Location.Y + streamlabsButton.Height + constSpacing * 2;


            this.loginYoutube.Location = new Point(flatButtonX, flatButtonY);
            this.loginYoutube.Width = relativeWidth - flatButtonX * 2;
            this.loginYoutube.Height = Math.Max(30, (int)(0.4 * (relativeHeight - this.streamlabsButton.Location.Y - this.streamlabsButton.Height - constSpacing * 8) / 3));

            this.loginYoutube.Location = new Point(flatButtonX, flatButtonY);
            this.loginYoutube.Width = relativeWidth - flatButtonX * 2;
            this.loginYoutube.Height = Math.Max(30, (int)(0.4 * (relativeHeight - this.streamlabsButton.Location.Y - this.streamlabsButton.Height - constSpacing * 8) / 3));


            this.streamlabsLogout.Location = new Point(flatButtonX, flatButtonY + this.loginYoutube.Height + constSpacing * 2);
            this.streamlabsLogout.Width = relativeWidth - flatButtonX * 2;
            this.streamlabsLogout.Height = Math.Max(30, (int)(0.4 * (relativeHeight - this.streamlabsButton.Location.Y - this.streamlabsButton.Height - constSpacing * 8) / 3));

            this.ReloadBrowsers.Location = new Point(flatButtonX, flatButtonY + this.streamlabsLogout.Height * 2 + constSpacing * 4);
            this.ReloadBrowsers.Width = relativeWidth - flatButtonX * 2;
            this.ReloadBrowsers.Height = Math.Max(30, (int)(0.4 * (relativeHeight - this.streamlabsButton.Location.Y - this.streamlabsButton.Height - constSpacing * 8) / 3));

            this.tabControl1.Location = new Point(flatButtonX, this.ReloadBrowsers.Location.Y + this.ReloadBrowsers.Height + constSpacing * 2);
            this.tabControl1.Width = this.ReloadBrowsers.Width;
            this.tabControl1.Height = (int)(0.6 * (relativeHeight - this.streamlabsButton.Location.Y - this.streamlabsButton.Height - constSpacing * 8) - constSpacing * 2);

            this.manulChat.Width = this.tabControl1.Width - this.button1.Location.X - this.button1.Width - constSpacing * 3;
            this.manulChat.Location = new Point(this.button1.Location.X + this.button1.Width + constSpacing, this.manulChat.Location.Y);

            this.trackBar1.Width = this.tabControl1.Width - constSpacing * 3;
            this.trackBar1.Location = new Point(constSpacing, this.manulChat.Location.Y + this.manulChat.Height + 5 * constSpacing);

            this.label2.Location = new Point(this.tabControl1.Width - this.label2.Width - constSpacing * 2, this.manulChat.Location.Y + this.manulChat.Height + 3 * constSpacing);

            this.label3.Location = new Point(this.tabControl1.Width - this.label3.Width - constSpacing, this.manulChat.Location.Y + this.manulChat.Height + this.trackBar1.Height + 4 * constSpacing);
            this.label4.Location = new Point(constSpacing, this.manulChat.Location.Y + this.manulChat.Height + this.trackBar1.Height + 4 * constSpacing);

            ChatBrowser.Invalidate();
            StreamlabsBrowser.Invalidate();

            //this.ChatBrowser.Update();
            // this.StreamlabsBrowser.Update();
        }
        #endregion

        #region button click and trigger
        private void SettingImage_Click(object sender, EventArgs e)
        {
            Point relativeMouse = this.SettingsImage.PointToClient(Cursor.Position);
            Rectangle correctHitbox = new Rectangle(8,
                                                    0,
                                                    this.SettingsImage.Width - 16,
                                                    this.SettingsImage.Height - 8);
            if (correctHitbox.Contains(relativeMouse))
                ShowSettingsScreen();
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            Point relativeMouse = this.backButton.PointToClient(Cursor.Position);
            Rectangle correctHitbox = new Rectangle(8,
                                                    0,
                                                    this.backButton.Width - 16,
                                                    this.backButton.Height - 8);
            if (correctHitbox.Contains(relativeMouse))
                HideSettingsScreen();
        }
        private void OpenLiveButton_Click(object sender, EventArgs e)
        {

            string injectScript = @"window.open('https://www.youtube.com/live_dashboard?nv=0', '_blank', 'location=false,height=600,width=700,scrollbars=yes,status=yes');";
            if (ChatBrowser.CanExecuteJavascriptInMainFrame)
                this.ChatBrowser.ExecuteScriptAsyncWhenPageLoaded(injectScript);

        }
        private void StreamlabsButton_Click(object sender, EventArgs e)
        {
            string injectScript = @"window.open('https://streamlabs.com/dashboard#/', '_blank', 'location=false,height=600,width=700,scrollbars=yes,status=yes');";
            if (ChatBrowser.CanExecuteJavascriptInMainFrame)
                this.ChatBrowser.ExecuteScriptAsyncWhenPageLoaded(injectScript);
        }
        private void LoginYoutube_Click(object sender, EventArgs e)
        {
            yt_relogin = true;
            this.ToggleLoading(2);
            this.loadingText.Text = "מחכה לסיגרת\nחלון ההתחברות..\n(לאחר ההתחברות)";
            this.StreamlabsBrowser.Load("https://www.youtube.com/logout");
        }
        private void StreamlabsLogout_Click(object sender, EventArgs e)
        {
            string injectScript = @"window.open('https://streamlabs.com/login?skip_splash=1&r=/dashboard&youtube=1&landing=1&force_login=1', '_blank', 'location=false,height=600,width=700,scrollbars=yes,status=yes');";
            if (ChatBrowser.CanExecuteJavascriptInMainFrame)
                this.ChatBrowser.ExecuteScriptAsyncWhenPageLoaded(injectScript);

        }
        private void ReloadBrowsers_Click(object sender, EventArgs e)
        {
            //LoadingUpdate(1);
            this.ChatBrowser.Reload();
            this.StreamlabsBrowser.Load(" https://streamlabs.com/dashboard/recent-events");
        }
        private void PictureBox1_Click_1(object sender, EventArgs e)
        {
            this.StreamlabsBrowser.Load("https://www.youtube.com/live_dashboard?nv=0");
            MessageBox.Show("התוכנה כעט מנסה לאתר את הצאט של הערוץ שלך, במידה ולא תצליח אנה הזן את הצאט שלך בצורה ידנית בהגדרות", "איתור צאט");
        }
        private void Button1_Click(object sender, EventArgs e)
        {

            string inputText = this.manulChat.Text;
            if (inputText == null || inputText == "" || inputText == " " || inputText.Contains(" "))
            {
                MessageBox.Show("אנה הזן אחד מהבאים:\n-קישור רגיל של הלייב\n-קישור מקוצר של הלייב.\n-מזהה (ID) של הלייב.", "קלט לא תקין", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading);
                return;
            }

            if (inputText.Contains("youtube.com/watch?v=")){
                Uri myUri = new Uri(inputText);
                inputText = HttpUtility.ParseQueryString(myUri.Query).Get("v");
            }
            else if (inputText.Contains("youtu.be/")){
                inputText = inputText.Split('/')[1];
            }
            else if (inputText.Length < 14 && inputText.Length > 4) {
                //legit the id
                chatID = inputText;
            }
            else{
                MessageBox.Show("אנה הזן אחד מהבאים:\n-קישור רגיל של הלייב\n-קישור מקוצר של הלייב.\n-מזהה (ID) של הלייב.", "קלט לא תקין", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading);
                return;
            }

            if (this.ChatBrowser.Address != Urls.youtube["chat"] + inputText){
                this.ChatBrowser.Load(Urls.youtube["chat"] + inputText);
                MessageBox.Show("חזור לחלון הצאט בכדי לוודאות שהוא נטען כמו שצריך", "פעולה בוצעה", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading);
            }
            else
                MessageBox.Show("לייב זה כבר טעון", "פעולה בוטלה", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading);


        }
        private void TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.Button1_Click(null, null);
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            string injectScript = @"window.open('https://www.olympicangelabz.com/pages/stream-settings/full.php', '_blank', 'location=false,height=400,width=950,scrollbars=yes,status=yes');";
            if (ChatBrowser.CanExecuteJavascriptInMainFrame)
                this.ChatBrowser.ExecuteScriptAsyncWhenPageLoaded(injectScript);
        }
        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            return;
        }
        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            double zoom = 0.0 - (this.trackBar1.Value * 50) / 100.0;
            Properties.Settings.Default["scale"] = zoom;

            if (this.ChatBrowser.IsBrowserInitialized)
                this.ChatBrowser.Reload();
            if (this.StreamlabsBrowser.IsBrowserInitialized)
                this.StreamlabsBrowser.Load(" https://streamlabs.com/dashboard/recent-events");
        }
        private void PictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/OlympicAngel");
        }
        #endregion

        #region close
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cef.Shutdown();
            this.subCount.localServer.Close();
            Properties.Settings.Default["posX"] = this.Location.X;
            Properties.Settings.Default["posY"] = this.Location.Y;
            Properties.Settings.Default["width"] = this.Width;
            Properties.Settings.Default["height"] = this.Height;
            Properties.Settings.Default.Save();
        }
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
            this.subCount.localServer.Close();
            Properties.Settings.Default["posX"] = this.Location.X;
            Properties.Settings.Default["posY"] = this.Location.Y;
            Properties.Settings.Default["width"] = this.Width;
            Properties.Settings.Default["height"] = this.Height;
            Properties.Settings.Default.Save();

            this.Hide();
        }

        protected override void OnClosed(EventArgs e)
        {
            Cef.Shutdown();
            this.subCount.localServer.Close();
            base.OnClosed(e);
        }
        #endregion

        private void InsertAdBtn_Click(object sender, EventArgs e)
        {
            FastBrowser.ActionAt(Urls.youtube["livestream_placeholder"].Replace("{id}", chatID), "InsertAd");
        }

        private void InsertMarkerBtn_Click(object sender, EventArgs e)
        {
            
               FastBrowser.ActionAt(Urls.youtube["livestream_placeholder"].Replace("{id}", chatID), "InsertMarker");

        }
    }



    #region ETC-Class
    
    #endregion
}
