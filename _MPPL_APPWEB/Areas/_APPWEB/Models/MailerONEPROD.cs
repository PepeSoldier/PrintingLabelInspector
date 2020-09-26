using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Model;

namespace _MPPL_WEB_START.Areas._APPWEB.Models
{
    public class MailerONEPROD
    {
        MailerAbstract mailer;

        public MailerONEPROD(MailerAbstract mailer)
        {
            this.mailer = mailer;
        }

        public void OeeReport(string emailDataHtml, string receivers, DateTime date)
        {
            if (receivers.Length > 3)
            {
                mailer.CreateNewMailMessage("ONEPROD OEE");
                mailer.MailMessage.To.Add(receivers);
                mailer.MailMessage.Subject = "OEE Raport za dzień: " + date.ToString("yyyy-MM-dd");
                mailer.MailMessage.Body = emailDataHtml;
                mailer.SendMail();
            }
        }
    }
}