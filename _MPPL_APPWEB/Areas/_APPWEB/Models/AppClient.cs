using _MPPL_WEB_START.Areas._APPWEB.Models.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas._APPWEB.Models
{

    public static class AppClient
    {
        public static AppClientApbstract appClient;
        public static AppClientApbstract GetClient(string clientName)
        {
            switch (clientName)
            {
                case "ElectroluxPLB": appClient = new AppClientElectroluxPLB(); break;
                case "ElectroluxPLS": appClient = new AppClientElectroluxPLS(); break;
                case "ElectroluxPLV": appClient = new AppClientElectroluxPLV(); break;
                case "Eldisy": appClient = new AppClientEldisy(); break;
                case "Eldisy2": appClient = new AppClientEldisy2(); break;
                case "Grandhome": appClient = new AppClientGrandhome(); break;
                case "WRP": appClient = new AppClientWRP(); break;
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