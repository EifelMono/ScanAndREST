using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace ScanAndRESTServer
{
    public static class Helper
    {
        public static void ClipboarCopy(string text)
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX:
                    #region MacOSX
                    try
                    {
                        using (var process = new Process())
                        {
                            process.StartInfo = new ProcessStartInfo("pbcopy", "-pboard general -Prefer txt");
                            process.StartInfo.UseShellExecute = false;
                            process.StartInfo.RedirectStandardInput = true;
                            process.StartInfo.RedirectStandardOutput = false;
                            process.Start();
                            process.StandardInput.Write(text);
                            process.StandardInput.Close();
                            process.WaitForExit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                    }
                    break;
                    #endregion
                case PlatformID.Unix:
                    break;
                default:
                    {
                        Clipboard.SetText(text); 
                        break;
                    }
            }
        }
    }
}

