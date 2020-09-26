using MDL_BASE.Models.IDENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.Models
{
    public class MailHelper
    {
        public static string GetUserEmails(List<User> users)
        {
            string emails = string.Empty;

            foreach (User user in users)
            {
                emails += GetUserEmail(user, emails, string.Empty);
            }

            return emails;
        }

        public static string GetUserEmail(User user, string currentEmails, string currentUserName)
        {
            if (user != null && user.UserName != currentUserName)
            {
                if (currentEmails != string.Empty && user.Email != null)
                    currentEmails += ", ";

                currentEmails += (user.Email != null) ? user.Email : string.Empty;
            }

            return currentEmails;
        }
    }
}