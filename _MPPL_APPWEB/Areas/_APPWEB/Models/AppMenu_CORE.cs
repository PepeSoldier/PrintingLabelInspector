using _MPPL_WEB_START.Areas._APPWEB.Models.Clients;

using MDL_BASE.Models;
using MDL_BASE.Models.IDENTITY;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace _MPPL_WEB_START.Areas._APPWEB.Models
{
    public static class MenuCore
    {
        public static MenuItem MenuItem = MenuSystemConfiguration();
        
        public static MenuItem MenuSystemConfiguration()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "KONFIGURACJA",
                HrefArea = "CORE",
                HrefAction = "Index",
                HrefController = "Home",
                Class1 = "fas fa-cogs",
                PictureName = "mdlConfig.png",
                AccessCode = (int)MenuAccessCode.COMMON_ACCESS,
                RequiredRole = DefRoles.USER,
                Children = new MenuItem[]
                {
                    Printers(),
                    //NotificationDevices(),
                }
            };
        }
        public static MenuItem Printers()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Drukarki",
                NameEnglish = "Printers",
                HrefArea = "CORE",
                HrefAction = "Printer",
                HrefController = "Printer",
                Class1 = "fas fa-print",
                AccessCode = (int)MenuAccessCode.COMMON_ACCESS,
                RequiredRole = DefRoles.ADMIN
            };
        }
        public static MenuItem NotificationDevices()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Powiadomienia",
                NameEnglish = "Notifications",
                HrefArea = "CORE",
                HrefAction = "Index",
                HrefController = "NotificationDevice",
                Class1 = "far fa-comment",
                AccessCode = (int)MenuAccessCode.COMMON_ACCESS,
                RequiredRole = DefRoles.USER
            };
        }
    }
}

