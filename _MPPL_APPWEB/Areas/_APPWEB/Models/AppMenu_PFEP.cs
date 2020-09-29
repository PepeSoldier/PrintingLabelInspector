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
    public static class MenuPFEP
    {
        public static MenuItem MenuItem = BuildMenu();
        public static MenuItem BuildMenu()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "PFEP",
                HrefArea = "PFEP",
                HrefAction = "Index",
                HrefController = "Home",
                Class1 = "fas fa-dolly-flatbed",
                PictureName = "mdl_pfep.jpg",
                AccessCode = (int)MenuAccessCode.PFEP_ACCESS,
                Children = new MenuItem[] {
                    MenuMPPL_Home.MenuItem,
                    PackingInstrItem(),
                    CalculationItem(),
                    LineFeedingItem(),
                    PackagesItem(),
                    AreasItem(),
                    MenuMPPL_Users.MenuItem,
                }
            };
        }

        public static MenuItem PackingInstrItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Instr. Pak.",
                HrefArea = "PFEP",
                HrefAction = "Browse",
                HrefController = "PackingInstruction",
                Class1 = "far fa-file-alt",
                AccessCode = (int)MenuAccessCode.PFEP_PCKINSTR_ACCESS
            };
        }
        public static MenuItem CalculationItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Kalkulacje",
                HrefArea = "PFEP",
                HrefAction = "Index",
                HrefController = "Calculation",
                Class1 = "fas fa-calculator",
                AccessCode = (int)MenuAccessCode.PFEP_PCKINSTR_ACCESS
            };
        }
        public static MenuItem LineFeedingItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "DEF Print",
                HrefArea = "PFEP",
                HrefAction = "Index",
                HrefController = "Print",
                Class1 = "fas fa-print",
                RequiredRole = DefRoles.PFEP_DEFPRINT_EDITOR,
                AccessCode = (int)MenuAccessCode.PFEP_LINEFEED_ACCESS
            };
        }
        public static MenuItem PackagesItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Opakowania",
                HrefArea = "PFEP",
                HrefAction = "Index",
                HrefController = "Package",
                Class1 = "fas fa-box-open",
                AccessCode = (int)MenuAccessCode.PFEP_ACCESS
            };
        }
        public static MenuItem AreasItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Obszary",
                HrefArea = "PFEP",
                HrefAction = "Index",
                HrefController = "Area",
                Class1 = "far fa-square",
                AccessCode = (int)MenuAccessCode.PFEP_ACCESS
            };
        }
    }
    //public static class MenuPFEP_Eldisy
    //{
    //    public static MenuItem MenuItem =
    //        new MenuItem
    //        {
    //            Id = 1,
    //            Name = "PFEP",
    //            HrefArea = "PFEP",
    //            HrefAction = "Browse",
    //            HrefController = "PackingInstruction",
    //            Class1 = "fas fa-dolly-flatbed",
    //            PictureName = "mdl_pfep.jpg",
    //            AccessCode = (int)MenuAccessCode.PFEP_ELDISY_ACCESS,
    //            Children = new MenuItem[] {
    //                MenuAP.HomeItem(),
    //                PackingInstrItem(),
    //                CalculationItem(),
    //                MenuPFEP.PackagesItem(),
    //                MenuPFEP.AreasItem(),
    //                MenuMPPL_Users.MenuItem,
    //            }
    //        };

    //    private static MenuItem PackingInstrItem()
    //    {
    //        return new MenuItem
    //        {
    //            Id = 1,
    //            Name = "Instr. Pak.",
    //            HrefArea = "PFEP",
    //            HrefAction = "Browse",
    //            HrefController = "PackingInstruction",
    //            Class1 = "far fa-file-alt",
    //            AccessCode = (int)MenuAccessCode.PFEP_ELDISY_ACCESS
    //        };
    //    }
    //    private static MenuItem CalculationItem()
    //    {
    //        return new MenuItem
    //        {
    //            Id = 1,
    //            Name = "Kalkulacje",
    //            HrefArea = "PFEP",
    //            HrefAction = "Index",
    //            HrefController = "Calculation",
    //            Class1 = "fas fa-calculator",
    //            AccessCode = (int)MenuAccessCode.PFEP_ELDISY_ACCESS
    //        };
    //    }
    //}
}

