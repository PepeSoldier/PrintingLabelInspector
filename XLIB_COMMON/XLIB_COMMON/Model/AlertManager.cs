using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using XLIB_COMMON.Interface;

namespace XLIB_COMMON.Model
{
    //SINGLETON!!
    public class AlertManager : IAlertManager
    {
        List<AlertModel> Alerts;
     
        private static readonly Object s_lock = new Object();
        private static AlertManager instance = null;
        public static AlertManager Instance
        {
            get
            {
                if (instance != null) return instance;
                Monitor.Enter(s_lock);
                AlertManager temp = new AlertManager();
                Interlocked.Exchange(ref instance, temp);
                Monitor.Exit(s_lock);
                return instance;
            }
        }

        private AlertManager()
        {
            Alerts = new List<AlertModel>();
        }
        
        public void AddAlert(AlertMessageType messageType, string message, string userName)
        {
            Alerts.Add(new AlertModel { MessageType = messageType, Message = message, UserName = userName });
        }
        public void AddAlertDBSaveProblem(string userName, string additionalMsg = "")
        {
            AddAlert(AlertMessageType.warning, "Zapis nie powiódł się. Sprobuj ponownie lub skontaktuj się z administratorem systemu. " + additionalMsg, userName);
        }
       
        public IEnumerable<IAlertModel> GetAlerts(string userName)
        {
            List<AlertModel> alertsCopy = new List<AlertModel>(Alerts.Where(x=>x.UserName == userName));
            Alerts.RemoveAll(x => x.UserName == userName);
            return alertsCopy;
        }
    }
}
