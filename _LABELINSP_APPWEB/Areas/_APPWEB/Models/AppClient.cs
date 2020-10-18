using _LABELINSP_APPWEB.Areas._APPWEB.Models.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _LABELINSP_APPWEB.Areas._APPWEB.Models
{

    public static class AppClient
    {
        public static AppClientApbstract appClient;
        public static AppClientApbstract GetClient(string clientName)
        {
            switch (clientName)
            {
                case "ElectroluxPLV": appClient = new AppClientElectroluxPLV(); break;
                case "PackingLabel": appClient = new AppClientElectroluxPLV(); break;
                case "Dev": appClient = new AppClientDev(); break;
                case "DevK": appClient = new AppClientDev(); break;
                case "DevP": appClient = new AppClientDev(); break;
                case "DevM": appClient = new AppClientDev(); break;
                default: appClient = new AppClientUnknown(); break;
            }

            return appClient;
        }
    }
}