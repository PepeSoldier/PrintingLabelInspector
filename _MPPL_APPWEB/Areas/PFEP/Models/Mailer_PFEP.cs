using MDL_BASE.Models.IDENTITY;
using MDL_PFEP.Model.ELDISY_PFEP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using XLIB_COMMON.Model;

namespace _MPPL_WEB_START.Areas.PFEP.Models
{
    public class Mailer_PFEP
    {
        MailerAbstract mailer;

        public Mailer_PFEP(MailerAbstract mailer)
        {
            this.mailer = mailer;
        }

        //public void CreateNewMailMessage(int id, string Action = "Show")
        //{
        //    mailer.MailMessage = new MailMessage();
        //    mailer.link = mailer.AppAdress + "/PFEP/PackingInstruction/" + Action + "/" + id;
        //    mailer.MailMessage.From = new MailAddress("instrukcjePakowania@eldisy.pl", "");
        //    mailer.MailMessage.IsBodyHtml = true;
        //    mailer.MailMessage.BodyEncoding = System.Text.Encoding.GetEncoding(1252);

        //    mailer.MailMessage.Body = "<h3> Kliknij w poniższy link aby zobaczyć szczegóły:</h3>" +
        //                              "<br><a href = '" + mailer.link + "' > " + mailer.link + " </a><br>" +
        //                              "<br> ";
        //}
        private string MailBody_1()
        {
            return "<h3> Kliknij w poniższy link aby zobaczyć szczegóły:</h3>" +
                   "<br><a href = '" + mailer.Link + "' > " + mailer.Link + " </a><br>" +
                   "<br> ";
        }

        public void NewCorrection(PackingInstruction pi, List<User> UsersList)
        {
            //this.mailer = new Mailer();
            if (UsersList.Count > 0)
            {
                //mailer.MailMessage = new MailMessage();
                //mailer.MailMessage.From = new MailAddress("instrukcjePakowania@eldisy.pl", "");
                //mailer.MailMessage.IsBodyHtml = true;
                //mailer.MailMessage.BodyEncoding = System.Text.Encoding.GetEncoding(1252);

                //mailer.MailMessage.Body = "<h3> Kliknij w poniższy link aby zobaczyć szczegóły:</h3>" +
                //                          "<br><a href = '" + mailer.Link + "' > " + mailer.Link + " </a><br>" +
                //                          "<br> ";

                mailer.CreateNewMailMessage("Instrukcje Pakowania");
                mailer.Link = mailer.Settings.AppAddress + "/PFEP/PackingInstruction/Edit/" + pi.Id;
                mailer.MailMessage.Body = MailBody_1();

                foreach (User receiver in UsersList)
                {
                    if (receiver.Email != null)
                    {
                        mailer.MailMessage.To.Add(receiver.Email);
                        mailer.MailMessage.Subject = "Nowa poprawka zgłoszona do instrukcji o numerze: " + pi.Id;
                        mailer.SendMail();
                    }
                }
            }
        }

        public void NewPackingInstruction(PackingInstruction pi, User receiver)
        {
            if(receiver != null)
            {
                //CreateNewMailMessage(pi.Id);
                //mailer.MailMessage.Body = "<h3> Kliknij w poniższy link aby zobaczyć szczegóły:</h3>" +
                //                          "<br><a href = '" + mailer.Link + "' > " + mailer.Link + " </a><br>" +
                //                          "<br> ";
                mailer.CreateNewMailMessage("Instrukcje Pakowania");
                mailer.MailMessage.Body = MailBody_1();

                if (receiver.Email != null)
                {
                    mailer.MailMessage.To.Add(receiver.Email);

                    if (pi.Id == 0)
                    {
                        mailer.MailMessage.Subject = "Nowa Instrukcja Pakowania wymaga Twojej akcji.";
                    }
                    else
                    {
                        mailer.MailMessage.Subject = "Instrukcja Pakowania o numerze: " + pi.Id + " wymaga Twojej akcji.";
                    }
                    mailer.SendMail();
                }
            }
        }

        public void ConfirmedPackingInstruction(PackingInstruction pi, User receiver)
        {
            if (receiver != null)
            {
                //CreateNewMailMessage(pi.Id);
                //mailer.MailMessage.Body = "<h3> Kliknij w poniższy link aby zobaczyć szczegóły:</h3>" +
                //                         "<br><a href = '" + mailer.Link + "' > " + mailer.Link + " </a><br>" +
                //                         "<br> ";
                mailer.CreateNewMailMessage("Instrukcje Pakowania");
                mailer.MailMessage.Body = MailBody_1();


                if (receiver.Email != null)
                {
                    mailer.MailMessage.To.Add(receiver.Email);
                    if (pi.Id == 0)
                    {
                        mailer.MailMessage.Subject = "Zatwierdzona Instrukcja Pakowania";
                    }
                    else
                    {
                        mailer.MailMessage.Subject = "Zatwierdzona Instrukcja Pakowania o numerze: " + pi.Id;
                    }
                    mailer.SendMail();
                }
            }
        }

        public void RejectedPackingInstruction(PackingInstruction pi, List<User> UsersList)
        {
            if (UsersList.Count > 0)
            {
                //CreateNewMailMessage(pi.Id);
                //mailer.MailMessage.Body = "<h3> Kliknij w poniższy link aby zobaczyć szczegóły:</h3>" +
                //                         "<br><a href = '" + mailer.Link + "' > " + mailer.Link + " </a><br>" +
                //                         "<br> ";
                mailer.CreateNewMailMessage("Instrukcje Pakowania");
                mailer.MailMessage.Body = MailBody_1();

                foreach (User receiver in UsersList)
                {
                    if (receiver.Email != null)
                    {
                        mailer.MailMessage.To.Add(receiver.Email);
                        mailer.MailMessage.Subject = "Odrzucona Instrukcja Pakowania o numerze: " + pi.Id;

                        mailer.SendMail();
                    }
                }
            }
        }
    }
}