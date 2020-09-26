using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace MDL_ONEPROD.Common
{
    public class NotificationManager
    {
        List<Notification> notifications;
        List<int> ids;
        private static readonly Object s_lock = new Object();
        private static NotificationManager instance = null;
        public static NotificationManager Instance
        {
            get
            {
                if (instance != null) return instance;
                Monitor.Enter(s_lock);
                NotificationManager temp = new NotificationManager();
                Interlocked.Exchange(ref instance, temp);
                Monitor.Exit(s_lock);
                return instance;
            }
        }

        private NotificationManager()
        {
            notifications = new List<Notification>();
            ids = new List<int>();
            ids.Add(0);
        }
        private int addId(int id)
        {
            if (id < 1)
            {
                int idnew = ids.Max() + 1;
                ids.Add(idnew);
                return idnew;
            }
            else
            {
                for (int i = 1; i < ids.Count; i++)
                {
                    if (ids[i] == id)
                        return id;
                }
                ids.Add(id);
                return id;
            }
        }
        
        //block
        public int AddNotificationBlock(string message, string status = "", string receiver = "", int id = 0)
        {
            int newId = addId(id);

            lock(notifications)
            {
                notifications.Add(new Notification { Message = message, Status = status, Receiver = receiver, Id = newId, Type = 1, DateTime = DateTime.Now });
            }
            return newId;
        }
        //log
        public int AddNotificationLog(string message, string status = "", string receiver = "")
        {
            int newId = 0;

            lock (notifications)
            {
                notifications.Add(new Notification { Message = message, Status = status, Receiver = receiver, Id = newId, Type = 2, DateTime = DateTime.Now });
            }
            return newId;
        }
        
        public List<Notification> GetNotifications(string receiver)
        {
            List<Notification> ntCopy;

            lock(notifications)
            {
                notifications.RemoveAll(n => n.Receiver == null);
                notifications.RemoveAll(n => n.DateTime < DateTime.Now.AddMinutes(-5));

                List<Notification> nt = notifications.Where(n => n.Receiver == receiver).ToList();
                ntCopy = new List<Notification>(nt);
                notifications.RemoveAll(n => n.Receiver == receiver);
            }
            
            return ntCopy;
        }

    }
}