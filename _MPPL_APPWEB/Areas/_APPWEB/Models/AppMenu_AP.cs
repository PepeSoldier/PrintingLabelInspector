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
    public static class MenuAP
    {
        public static MenuItem MenuItem = BuildMenu();
        private static MenuItem BuildMenu()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "ActionPlans",
                HrefArea = "AP",
                HrefAction = "Index",
                HrefController = "Start",
                Class1 = "fas fa-columns",
                PictureName = "mdl_ap.jpg",
                AccessCode = (int)MenuAccessCode.AP_ACCESS,
                RequiredRole = DefRoles.ADMIN,
                Children = new MenuItem[]
                {
                    MenuMPPL_Home.MenuItem,
                    HomeItem(),
                    AddEditItem(),
                    BrowseItem(),
                    MeetingsItem(),
                    GanttChartItem(),
                }
            };
        }

        public static MenuItem HomeItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Home",
                HrefArea = "AP",
                HrefAction = "Index",
                HrefController = "Start",
                Class1 = "glyphicon glyphicon-home"
            };
        }
        private static MenuItem AddEditItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Dodaj",
                HrefArea = "AP",
                HrefAction = "Edit",
                HrefController = "Action",
                Class1 = "glyphicon glyphicon-edit"
            };
        }
        private static MenuItem BrowseItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Przeglądaj",
                HrefArea = "AP",
                HrefAction = "Browsenew",
                HrefController = "Action",
                Class1 = "glyphicon glyphicon-list"
            };
        }
        private static MenuItem MeetingsItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Spotkania",
                HrefArea = "AP",
                HrefAction = "Browse",
                HrefController = "Meeting",
                Class1 = "glyphicon glyphicon-refresh",
                AccessCode = (int)MenuAccessCode.AP_MEETING_ACCESS
            };
        }
        private static MenuItem GanttChartItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Gantt Chart",
                HrefArea = "AP",
                HrefAction = "Show",
                HrefController = "GanttChart",
                Class1 = "glyphicon glyphicon-indent-left"
            };
        }
    }
}

