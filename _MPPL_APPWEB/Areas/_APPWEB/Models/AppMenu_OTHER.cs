using _MPPL_WEB_START.Areas._APPWEB.Models.Clients;
using MDL_AP.Models;
using MDL_BASE.Models;
using MDL_BASE.Models.IDENTITY;
using MDL_PFEP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace _MPPL_WEB_START.Areas._APPWEB.Models
{
    public static class MenuOTHER
    {
        public static MenuItem MenuItem = MenuHSEItem();

        public static MenuItem MenuHSEItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "KRZYŻ BEZPIECZEŃSTWA",
                HrefArea = "OTHER",
                HrefAction = "Cross",
                HrefController = "HSE",
                Class1 = "fas fa-cross",
                PictureName = "mdlHSE.png",
                AccessCode = (int)MenuAccessCode.OTHER_HSE,
            };
        }
    }
}

