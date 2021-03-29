using System;
using System.Drawing;
using System.Windows.Forms;

namespace StreamerPlusApp
{
    partial class Main
    {
        int width;
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.ChatBrowser = new CefSharp.WinForms.ChromiumWebBrowser();
            this.StreamlabsBrowser = new CefSharp.WinForms.ChromiumWebBrowser();
            this.loginYoutube = new System.Windows.Forms.Button();
            this.streamlabsLogout = new System.Windows.Forms.Button();
            this.ReloadBrowsers = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.manulChat = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.LoadingPanel = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.loadingText = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.streamlabsButton = new System.Windows.Forms.Button();
            this.OpenLiveButton = new System.Windows.Forms.Button();
            this.fastTitle = new System.Windows.Forms.PictureBox();
            this.backButton = new System.Windows.Forms.PictureBox();
            this.SettingsImage = new System.Windows.Forms.PictureBox();
            this.devider = new System.Windows.Forms.PictureBox();
            this.InsertAdBtn = new System.Windows.Forms.PictureBox();
            this.InsertMarkerBtn = new System.Windows.Forms.PictureBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.LoadingPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SettingsImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.devider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsertAdBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsertMarkerBtn)).BeginInit();
            this.SuspendLayout();
            // 
            // ChatBrowser
            // 
            this.ChatBrowser.AccessibleName = "youtube chat";
            this.ChatBrowser.ActivateBrowserOnCreation = true;
            this.ChatBrowser.Location = new System.Drawing.Point(15, 28);
            this.ChatBrowser.Margin = new System.Windows.Forms.Padding(0);
            this.ChatBrowser.Name = "ChatBrowser";
            this.ChatBrowser.Size = new System.Drawing.Size(276, 23);
            this.ChatBrowser.TabIndex = 0;
            // 
            // StreamlabsBrowser
            // 
            this.StreamlabsBrowser.AccessibleName = "streamlabs donations";
            this.StreamlabsBrowser.ActivateBrowserOnCreation = true;
            this.StreamlabsBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StreamlabsBrowser.Location = new System.Drawing.Point(15, 516);
            this.StreamlabsBrowser.Margin = new System.Windows.Forms.Padding(0);
            this.StreamlabsBrowser.Name = "StreamlabsBrowser";
            this.StreamlabsBrowser.Size = new System.Drawing.Size(276, 23);
            this.StreamlabsBrowser.TabIndex = 1;
            // 
            // loginYoutube
            // 
            this.loginYoutube.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.loginYoutube.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.loginYoutube.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.loginYoutube.FlatAppearance.BorderSize = 2;
            this.loginYoutube.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkRed;
            this.loginYoutube.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(86)))), ((int)(((byte)(86)))));
            this.loginYoutube.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginYoutube.Font = new System.Drawing.Font("Heebo", 10F);
            this.loginYoutube.ForeColor = System.Drawing.Color.White;
            this.loginYoutube.Location = new System.Drawing.Point(50, 120);
            this.loginYoutube.Name = "loginYoutube";
            this.loginYoutube.Size = new System.Drawing.Size(211, 31);
            this.loginYoutube.TabIndex = 10;
            this.loginYoutube.Text = "התחבר מחדש ליוטיוב";
            this.loginYoutube.UseVisualStyleBackColor = false;
            this.loginYoutube.Visible = false;
            this.loginYoutube.Click += new System.EventHandler(this.LoginYoutube_Click);
            // 
            // streamlabsLogout
            // 
            this.streamlabsLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.streamlabsLogout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.streamlabsLogout.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.streamlabsLogout.FlatAppearance.BorderSize = 2;
            this.streamlabsLogout.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkRed;
            this.streamlabsLogout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(86)))), ((int)(((byte)(86)))));
            this.streamlabsLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.streamlabsLogout.Font = new System.Drawing.Font("Heebo", 10F);
            this.streamlabsLogout.ForeColor = System.Drawing.Color.White;
            this.streamlabsLogout.Location = new System.Drawing.Point(50, 157);
            this.streamlabsLogout.Name = "streamlabsLogout";
            this.streamlabsLogout.Size = new System.Drawing.Size(211, 33);
            this.streamlabsLogout.TabIndex = 11;
            this.streamlabsLogout.Text = "התחבר מחדש לסטריםלאבס";
            this.streamlabsLogout.UseVisualStyleBackColor = false;
            this.streamlabsLogout.Visible = false;
            this.streamlabsLogout.Click += new System.EventHandler(this.StreamlabsLogout_Click);
            // 
            // ReloadBrowsers
            // 
            this.ReloadBrowsers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.ReloadBrowsers.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ReloadBrowsers.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.ReloadBrowsers.FlatAppearance.BorderSize = 2;
            this.ReloadBrowsers.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkRed;
            this.ReloadBrowsers.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(86)))), ((int)(((byte)(86)))));
            this.ReloadBrowsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ReloadBrowsers.Font = new System.Drawing.Font("Heebo", 10F);
            this.ReloadBrowsers.ForeColor = System.Drawing.Color.White;
            this.ReloadBrowsers.Location = new System.Drawing.Point(50, 196);
            this.ReloadBrowsers.Name = "ReloadBrowsers";
            this.ReloadBrowsers.Size = new System.Drawing.Size(211, 33);
            this.ReloadBrowsers.TabIndex = 12;
            this.ReloadBrowsers.Text = "רענן צאט ותרומות";
            this.ReloadBrowsers.UseVisualStyleBackColor = false;
            this.ReloadBrowsers.Visible = false;
            this.ReloadBrowsers.Click += new System.EventHandler(this.ReloadBrowsers_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 232);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeftLayout = true;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(279, 281);
            this.tabControl1.TabIndex = 14;
            this.tabControl1.Visible = false;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.trackBar1);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.manulChat);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(271, 251);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "הגדרות";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(6, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "25%";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(234, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "100%";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(169, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "קנה מידה של תצוגה";
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.trackBar1.LargeChange = 2;
            this.trackBar1.Location = new System.Drawing.Point(8, 126);
            this.trackBar1.Maximum = 5;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.trackBar1.Size = new System.Drawing.Size(257, 45);
            this.trackBar1.TabIndex = 3;
            this.trackBar1.Value = 2;
            this.trackBar1.ValueChanged += new System.EventHandler(this.TrackBar1_ValueChanged);
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkRed;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Tomato;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(6, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(43, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "עדכן";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(143, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "צאט ידני - הזן כתובת לייב";
            // 
            // manulChat
            // 
            this.manulChat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.manulChat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.manulChat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.manulChat.Location = new System.Drawing.Point(43, 23);
            this.manulChat.Name = "manulChat";
            this.manulChat.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.manulChat.Size = new System.Drawing.Size(222, 24);
            this.manulChat.TabIndex = 0;
            this.manulChat.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox1_KeyUp);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pictureBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(271, 251);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "אודות";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.pictureBox2.BackgroundImage = global::StreamerPlusApp.Properties.Resources.sticker;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(265, 245);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBox2_MouseClick);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(271, 251);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "רשומים בזמן אמת";
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(3, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(265, 245);
            this.label5.TabIndex = 5;
            this.label5.Text = resources.GetString("label5.Text");
            // 
            // LoadingPanel
            // 
            this.LoadingPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.LoadingPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(87)))), ((int)(((byte)(69)))));
            this.LoadingPanel.Controls.Add(this.loadingText);
            this.LoadingPanel.Controls.Add(this.pictureBox3);
            this.LoadingPanel.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.LoadingPanel.Location = new System.Drawing.Point(0, 0);
            this.LoadingPanel.Name = "LoadingPanel";
            this.LoadingPanel.Size = new System.Drawing.Size(200, 100);
            this.LoadingPanel.TabIndex = 16;
            this.LoadingPanel.Visible = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox3.BackgroundImage = global::StreamerPlusApp.Properties.Resources.loaderLogo2;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox3.InitialImage = global::StreamerPlusApp.Properties.Resources.loaderLogo2;
            this.pictureBox3.Location = new System.Drawing.Point(0, 0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(197, 100);
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            // 
            // loadingText
            // 
            this.loadingText.AutoEllipsis = true;
            this.loadingText.BackColor = System.Drawing.Color.Transparent;
            this.loadingText.Dock = System.Windows.Forms.DockStyle.Top;
            this.loadingText.Font = new System.Drawing.Font("Heebo", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadingText.ForeColor = System.Drawing.Color.White;
            this.loadingText.Location = new System.Drawing.Point(0, 0);
            this.loadingText.Name = "loadingText";
            this.loadingText.Size = new System.Drawing.Size(200, 75);
            this.loadingText.TabIndex = 1;
            this.loadingText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.loadingText.UseCompatibleTextRendering = true;
            this.loadingText.BackColor = Color.DarkRed;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.BackgroundImage = global::StreamerPlusApp.Properties.Resources.streamsettings;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Location = new System.Drawing.Point(119, 91);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 15;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::StreamerPlusApp.Properties.Resources.reload;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Location = new System.Drawing.Point(5, -5);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(28, 28);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.PictureBox1_Click_1);
            // 
            // streamlabsButton
            // 
            this.streamlabsButton.BackColor = System.Drawing.Color.Transparent;
            this.streamlabsButton.BackgroundImage = global::StreamerPlusApp.Properties.Resources.streamlabsButton;
            this.streamlabsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.streamlabsButton.Location = new System.Drawing.Point(186, 91);
            this.streamlabsButton.Name = "streamlabsButton";
            this.streamlabsButton.Size = new System.Drawing.Size(75, 23);
            this.streamlabsButton.TabIndex = 9;
            this.streamlabsButton.UseVisualStyleBackColor = false;
            this.streamlabsButton.Visible = false;
            this.streamlabsButton.Click += new System.EventHandler(this.StreamlabsButton_Click);
            // 
            // OpenLiveButton
            // 
            this.OpenLiveButton.BackColor = System.Drawing.Color.Transparent;
            this.OpenLiveButton.BackgroundImage = global::StreamerPlusApp.Properties.Resources.LiveButton;
            this.OpenLiveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.OpenLiveButton.Location = new System.Drawing.Point(50, 91);
            this.OpenLiveButton.Name = "OpenLiveButton";
            this.OpenLiveButton.Size = new System.Drawing.Size(75, 23);
            this.OpenLiveButton.TabIndex = 8;
            this.OpenLiveButton.UseVisualStyleBackColor = false;
            this.OpenLiveButton.Visible = false;
            this.OpenLiveButton.Click += new System.EventHandler(this.OpenLiveButton_Click);
            // 
            // fastTitle
            // 
            this.fastTitle.BackColor = System.Drawing.Color.Transparent;
            this.fastTitle.BackgroundImage = global::StreamerPlusApp.Properties.Resources.fastText;
            this.fastTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.fastTitle.Location = new System.Drawing.Point(12, 169);
            this.fastTitle.Margin = new System.Windows.Forms.Padding(0);
            this.fastTitle.Name = "fastTitle";
            this.fastTitle.Size = new System.Drawing.Size(276, 60);
            this.fastTitle.TabIndex = 7;
            this.fastTitle.TabStop = false;
            this.fastTitle.Visible = false;
            // 
            // backButton
            // 
            this.backButton.BackColor = System.Drawing.Color.Transparent;
            this.backButton.BackgroundImage = global::StreamerPlusApp.Properties.Resources.back;
            this.backButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.backButton.Location = new System.Drawing.Point(94, -10);
            this.backButton.Margin = new System.Windows.Forms.Padding(0);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(100, 38);
            this.backButton.TabIndex = 5;
            this.backButton.TabStop = false;
            this.backButton.Visible = false;
            this.backButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // SettingsImage
            // 
            this.SettingsImage.BackColor = System.Drawing.Color.Transparent;
            this.SettingsImage.BackgroundImage = global::StreamerPlusApp.Properties.Resources.settings;
            this.SettingsImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.SettingsImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SettingsImage.Location = new System.Drawing.Point(94, -10);
            this.SettingsImage.Margin = new System.Windows.Forms.Padding(0);
            this.SettingsImage.Name = "SettingsImage";
            this.SettingsImage.Size = new System.Drawing.Size(100, 38);
            this.SettingsImage.TabIndex = 3;
            this.SettingsImage.TabStop = false;
            this.SettingsImage.Click += new System.EventHandler(this.SettingImage_Click);
            // 
            // devider
            // 
            this.devider.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.devider.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("devider.BackgroundImage")));
            this.devider.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.devider.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.devider.Location = new System.Drawing.Point(15, 349);
            this.devider.Name = "devider";
            this.devider.Size = new System.Drawing.Size(246, 20);
            this.devider.TabIndex = 2;
            this.devider.TabStop = false;
            this.devider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Devider_MouseDown);
            this.devider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Devider_MouseMove);
            this.devider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Devider_MouseUp);
            // 
            // InsertAdBtn
            // 
            this.InsertAdBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InsertAdBtn.BackColor = System.Drawing.Color.Transparent;
            this.InsertAdBtn.BackgroundImage = global::StreamerPlusApp.Properties.Resources.insertAd;
            this.InsertAdBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.InsertAdBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.InsertAdBtn.Location = new System.Drawing.Point(224, -5);
            this.InsertAdBtn.Margin = new System.Windows.Forms.Padding(0);
            this.InsertAdBtn.Name = "InsertAdBtn";
            this.InsertAdBtn.Size = new System.Drawing.Size(28, 28);
            this.InsertAdBtn.TabIndex = 17;
            this.InsertAdBtn.TabStop = false;
            this.InsertAdBtn.Click += new System.EventHandler(this.InsertAdBtn_Click);
            // 
            // InsertMarkerBtn
            // 
            this.InsertMarkerBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InsertMarkerBtn.BackColor = System.Drawing.Color.Transparent;
            this.InsertMarkerBtn.BackgroundImage = global::StreamerPlusApp.Properties.Resources.insertMarker;
            this.InsertMarkerBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.InsertMarkerBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.InsertMarkerBtn.Location = new System.Drawing.Point(252, -5);
            this.InsertMarkerBtn.Margin = new System.Windows.Forms.Padding(0);
            this.InsertMarkerBtn.Name = "InsertMarkerBtn";
            this.InsertMarkerBtn.Size = new System.Drawing.Size(28, 28);
            this.InsertMarkerBtn.TabIndex = 18;
            this.InsertMarkerBtn.TabStop = false;
            this.InsertMarkerBtn.Click += new System.EventHandler(this.InsertMarkerBtn_Click);
            // 
            // Main
            // 
            this.AccessibleName = "סטרימר פלוס";
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(284, 411);
            this.Controls.Add(this.InsertMarkerBtn);
            this.Controls.Add(this.InsertAdBtn);
            this.Controls.Add(this.LoadingPanel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ReloadBrowsers);
            this.Controls.Add(this.streamlabsLogout);
            this.Controls.Add(this.loginYoutube);
            this.Controls.Add(this.streamlabsButton);
            this.Controls.Add(this.OpenLiveButton);
            this.Controls.Add(this.fastTitle);
            this.Controls.Add(this.SettingsImage);
            this.Controls.Add(this.StreamlabsBrowser);
            this.Controls.Add(this.ChatBrowser);
            this.Controls.Add(this.devider);
            this.Controls.Add(this.backButton);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Heebo", 8.25F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(15)))), ((int)(((byte)(255)))));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(9999, 9999);
            this.MinimumSize = new System.Drawing.Size(300, 450);
            this.Name = "Main";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "סטרימר פלוס - 0.25";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.ResizeEnd += new System.EventHandler(this.Main_ResizeEnd);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Main_KeyUp);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.LoadingPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.backButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SettingsImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.devider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsertAdBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InsertMarkerBtn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public CefSharp.WinForms.ChromiumWebBrowser ChatBrowser;
        public CefSharp.WinForms.ChromiumWebBrowser StreamlabsBrowser;
        private System.Windows.Forms.PictureBox devider;
        private System.Windows.Forms.PictureBox SettingsImage;
        private System.Windows.Forms.PictureBox backButton;
        private System.Windows.Forms.PictureBox fastTitle;
        private System.Windows.Forms.Button OpenLiveButton;
        private System.Windows.Forms.Button streamlabsButton;
        private System.Windows.Forms.Button loginYoutube;
        private System.Windows.Forms.Button streamlabsLogout;
        private System.Windows.Forms.Button ReloadBrowsers;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox manulChat;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel LoadingPanel;
        public System.Windows.Forms.Label loadingText;
        private System.Windows.Forms.TabPage tabPage3;
        public System.Windows.Forms.Label label5;
        private PictureBox InsertAdBtn;
        private PictureBox InsertMarkerBtn;
        private PictureBox pictureBox3;
    }
}

