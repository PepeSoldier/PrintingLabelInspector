using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;

namespace XLIB_COMMON.Model
{
    public class Logger2FileSingleton : Logger2File
    {
        static string LogFilePath = XLIB_COMMON.Properties.Settings.Default.LogFilePath;

        private Logger2FileSingleton() : base("MPPL_LOG", LogFilePath, AppDomain.CurrentDomain.BaseDirectory)
        {
        }

        private static Logger2FileSingleton instance;
        public static Logger2FileSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Logger2FileSingleton();
                    }
                }
                return instance;
            }
        }
    }

    public class Logger2File : ILogger
    {
        protected static object syncRoot = new Object();
        private string filename;
        private string path;

        public Logger2File(string logFileName, string logFilePath = "", string baseDirectory = "")
        {
            filename = logFileName;
            path = logFilePath;
            path = path == "" ? baseDirectory : path;
            path = path == "" ? @"C:\Log" : path;
            //AppDomain.CurrentDomain.BaseDirectory
        }

        public void SaveLog(string msg)
        {
            SaveLog(msg, "");
        }
        public void SaveLog(string msg, int prefix = 0)
        {
            SaveLog(msg, prefix.ToString());
        }
        public void SaveLog_NoTime(string msg, int prefix = 0)
        {
            SaveLog_NoTime(msg, prefix.ToString());
        }
        public void SaveLog(string msg, string prefix = "0")
        {
            string txt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + msg;
            //Console.WriteLine(txt);
            writeToFile(txt, prefix);
        }
        public void SaveLog_NoTime(string msg, string prefix = "0")
        {
            //Console.WriteLine(msg);
            writeToFile(msg, prefix);
        }

        private void writeToFile(string msg, string prefix)
        {
            try
            {
                lock (syncRoot)
                {
                    using (StreamWriter writer = File.AppendText(path + "\\" + filename + "_" + prefix.ToString() + "_" + DateTime.Now.Date.ToString("yyyyMMdd") + ".txt"))
                    {
                        writer.WriteLine(msg);
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch
            {
                //Console.WriteLine("Sava Log Failed")
            }
        }
    }
}