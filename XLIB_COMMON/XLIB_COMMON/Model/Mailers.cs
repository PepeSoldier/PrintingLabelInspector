using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace XLIB_COMMON.Model
{
    public class MailerSettings
    {
        public bool Enabled { get; set; }

        public string AppAddress { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPwd { get; set; }
        public int SmtpPort { get; set; }
        public string SenderMail { get; set; }
        public string SenderDisplayName { get; set; }
        public string SenderDisplayNameSuffix { get; set; }
        public string ReceiverCC { get; set; }
        public string Receiver { get; set; }
    }

    public abstract class MailerAbstract
    {
        public MailerSettings Settings { get; set; }

        public MailerAbstract(MailerSettings mailerSettings)
        {
            //appAdress = Properties.Settings.Default.AppAdress; 
            //enabled = Properties.Settings.Default.EmailsEnabled;

            Settings = mailerSettings;

            #if DEBUG
            Settings.Enabled = false;
            #endif

            string smtpServer = Settings.SmtpServer;
            string user = Settings.SmtpUser;
            string pwd = Settings.SmtpPwd;
            int port = Settings.SmtpPort;

            this.smtpServer = new SmtpClient(smtpServer, port);

            if (user != "" && pwd != "")
            {
                this.smtpServer.UseDefaultCredentials = false;
                this.smtpServer.Credentials = new System.Net.NetworkCredential(user, pwd);
            }
        }
        
        //public bool Enabled { get => enabled; set => enabled = value; }
        //public string AppAdress { get => appAdress; set => appAdress = value; }
        public MailMessage MailMessage { get => mailMessage; }

        protected MailMessage mailMessage;
        protected SmtpClient smtpServer;
        //protected string appAdress;
        //protected bool enabled;
        public string Link;

        public void CreateNewMailMessage(string senderDisplayName = "")
        {
            string displayName = senderDisplayName == "" ? Settings.SenderDisplayName : senderDisplayName;
            displayName = displayName + Settings.SenderDisplayNameSuffix;

            mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(Settings.SenderMail, displayName);
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding(1252);

            if (Settings.ReceiverCC.Length > 3)
                mailMessage.CC.Add(Settings.ReceiverCC);
            
        }

        public void SendMail()
        {
            smtpServer.Send(mailMessage);
        }
    }
}