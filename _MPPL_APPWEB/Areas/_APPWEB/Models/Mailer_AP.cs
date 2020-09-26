using MDL_AP.Models.ActionPlan;
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

        public void ActionCreated(ActionModel action, string receivers)
        {
            if (receivers.Length > 3 && mailer.Settings.Enabled)
            {
                string link = mailer.Settings.AppAddress + "/Action/Show/" + action.Id;

                mailer.CreateNewMailMessage("ActionPlans");
                mailer.MailMessage.To.Add(receivers);
                mailer.MailMessage.Subject = "Akcja (id:" + action.Id + ") została utworzona";
                mailer.MailMessage.Body = "<h3>Tytuł Akcji: " + action.Title + "</h3>";
                mailer.MailMessage.Body += "<b>Opis problemu:</b> " + action.Problem + "<br>";
                mailer.MailMessage.Body += "<b>Tworzący:</b> " + action.Creator.FullName + "<br>";
                mailer.MailMessage.Body += "<b>Data rozpoczęcia:</b> " + action.StartDate + "<br>";
                mailer.MailMessage.Body += "<b>Data Zakonczenia:</b> " + action.EndDate + "<br>";
                mailer.MailMessage.Body += "<b>Typ:</b> " + action.Type.Name + "<br>";
                mailer.MailMessage.Body += "<b>Obszar, stanowisko, zmiana:</b> " + action.Area.Name + ", " + action.Workstation.Name + ", " + action.ShiftCode.Name + "<br>";
                mailer.MailMessage.Body += "<b>Przypisany dział:</b> " + action.Department.Name + "<br>";
                mailer.MailMessage.Body += "<h3>Kliknij poniższy link aby zobaczyć szczegóły:</h3><br><a href='" + link + "'>" + link + "</a><br><br>";
                mailer.SendMail();
            }
        }
        public void ActionModified(ActionModel action, string receivers, string modificationDetails)
        {
            if (receivers.Length > 3 && mailer.Settings.Enabled && modificationDetails.Length > 0)
            {
                string link = mailer.Settings.AppAddress + "/Action/Show/" + action.Id;

                mailer.CreateNewMailMessage("ActionPlans");
                mailer.MailMessage.To.Add(receivers);
                mailer.MailMessage.Subject = "Akcja (id:" + action.Id + ") - modyfikacja parametrów";
                mailer.MailMessage.Body = "<h3>Tytuł Akcji: " + action.Title + "</h3>";
                mailer.MailMessage.Body += "<b>Opis problemu:</b> " + action.Problem + "<br>";
                mailer.MailMessage.Body += "<b>Tworzący:</b> " + action.Creator.FullName + "<br>";
                mailer.MailMessage.Body += "<b>Data rozpoczęcia:</b> " + action.StartDate + "<br>";
                mailer.MailMessage.Body += "<b>Data Zakonczenia:</b> " + action.EndDate + "<br>";
                mailer.MailMessage.Body += "<b>Typ:</b> " + action.Type.Name + "<br>";
                mailer.MailMessage.Body += "<b>Obszar, stanowisko, zmiana:</b> " + action.Area.Name + ", " + action.Workstation.Name + ", " + action.ShiftCode.Name + "<br>";
                mailer.MailMessage.Body += "<b>Przypisany dział:</b> " + action.Department.Name + "<br>";
                mailer.MailMessage.Body += "<h3>Zmiany w akcji:</h3>" + modificationDetails;
                mailer.MailMessage.Body += "<h3>Kliknij poniższy link aby przejść do akcji:</h3><br><a href='" + link + "'>" + link + "</a><br><br>";
                mailer.SendMail();
            }
        }
        public void ActionResponsibleChanged(ActionModel action, string receivers, string newResponsible)
        {
            if (receivers.Length > 3 && mailer.Settings.Enabled)
            {
                string link = mailer.Settings.AppAddress + "/Action/Show/" + action.Id;

                mailer.CreateNewMailMessage("ActionPlans");
                mailer.MailMessage.To.Add(receivers);
                mailer.MailMessage.Subject = "Akcja (id:" + action.Id + ") - zmiana osoby odpowiedzialnej";
                mailer.MailMessage.Body = "<h3>Tytuł Akcji: " + action.Title + "</h3>";
                mailer.MailMessage.Body += "<b>Opis problemu:</b> " + action.Problem + "<br>";
                mailer.MailMessage.Body += "<b>Tworzący:</b> " + action.Creator.FullName + "<br>";
                mailer.MailMessage.Body += "<b>Data rozpoczęcia:</b> " + action.StartDate + "<br>";
                mailer.MailMessage.Body += "<b>Data Zakonczenia:</b> " + action.EndDate + "<br>";
                mailer.MailMessage.Body += "<b>Typ:</b> " + action.Type.Name + "<br>";
                mailer.MailMessage.Body += "<b>Obszar, stanowisko, zmiana:</b> " + action.Area.Name + ", " + action.Workstation.Name + ", " + action.ShiftCode.Name + "<br>";
                mailer.MailMessage.Body += "<b>Przypisany dział:</b> " + action.Department.Name + "<br>";
                mailer.MailMessage.Body += "<h3>Osoba odpowiedzialna:</h3>" + newResponsible;
                mailer.MailMessage.Body += "<h3>Kliknij poniższy link aby przejść do akcji:</h3><br><a href='" + link + "'>" + link + "</a><br><br>";
                mailer.SendMail();
            }
        }
        public void ActionDepartmentChanged(ActionModel action, string receivers, string newDepartment)
        {
            if (receivers.Length > 3 && mailer.Settings.Enabled)
            {
                string link = mailer.Settings.AppAddress + "/Action/Show/" + action.Id;

                mailer.CreateNewMailMessage("ActionPlans");
                mailer.MailMessage.To.Add(receivers);
                mailer.MailMessage.Subject = "Akcja (id:" + action.Id + ") - zmiana działu odpowiedzialnego";
                mailer.MailMessage.Body = "<h3>Tytuł Akcji: " + action.Title + "</h3>";
                mailer.MailMessage.Body += "<b>Opis problemu:</b> " + action.Problem + "<br>";
                mailer.MailMessage.Body += "<b>Tworzący:</b> " + action.Creator.FullName + "<br>";
                mailer.MailMessage.Body += "<b>Data rozpoczęcia:</b> " + action.StartDate + "<br>";
                mailer.MailMessage.Body += "<b>Data Zakonczenia:</b> " + action.EndDate + "<br>";
                mailer.MailMessage.Body += "<b>Typ:</b> " + action.Type.Name + "<br>";
                mailer.MailMessage.Body += "<b>Obszar, stanowisko, zmiana:</b> " + action.Area.Name + ", " + action.Workstation.Name + ", " + action.ShiftCode.Name + "<br>";
                mailer.MailMessage.Body += "<b>Przypisany dział:</b> " + action.Department.Name + "<br>";
                mailer.MailMessage.Body += "<h3>Dział odpowiedzialny:</h3>" + newDepartment;
                mailer.MailMessage.Body += "<h3>Kliknij poniższy link aby przejść do akcji:</h3><br><a href='" + link + "'>" + link + "</a><br><br>";
                mailer.SendMail();
            }
        }
        public void ActionDepartmentManagerInfo(ActionModel action, string receivers, string newDepartment)
        {
            if (receivers.Length > 3 && mailer.Settings.Enabled)
            {
                string link = mailer.Settings.AppAddress + "/Action/Show/" + action.Id;

                mailer.CreateNewMailMessage("ActionPlans");
                mailer.MailMessage.To.Add(receivers);
                mailer.MailMessage.Subject = "Akcja (id:" + action.Id + ") - przypisana do twojego działu";
                mailer.MailMessage.Body = "<h3>Tytuł Akcji: " + action.Title + "</h3>";
                mailer.MailMessage.Body += "<b>Opis problemu:</b> " + action.Problem + "<br>";
                mailer.MailMessage.Body += "<b>Tworzący:</b> " + action.Creator.FullName + "<br>";
                mailer.MailMessage.Body += "<b>Data rozpoczęcia:</b> " + action.StartDate + "<br>";
                mailer.MailMessage.Body += "<b>Data Zakonczenia:</b> " + action.EndDate + "<br>";
                mailer.MailMessage.Body += "<b>Typ:</b> " + action.Type.Name + "<br>";
                mailer.MailMessage.Body += "<b>Obszar, stanowisko, zmiana:</b> " + action.Area.Name + ", " + action.Workstation.Name + ", " + action.ShiftCode.Name + "<br>";
                mailer.MailMessage.Body += "<b>Przypisany dział:</b> " + action.Department.Name + "<br>";
                mailer.MailMessage.Body += "<h3>Dział odpowiedzialny:</h3>" + newDepartment;
                mailer.MailMessage.Body += "<h3>Kliknij poniższy link aby przejść do akcji:</h3><br><a href='" + link + "'>" + link + "</a><br><br>";
                mailer.SendMail();
            }
        }
        public void ActionStatusChanged(ActionModel action, string receivers, string newStatus)
        {
            if (receivers.Length > 3 && mailer.Settings.Enabled)
            {
                string link = mailer.Settings.AppAddress + "/Action/Show/" + action.Id;

                mailer.CreateNewMailMessage("ActionPlans");
                mailer.MailMessage.To.Add(receivers);
                mailer.MailMessage.Subject = "Akcja (id:" + action.Id + ") - zmiana statusu";
                mailer.MailMessage.Body = "<h3>Tytuł Akcji: " + action.Title + "</h3>";
                mailer.MailMessage.Body += "<b>Opis problemu:</b> " + action.Problem + "<br>";
                mailer.MailMessage.Body += "<b>Tworzący:</b> " + action.Creator.FullName + "<br>";
                mailer.MailMessage.Body += "<b>Data rozpoczęcia:</b> " + action.StartDate + "<br>";
                mailer.MailMessage.Body += "<b>Data Zakonczenia:</b> " + action.EndDate + "<br>";
                mailer.MailMessage.Body += "<b>Typ:</b> " + action.Type.Name + "<br>";
                mailer.MailMessage.Body += "<b>Obszar, stanowisko, zmiana:</b> " + action.Area.Name + ", " + action.Workstation.Name + ", " + action.ShiftCode.Name + "<br>";
                mailer.MailMessage.Body += "<b>Przypisany dział:</b> " + action.Department.Name + "<br>";
                mailer.MailMessage.Body += "<h3>Status:</h3>" + newStatus;
                mailer.MailMessage.Body += "<h3>Kliknij poniższy link aby przejść do akcji:</h3><br><a href='" + link + "'>" + link + "</a><br><br>";
                mailer.SendMail();
            }
        }
        public void ActionActivityAdded(ActionModel action, string receivers, string activityDetails, string creatorUserFullName)
        {
            if (receivers.Length > 3 && mailer.Settings.Enabled)
            {
                string link = mailer.Settings.AppAddress + "/Action/Show/" + action.Id;

                mailer.CreateNewMailMessage("ActionPlans");
                mailer.MailMessage.To.Add(receivers);
                mailer.MailMessage.Subject = "Akcja (id:" + action.Id + ") - dodano aktywność";
                mailer.MailMessage.Body = "<h3>Tytuł Akcji: " + action.Title + "</h3>";
                mailer.MailMessage.Body += "<b>Opis problemu:</b> " + action.Problem + "<br>";
                mailer.MailMessage.Body += "<b>Tworzący:</b> " + action.Creator.FullName + "<br>";
                mailer.MailMessage.Body += "<b>Data rozpoczęcia:</b> " + action.StartDate + "<br>";
                mailer.MailMessage.Body += "<b>Data Zakonczenia:</b> " + action.EndDate + "<br>";
                mailer.MailMessage.Body += "<b>Typ:</b> " + action.Type.Name + "<br>";
                mailer.MailMessage.Body += "<b>Obszar, stanowisko, zmiana:</b> " + action.Area.Name + ", " + action.Workstation.Name + ", " + action.ShiftCode.Name + "<br>";
                mailer.MailMessage.Body += "<b>Przypisany dział:</b> " + action.Department.Name + "<br>";
                mailer.MailMessage.Body += "<h3>Aktywność:</h3>" + creatorUserFullName + " - " + activityDetails;
                mailer.MailMessage.Body += "<h3>Kliknij poniższy link aby przejść do akcji:</h3><br><a href='" + link + "'>" + link + "</a><br><br>";
                mailer.SendMail();
            }
        }
    }
}