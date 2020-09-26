using MDL_iLOGIS.ComponentWHDOC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Model;

namespace MDL_iLOGIS.ComponentCore.Models
{
    public class iLogisMailer
    {
        MailerAbstract mailer;

        public iLogisMailer(MailerAbstract mailer)
        {
            this.mailer = mailer;
        }

        public void WzDoc(WhDocumentWZ whDocumentWZ, string receivers)
        {

            string ConfirmationMessage = whDocumentWZ.Status == ComponentWHDOC.Enums.EnumWhDocumentStatus.approved ? "POTWIERDZONY" : "ODRZUCONY"; 
            if (receivers != null && receivers.Length > 3 && mailer.Settings.Enabled)
            {
                string link = mailer.Settings.AppAddress + "/iLOGIS/WhDoc/Index/" + whDocumentWZ.Id;

                mailer.CreateNewMailMessage("WZ Informacja");
                mailer.MailMessage.To.Add(receivers);
                mailer.MailMessage.Subject = "Dokument WZ (id:" + whDocumentWZ.Id + ") został " + ConfirmationMessage;
                mailer.MailMessage.Body = "<h3>Odpowiedzialny: " + whDocumentWZ.Approver.Name + " " + whDocumentWZ.Approver.LastName + "</h3>";
                mailer.MailMessage.Body += "<h3>Kliknij poniższy link aby zobaczyć szczegóły:</h3><br><a href='" + link + "'>" + link + "</a><br><br>";
                mailer.SendMail();
            }
        }
    }
}
