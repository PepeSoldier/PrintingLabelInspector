using _LABELINSP_APPWEB.Areas._APPWEB.Models.Clients;
using MDL_BASE.Models;
using MDL_BASE.Models.IDENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace _LABELINSP_APPWEB.Areas._APPWEB.Models
{
    public enum MenuAccessCode
    {
        DEFAULT = 0,
        COMMON_ACCESS = 100100,
        FULL_ACCESS = 900900,
    }

    public static class AppMenu
    {
        public static AppClientApbstract appClient;

        public static MenuItem[] MenuItems = {
            MenuMPPL_Home.MenuItem,
            MenuCore.MenuItem,
            MenuMPPL_Users.MenuItem,
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

