using IMPLEA.HttpServices;
using IMPLEA.NetworkServices.TCP;
using IMPLEA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Model;

namespace MDL_LABELINSP.Models
{
    public class Tcp2Web
    {
        List<TcpListener> tcpListeners;
        WebApi webApi;

        public Tcp2Web(string apiAddress, string apiCallUrl)
        {
            tcpListeners = new List<TcpListener>();
            webApi = new WebApi(apiAddress, apiCallUrl, (ILogger)Logger2FileSingleton.Instance);
        }

        public void RegisterAndRunTCPListener(string ip, string strport)
        {
            int port = 0;

            try { port = Convert.ToInt32(strport); }
            catch { }

            if (port > 0)
            {
                Logger2FileSingleton.Instance.SaveLog("Run Tcp Listener for " + ip + ":" + strport);

                string name = strport;
                TcpListener tcp = new TcpListener(name, ip, port, (ILogger)Logger2FileSingleton.Instance);
                tcpListeners.Add(tcp);

                tcp.RegisterObserver(webApi);
                tcp.StartTCP();
            }
            else
            {
                Logger2FileSingleton.Instance.SaveLog("Cannot listen the port: " + strport + ". Make sure you put the number 0-65536.");
            }
        }

        public void Stop()
        {
            foreach (TcpListener tcp in tcpListeners)
            {
                Logger2FileSingleton.Instance.SaveLog("Stopping TCP Listener " + tcp.IP + ":" + tcp.Port.ToString());
                tcp.StopTCP();
            }
            tcpListeners = null;
            Logger2FileSingleton.Instance.SaveLog("All TCP Listeners stopped");
            webApi = null;
            Logger2FileSingleton.Instance.SaveLog("WebApi closed");
        }
    }
}
