using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamerPlusApp
{
    static class Util
    {
        public static double version = 1.4;
        private static int blurMapRenderCount;

        public static uint ColorToUInt(Color color)
        {
            return (uint)((color.A << 24) | (color.R << 16) | (color.G << 8) | (color.B << 0));
        }

        public static void Argreement(Form mainFormRef)
        {
            if (Properties.Settings.Default.Agreement != version)
            {
                DialogResult result = MessageBox.Show(@"בשימוש בתוכנה 'סטרימר פלוס' יש להסכים לתנאי השימוש, אנה קרא ובחר בסוף אם אתה מסכים או לא:
בשימוש בתוכנה יש התחברות לאתר 'יוטיוב' ו'סטרימלאבס' ההתחברות לשינהם נעשית על ידי התחברות לחשבון גוגל פלוס שלכם;
אני מסכים לאפשר לתכונה לנסות לאסוף מידע מהחשבון, מידע כמו - כמות הרשומים בחשבון יוטיוב המחובר, מזהה לייב פעיל-כתצואה מזה את הצאט, ומידע על תרומות.
חשוב לציין שהתוכנה לא משתמשת במידע הזה מעבר לשימוש היחידי שהוא על מנת להציג אותו למשתמש בתוכנה.
חשוב גם לציין שהתוכנה היא לא כחלק מיוטיוב או מסטרימלאבס ועובדת נפרדת להם אך מתבססת על ההצגה שלהם.
בשימוש בתוכנה המשתמש מאשר שהוא מסכים לתכונה לבצע את הדברים שנאמרו למעלה.", "תנאי שימוש", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                if (result == DialogResult.Yes)
                {
                    Properties.Settings.Default.Agreement = version;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    mainFormRef.Close();
                    Environment.Exit(0);
                }
            }
        }
    
        public static void CheckForUpdates(Form mainFormRef)
        {
            double serverVersion = double.Parse(cUrl("https://www.olympicangelabz.com/pages/StreamerPlus/version.txt"), new CultureInfo("en-US"));
            if (serverVersion > version)
            {
                DialogResult result = MessageBox.Show("האם תרצה להתקין אותה עכשיו?", "קיימת גרסה חדשה - " + serverVersion.ToString(new CultureInfo("en-US")), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("https://www.olympicangelabz.com/StreamerPlus");
                    mainFormRef.Close();
                    Environment.Exit(0);
                }
            }
        }

        public static string cUrl(string urlPath)
        {
            if (urlPath == null)
                return "";
            TimoutWebClient wbC = new TimoutWebClient();
            wbC.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            try
            {
                string data = wbC.DownloadString(urlPath);
                wbC.Dispose();
                return data;
            }
            catch (Exception)
            {
                return "1.0";
            }
        }

        public static Image PrintScreen(Form mainFormRef)
        {
            Bitmap bmp = Screenshot.TakeSnapshot(mainFormRef);
            BitmapFilter.GaussianBlur(bmp, 1);

            if (blurMapRenderCount >= 3)
            {
                blurMapRenderCount = 0;
                System.GC.Collect();
            }
            blurMapRenderCount++;
            return bmp;
        }
    }
    public static class Safe
    {
        public static void Invoke(Action action)
        {
            Control control = BrowserFlow.mainFormRef;
            if (control == null || action == null)
                return;

            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }
    }

#pragma warning disable CA1051, CA1815
    public class ConvMatrix
    {
        public int TopLeft,TopMid , TopRight,MidLeft , Pixel = 1, MidRight, BottomLeft , BottomMid, BottomRight,Factor = 1, Offset ;
        public void SetAll(int nVal)
        {
            TopLeft = TopMid = TopRight = MidLeft = Pixel = MidRight = BottomLeft = BottomMid = BottomRight = nVal;
        }
    }
    public static class BitmapFilter
    {
        private static bool Conv3x3(Bitmap b, ConvMatrix m)
        {
            // Avoid divide by zero errors
            if (0 == m.Factor) return false;

            Bitmap bSrc = (Bitmap)b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            int stride2 = stride * 2;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = stride + 6 - b.Width * 3;
                int nWidth = b.Width - 2;
                int nHeight = b.Height - 2;

                int nPixel;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nPixel = ((((pSrc[2] * m.TopLeft) + (pSrc[5] * m.TopMid) + (pSrc[8] * m.TopRight) +
                            (pSrc[2 + stride] * m.MidLeft) + (pSrc[5 + stride] * m.Pixel) + (pSrc[8 + stride] * m.MidRight) +
                            (pSrc[2 + stride2] * m.BottomLeft) + (pSrc[5 + stride2] * m.BottomMid) + (pSrc[8 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[5 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[1] * m.TopLeft) + (pSrc[4] * m.TopMid) + (pSrc[7] * m.TopRight) +
                            (pSrc[1 + stride] * m.MidLeft) + (pSrc[4 + stride] * m.Pixel) + (pSrc[7 + stride] * m.MidRight) +
                            (pSrc[1 + stride2] * m.BottomLeft) + (pSrc[4 + stride2] * m.BottomMid) + (pSrc[7 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[4 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[0] * m.TopLeft) + (pSrc[3] * m.TopMid) + (pSrc[6] * m.TopRight) +
                            (pSrc[0 + stride] * m.MidLeft) + (pSrc[3 + stride] * m.Pixel) + (pSrc[6 + stride] * m.MidRight) +
                            (pSrc[0 + stride2] * m.BottomLeft) + (pSrc[3 + stride2] * m.BottomMid) + (pSrc[6 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[3 + stride] = (byte)nPixel;

                        p += 3;
                        pSrc += 3;
                    }

                    p += nOffset;
                    pSrc += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);

            return true;
        }

        public static bool GaussianBlur(Bitmap b, int nWeight /* default to 4*/)
        {
            if (b == null)
                return false;
            ConvMatrix m = new ConvMatrix();
            m.SetAll(1);
            m.Pixel = nWeight;
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = 2;

            m.Factor = nWeight + 21;
            BitmapFilter.Conv3x3(b, m);
            m.Factor = nWeight + 11;
            BitmapFilter.Conv3x3(b, m);
            m.Factor = nWeight + 20;
            return BitmapFilter.Conv3x3(b, m);
        }
    }
    class Screenshot
    {
        public static Bitmap TakeSnapshot(Control ctl)
        {
            Bitmap bmp = new Bitmap(ctl.Size.Width, ctl.Size.Height);
            using (Graphics g = System.Drawing.Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(
                    ctl.PointToScreen(ctl.ClientRectangle.Location),
                    new Point(0, 0), ctl.ClientRectangle.Size
                );
            }
            return bmp;
        }
    }
    public class TimoutWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout =  5 * 1000;
            return w;
        }
    }
}
