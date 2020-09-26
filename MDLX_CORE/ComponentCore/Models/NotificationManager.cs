using MDL_CORE.ComponentCore.Entities;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using WebPush;
using XLIB_COMMON.Model;

namespace MDL_CORE.ComponentCore.Models
{
    public static class NotificationManager
    {
        public static string PublicKey = "BGL5SjteTYVrwoqDifDnsFksDMnGK2e2Hnvpx0J6adOeV8k2iHePivazsNRfvind0HvidNJwJ9aos_00qNZPfkM";
        public static string PrivateKey = "70M3JSvzw-6tVxMvfUA7PcI4KFQjcE1EFs4ldCwR1nI";
        public static string VapidSubject = "mailto:piotr.kaskow@implea.pl";

        public static string SendNotification(UnitOfWorkCore uow, string userId, string title, string bodyTxt, string link)
        {
            string msg = "";
            List<NotificationDevice> deviceList = uow.NotificationDeviceRepo.GetByUserId(userId);
            if (deviceList == null)
            {
                msg = "Brak urządzeń dla tego użytkownika";
            }
            else
            {
                var payload = new
                {
                    notification = new
                    {
                        title = title,
                        body = bodyTxt,
                        badge = link
                    }
                };
                var json = new JavaScriptSerializer().Serialize(payload);

                foreach (var device in deviceList)
                {
                    var pushSubscription = new PushSubscription(device.PushEndpoint, device.PushP256DH, device.PushAuth);
                    var vapidDetails = new VapidDetails(VapidSubject, PublicKey, PrivateKey);
                    var webPushClient = new WebPushClient();
                    try
                    {
                        webPushClient.SendNotification(pushSubscription, json, vapidDetails);
                    }
                    catch(Exception ex)
                    {
                        Logger2FileSingleton.Instance.SaveLog("NotificationManager.SendNotification: " + ex.Message);
                    }
                }
            }
            return msg;
        }

        [Obsolete]
        public static void SendSMS_old(string phoneNumber, string link)
        {
            try
            {
                string URI = "http://admin:Qwert123@10.26.14.14/values.xml?Cmd=SMS&Nmr=" + phoneNumber + "&Text= Dokument WZ do zaakcpetowania. Kliknij w link: " + link;
                
                WebClient webClient = new WebClient();
                Stream stream = webClient.OpenRead(URI);
            }
            catch (Exception ex)
            {   
            }
        }

        public static void SendSMS(string number, string link)
        {
            string msg = "Nowy dokument WZ do zaakcpetowania: ";
            string fullPath = "http://10.26.14.14/values.xml?Cmd=SMS&Nmr=" + number + "&Text=" + msg + link;

            WebRequest webClient = WebRequest.Create(fullPath);
            webClient.Method = "GET";
            webClient.UseDefaultCredentials = true;
            webClient.ContentType = "text/xml";
            webClient.Credentials = new NetworkCredential("admin", "Qwert123");

            try
            {
                var resp = webClient.GetResponse();
            }
            catch (Exception ex)
            {
                Logger2FileSingleton.Instance.SaveLog("NotificationManager.SendSMS: " + ex.Message);
            }
        }

    }
}