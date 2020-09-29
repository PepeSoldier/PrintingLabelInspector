using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Model;

namespace _MPPL_WEB_START.Areas.Models
{
    public class Mailer_AP
    {
        MailerAbstract mailer;

        public Mailer_AP(MailerAbstract mailer)
        {
            this.mailer = mailer;
        }

        //public void ActionCreated(ActionModel action, string receivers)
        //{
        //    if (receivers.Length > 3 && mailer.Settings.Enabled)
        //    {
        //        string link = mailer.Settings.AppAddress + "/Action/Show/" + action.Id;

        //        mailer.CreateNewMailMessage("ActionPlans");
        //        mailer.MailMessage.To.Add(receivers);
        //        mailer.MailMessage.Subject = "Akcja (id:" + action.Id + ") została utworzona";
        //        mailer.MailMessage.Body = "<h3>Tytuł Akcji: " + action.Title + "</h3>";
        //        mailer.MailMessage.Body += "<b>Opis problemu:</b> " + action.Problem + "<br>";
        //        mailer.MailMessage.Body += "<b>Tworzący:</b> " + action.Creator.FullName + "<br>";
        //        mailer.MailMessage.Body += "<b>Data rozpoczęcia:</b> " + action.StartDate + "<br>";
        //        mailer.MailMessage.Body += "<b>Data Zakonczenia:</b> " + action.EndDate + "<br>";
        //        mailer.MailMessage.Body += "<b>Typ:</b> " + action.Type.Name + "<br>";
        //        mailer.MailMessage.Body += "<b>Obszar, stanowisko, zmiana:</b> " + action.Area.Name + ", " + action.Workstation.Name + ", " + action.ShiftCode.Name + "<br>";
        //        mailer.MailMessage.Body += "<b>Przypisany dział:</b> " + action.Department.Name + "<br>";
        //        mailer.MailMessage.Body += "<h3>Kliknij poniższy link aby zobaczyć szczegóły:</h3><br><a href='" + link + "'>" + link + "</a><br><br>";
        //        mailer.SendMail();
        //    }
        //}
    }
}