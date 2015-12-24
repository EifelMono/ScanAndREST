using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.CodeDom.Compiler;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace ScanAndRESTServer
{
    public static class Helper
    {
        #region Clipboard

        private static List<string> ClipboardTexts = new List<string>();

        public static void ClipboardMainThread()
        {
            try
            {
                string newClipboardText = null;
                lock (ClipboardTexts)
                {
                    if (ClipboardTexts.Count > 0)
                    {
                        newClipboardText = ClipboardTexts[0];
                        ClipboardTexts.RemoveAt(0);
                    }
                }
                if (newClipboardText != null)
                {
                    switch (Environment.OSVersion.Platform)
                    {
                    #region MacOSX
                        case PlatformID.Unix:
                        case PlatformID.MacOSX:
                            TryExcept(() =>
                                {
                                    using (var process = new Process())
                                    {
                                        process.StartInfo = new ProcessStartInfo("pbcopy", "-pboard general -Prefer txt");
                                        process.StartInfo.UseShellExecute = false;
                                        process.StartInfo.RedirectStandardInput = true;
                                        process.StartInfo.RedirectStandardOutput = false;
                                        process.Start();
                                        process.StandardInput.Write(newClipboardText);
                                        process.StandardInput.Close();
                                        process.WaitForExit();
                                    }
                                });
                            break;
                    #endregion
                    #region Windows
                        default:
                            TryExcept(() => Clipboard.SetText(newClipboardText));
                            break;
                    #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public
        static void AddClipboard(string text)
        {
            lock (ClipboardTexts)
            {
                ClipboardTexts.Add(text);
            }
        }

        #endregion

        #region IpAddrress

        public static IPAddress[] GetIpAddresses()
        {
            return Dns.GetHostAddresses(Dns.GetHostName());
        }

        public static string ToIPAddressString(this IPAddress ipAddress)
        {
            return  string.Format("{0}.{1}.{2}.{3}",
                ipAddress.GetAddressBytes()[0],
                ipAddress.GetAddressBytes()[1],
                ipAddress.GetAddressBytes()[2],
                ipAddress.GetAddressBytes()[3]);
        }

        #endregion

        #region Others

        public static void TryExcept(Action action)
        {
            try
            {
                action();
            }
            catch
            {
            }
        }

        #endregion
    }
}

