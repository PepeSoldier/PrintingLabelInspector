using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Z_LUNCHER
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseUrl = Properties.Settings.Default.BaseURL;
            string commandPath = "\"Program Files (x86)\"";
            string commandDelims = "\"delims=[] tokens=2\"";
            string str1 = "for /f " + commandDelims + " %%a in ('ping -4 -n 1 %ComputerName% ^| findstr [') do set NetworkIP=%%a";
            //string str2 = @"START C:\" + commandPath + @"\Google\Chrome\Application\chrome.exe --start-fullscreen --incognito --app=http://localhost:49962/ONEPROD/MES/Workplace/1?ip=%NetworkIP%";
            //string str2 = @"START C:\" + commandPath + @"\Google\Chrome\Application\chrome.exe --incognito http://localhost:49962/ONEPROD/MES/Workplace/1?ip=" + GetLocalIPAddress();
            string str2 = @"START C:\" + commandPath + @"\Google\Chrome\Application\chrome.exe --start-fullscreen --incognito --app=" + baseUrl + "/ONEPROD/MES/Workplace/1?ip=" + GetLocalIPAddress();

            //StreamWriter sw = new StreamWriter("implea_luncher.bat");
            //sw.WriteLine(str1);
            //sw.WriteLine(str2);
            //sw.Close();
            //Process.Start("implea_luncher.bat");
            Process[] processCollection = Process.GetProcesses();
            bool isAppRunning = false;
            foreach (Process p in processCollection)
            {
                if(p.ProcessName == "chrome")
                {
                    isAppRunning = true;
                }

            }
            if (isAppRunning)
            {
                Console.WriteLine("Masz już uruchomioną aplikację ... ");
                Console.WriteLine("Wciśnij ENTER");
                Console.ReadKey();
            }
            else
            {
                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();
                //cmd.StandardInput.WriteLine(str1);
                cmd.StandardInput.WriteLine(str2);
                //cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
