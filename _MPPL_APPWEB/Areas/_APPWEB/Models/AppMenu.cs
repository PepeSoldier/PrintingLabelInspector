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
    public enum MenuAccessCode
    {
        DEFAULT = 0,
        COMMON_ACCESS = 100100,
        FULL_ACCESS = 900900,
        AP_ACCESS = 100200,
        AP_MEETING_ACCESS = 100200,
        PFEP_ACCESS = 200100,
        PFEP_PCKINSTR_ACCESS = 200200,
        PFEP_LINEFEED_ACCESS = 200300,
        PRD_ACCESS = 300100,
        ONEPROD_ACCESS = 400100,
        ONEPROD_APS_ACCESS = 400120,
        ONEPROD_WMS_ACCESS = 400140,
        ONEPROD_MES_ACCESS = 400180,
        ONEPROD_MES_TRACE_ACCESS = 400185,
        ONEPROD_OEE_ACCESS = 400200,
        ONEPROD_OEE_DASHBOARD_ACCESS = 400220,
        ONEPROD_OEE_REPORTS_ACCESS = 400240,
        ONEPROD_OEE_COMPARE_ACCESS = 400250,
        ONEPROD_OEE_ENERGY_ACCESS = 400260,
        ONEPROD_RTV_ACCESS = 400300,
        ONEPROD_JOBLIST_ACCESS = 400400,
        ONEPROD_QUALITY_ACCESS = 400500,
        OTHER_HSE = 500100,
        ILOGIS_ACCESS = 600100,
        ILOGIS_DASHBOARD_ACCESS = 600150,
        ILOGIS_PFEP_ACCESS = 600200,
        ILOGIS_WMS_ACCESS = 600300,
        ILOGIS_PICKLIST_ACCESS = 600400,
        ILOGIS_DELIVERYLIST_ACCESS = 600450,
        ILOGIS_JOBLIST_ACCESS = 600500,
        ILOGIS_WHDOC_ACCESS = 600600,
        ILOGIS_CONFIG = 607000,
        ILOGIS_CONFIG_RESOURCE = 607010,
        ILOGIS_CONFIG_WORKSTATIONS = 607020,
        ILOGIS_CONFIG_ITEMS = 607030,
        ILOGIS_CONFIG_PACKAGES = 607040,
        ILOGIS_CONFIG_PACKAGEITEM= 607050,
        ILOGIS_CONFIG_WORKSTATIONITEM = 607060,
        ILOGIS_CONFIG_TRANSPORTER = 607070,
        ILOGIS_CONFIG_AUTOMATICRULE = 607080,
        ILOGIS_CONFIG_AUTOMATICRULEPACKAGE = 607090,
        ILOGIS_CONFIG_PFEPDATA = 607100,
        ILOGIS_CONFIG_WAREHOUSE = 607110,
        ILOGIS_CONFIG_WAREHOUSELOCATION = 607120,
        ILOGIS_CONFIG_WAREHOUSELOCATIONTYPE = 607130,
        ILOGIS_CONFIG_WAREHOUSELOCATIONSORT = 607140,
        ILOGIS_CONFIG_ITEMCOPY = 607150,
        ILOGIS_CONFIG_CONTRACTORS = 607160,
    }

    public static class AppMenu
    {
        public static AppClientApbstract appClient;

        public static MenuItem[] MenuItems = {
            MenuMPPL_Home.MenuItem,
            MenuPRD.MenuItem,
            MenuAP.MenuItem,
            MenuiLOGIS.MenuItem,
            MenuONEPROD.MenuItem,
            MenuONEPRODoee.MenuItem,
            MenuOTHER.MenuItem,
            MenuCore.MenuItem,
            MenuMPPL_Users.MenuItem,
            //MenuPFEP.MenuItem,
            //MenuMPPL_Test.MenuItem
            //MenuPFEP_Eldisy.MenuItem,
        };

        public static bool CheckAccess(IMenuItem menuItem, AppClientApbstract appClient1)
        {
            return appClient1.hasAccess((MenuAccessCode)menuItem.AccessCode);
        }
        public static bool CheckAccess(IMenuItem menuItem, AppClientApbstract appClient1, IPrincipal user)
        {
            return appClient1.hasAccess((MenuAccessCode)menuItem.AccessCode) 
                    && HasUserAccess(user, menuItem.RequiredRole);
        }
        public static bool HasClientAccess(string clientName, MenuAccessCode accessCode)
        {
            appClient = AppClient.GetClient(clientName);

            if (appClient.hasAccess(accessCode))
                return true;
            else
                return false;
        }
        public static bool HasUserAccess(System.Security.Principal.IPrincipal user, string requiredRole)
        {
            if( (requiredRole == null || 
                (requiredRole != null && user.Identity.IsAuthenticated && user.IsInRole(requiredRole))))
                return true;
            else
                return false;
        }
    }

    public static class MenuMPPL_Home
    {
        public static MenuItem MenuItem = new MenuItem
        {
            Id = 1, Name = "Home",
            HrefArea = "", HrefAction = "Index", HrefController = "Home",
            Class1 = "fas fa-home"
        };
    }
    public static class MenuMPPL_Users
    {
        public static MenuItem MenuItem = MenuMPPL_UsersItem();//MyProfileItem();

        private static MenuItem MenuMPPL_UsersItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Użytkownicy",
                HrefArea = "IDENTITY",
                HrefAction = "Browse",
                HrefController = "Account",
                Class1 = "fas fa-users",
                PictureName = "mdlUsers.png",
                RequiredRole = DefRoles.ADMIN,
                Children = new MenuItem[] {
                    MenuMPPL_Home.MenuItem,
                    UsersListItem(),
                    MyProfileItem(),
                    RegisterUser(),
                    RegisterOperator()
                }
            };
        }
        private static MenuItem MyProfileItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Mój profil",
                HrefArea = "IDENTITY",
                HrefAction = "Show",
                HrefController = "Account",
                Class1 = "fas fa-user",
            };
        }
        private static MenuItem RegisterUser()
        {
            return new MenuItem
            {
                Id = 2,
                Name = "Stwórz Użytkownika",
                HrefArea = "IDENTITY",
                HrefAction = "Register",
                HrefController = "Account",
                Class1 = "fas fa-user",
            };
        }
        private static MenuItem RegisterOperator()
        {
            return new MenuItem
            {
                Id = 2,
                Name = "Stwórz Operatora",
                HrefArea = "IDENTITY",
                HrefAction = "RegisterOperator",
                HrefController = "Account",
                Class1 = "fas fa-street-view",
            };
        }
        private static MenuItem UsersListItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Lista użytkowników",
                HrefArea = "IDENTITY",
                HrefAction = "Browse",
                HrefController = "Account",
                Class1 = "fas fa-users",
            };
        }
    }
    public static class MenuMPPL_Test
    {
        public static MenuItem MenuItem = MenuMPPL_TestItem();

        private static MenuItem MenuMPPL_TestItem()
        {
            return new MenuItem
        {
            Id = 1,
            Name = "Tests",
            HrefArea = "",
            HrefAction = "AppTest",
            HrefController = "Test",
            Class1 = "glyphicon glyphicon-play",
            RequiredRole = DefRoles.ADMIN,
            Children = new MenuItem[]
            {
                AppTest1Item(),
                GridTestItem()
            }
        };
        }
        private static MenuItem GridTestItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Grid Test",
                HrefArea = "",
                HrefAction = "GridTest",
                HrefController = "Test",
                Class1 = "glyphicon glyphicon-play",
            };
        }
        private static MenuItem AppTest1Item()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "App Tests 1",
                HrefArea = "",
                HrefAction = "AppTest",
                HrefController = "Test",
                Class1 = "glyphicon glyphicon-play",
            };
        }
    }
}

