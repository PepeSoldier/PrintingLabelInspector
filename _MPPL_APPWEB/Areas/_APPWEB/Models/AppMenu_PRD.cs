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
    public static class MenuPRD
    {
        public static MenuItem MenuItem = BuildMenu();
        private static MenuItem BuildMenu()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "PRD",
                HrefArea = "PRD",
                HrefAction = "Index",
                HrefController = "Home",
                Class1 = "fas fa-industry",
                PictureName = "mdl_prd.jpg",
                AccessCode = (int)MenuAccessCode.PRD_ACCESS,
                Children = new MenuItem[]
                {
                    MenuMPPL_Home.MenuItem,
                    PlanItem(),
                    PSIAnalyzeItem(),
                    PSIResultsItem(),
                }
            };
        }

        private static MenuItem PSIResultsItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "PSI - Wyniki",
                HrefArea = "PRD",
                HrefAction = "Results",
                HrefController = "PSI",
                Class1 = "fas fa-chart-pie"
            };
        }
        private static MenuItem PSIAnalyzeItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "PSI - Analiza",
                HrefArea = "PRD",
                HrefAction = "Analyze",
                HrefController = "PSI",
                Class1 = "fas fa-search"
            };
        }
        private static MenuItem PlanItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Plan",
                HrefArea = "PRD",
                HrefAction = "Index",
                HrefController = "Schedule",
                Class1 = "fas fa-th-list"
            };
        }
    }
}

