using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Model;

namespace _LABELINSP_APPWEB.Models
{
    
    public class MailerSettingsLoader
    {
        public static MailerSettings LoadSettings()
        {
            return new MailerSettings() {
                AppAddress = Properties.Settings.Default.AppAdress,
                ReceiverCC = Properties.Settings.Default.EmailReceiverCC,
                Receiver = Properties.Settings.Default.EmailReceiver,
                Enabled = Properties.Settings.Default.EmailsEnabled,
                SenderMail = Properties.Settings.Default.EmailSenderMail,
                SenderDisplayName = Properties.Settings.Default.EmailSenderDisplayName,
                SenderDisplayNameSuffix = Properties.Settings.Default.EmailSenderDisplayNameSuffix,
                SmtpPort = Properties.Settings.Default.SmtpPort,
                SmtpPwd = Properties.Settings.Default.SmtpPwd,
                SmtpServer = Properties.Settings.Default.SmtpServer,
                SmtpUser = Properties.Settings.Default.SmtpUser
            };
        }
    }

    public class Mailer : MailerAbstract
    {
        public Mailer(MailerSettings mailerSettings) : base(mailerSettings)
        {
        }

        public static Mailer Create()
        {
            return new Mailer(MailerSettingsLoader.LoadSettings());
        }
    }

    //public class MailerEldisy : MailerAbstract
    //{
    //    public MailerEldisy(MailerSettings mailerSettings) : base(mailerSettings)
    //    {
    //    }

    //    public static MailerEldisy Create()
    //    {
    //        return new MailerEldisy(MailerSettingsLoader.LoadSettings());
    //    }
    //}

    //public class MailerElectrolux : MailerAbstract
    //{
    //    public MailerElectrolux(MailerSettings mailerSettings) : base(mailerSettings)
    //    {
    //    }

    //    public static MailerElectrolux Create()
    //    {
    //        return new MailerElectrolux(MailerSettingsLoader.LoadSettings());
    //    }
    //    //Other Settings Are Configurable from web.Config
    //    //public override void CreateNewMailMessage(string senderDisplayName = "")
    //    //{
    //    //    string displayName;
    //    //    displayName = senderDisplayName == "" ? Settings.SenderDisplayName : senderDisplayName;
    //    //    displayName = displayName + Settings.SenderDisplayNameSuffix;

    //    //    mailMessage = new MailMessage();
    //    //    mailMessage.From = new MailAddress("noreply@electrolux.com", displayName);
    //    //    mailMessage.CC.Add("kamil.krzyzanowski@electrolux.com");
    //    //    mailMessage.IsBodyHtml = true;
    //    //    mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding(1252);
    //    //}
    //}
}