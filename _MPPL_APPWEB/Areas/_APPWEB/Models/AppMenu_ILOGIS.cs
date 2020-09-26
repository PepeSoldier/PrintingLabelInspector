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
    public static class MenuiLOGIS
    {
        public static MenuItem MenuItem = BuildMenu();
        public static MenuItem MenuItemMobile = BuildMenu_Mobile();

        public static MenuItem BuildMenu()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "iLOGIS / PFEP",
                HrefArea = "iLOGIS",
                HrefAction = "Index",
                HrefController = "Home",
                Class1 = "fas fa-warehouse",
                PictureName = "mdl_iLOGIS.jpg",
                AccessCode = (int)MenuAccessCode.ILOGIS_ACCESS,
                //RequiredRole = DefRoles.Admin,
                Children = new MenuItem[] {
                    MenuMPPL_Home.MenuItem,
                    Dashboard(),
                    Config(),

                    AddDelivery(),
                    DeliveryBrowse(),
                    Stocks(),
                    Movement(),
                    MovementHistory(),
                    WhDocument(),
                    PickingStatus(),
                    //JobListItem(),
                    
                    MobileWMS(),
                    PickingList(),
                    DeliveryList(),
                    DEF_Print(),
                    TransporterLog(),
                    //DeliveryInspection(),
                }
            };
        }
        public static MenuItem BuildMenu_Mobile()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "iLOGIS / PFEP",
                HrefArea = "iLOGIS",
                HrefAction = "Index",
                HrefController = "Home",
                Class1 = "fas fa-warehouse",
                PictureName = "mdl_iLOGIS.jpg",
                AccessCode = (int)MenuAccessCode.ILOGIS_ACCESS,
                Category = "Mobile",
                //RequiredRole = DefRoles.Admin,
                Children = new MenuItem[] {
                    MenuMPPL_Home.MenuItem,
                    StocksMobile(),
                    MovementMobile(),
                    PickingList(),
                    DeliveryListLineFeed(),
                    //MovementHistory(),
                    DeliveryInspection(),
                    MobileSettings(),
                }
            };
        }

        //--------CATEGORY: GENERAL ----------------
        public static MenuItem Dashboard()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Dashboard",
                HrefArea = "iLOGIS",
                HrefAction = "Index",
                HrefController = "WMS",
                Class1 = "fas fa-warehouse",
                AccessCode = (int)MenuAccessCode.ILOGIS_DASHBOARD_ACCESS,
                Category = "General",
            };
        }
        public static MenuItem Config()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Konfiguracja",
                HrefArea = "iLOGIS",
                HrefAction = "Data",
                HrefController = "PFEP",
                Class1 = "far fa-square",
                AccessCode = (int)MenuAccessCode.ILOGIS_ACCESS,
                Category = "General",
            };
        }
        //--------CATEGORY: DESKTOP ----------------
        public static MenuItem AddDelivery()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Dodaj Dostawę",
                HrefArea = "iLOGIS",
                HrefAction = "Edit",
                HrefController = "Delivery",
                Class1 = "fas fa-plus-square",
                AccessCode = (int)MenuAccessCode.ILOGIS_WMS_ACCESS,
                Category = "Desktop",
            };
        }
        public static MenuItem DeliveryBrowse()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Przeglądanie dostaw",
                HrefArea = "iLOGIS",
                HrefAction = "Browse",
                HrefController = "Delivery",
                //Class1 = "fas fa-truck-loading",
                Class1 = "fas fa-truck-moving",
                AccessCode = (int)MenuAccessCode.ILOGIS_WMS_ACCESS,
                Category = "Desktop",
            };
        }
        public static MenuItem Stocks()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Zapasy",
                HrefArea = "iLOGIS",
                HrefAction = "Stock",
                HrefController = "StockUnit",
                Class1 = "fas fa-boxes",
                AccessCode = (int)MenuAccessCode.ILOGIS_WMS_ACCESS,
                Category = "Desktop",
            };
        }
        public static MenuItem Movement()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Przesunięcia magazynowe",
                HrefArea = "iLOGIS",
                HrefAction = "Movement",
                HrefController = "Movement",
                Class1 = "fas fa-people-carry",
                AccessCode = (int)MenuAccessCode.ILOGIS_WMS_ACCESS,
                Category = "Desktop",
            };
        }
        public static MenuItem MovementHistory()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Historia Przesunięć",
                HrefArea = "iLOGIS",
                HrefAction = "History",
                HrefController = "Movement",
                Class1 = "fas fa-book",
                AccessCode = (int)MenuAccessCode.ILOGIS_WMS_ACCESS,
                Category = "Desktop",
            };
        }
        public static MenuItem JobListItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "JobList Print",
                HrefArea = "iLOGIS",
                HrefAction = "Index",
                HrefController = "JobList",
                Class1 = "fas fa-print",
                AccessCode = (int)MenuAccessCode.ILOGIS_JOBLIST_ACCESS,
                Category = "Desktop",
            };
        }
        public static MenuItem WhDocument()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Dokumenty",
                HrefArea = "iLOGIS",
                HrefAction = "Browse",
                HrefController = "WhDoc",
                Class1 = "far fa-file-pdf",
                AccessCode = (int)MenuAccessCode.ILOGIS_WHDOC_ACCESS,
                Category = "Desktop",
            };
        }
        public static MenuItem PickingStatus()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Status Pickingu",
                HrefArea = "iLOGIS",
                HrefAction = "PickingStatus",
                HrefController = "PickingList",
                Class1 = "far fa-list-alt",
                AccessCode = (int)MenuAccessCode.ILOGIS_PICKLIST_ACCESS,
                Category = "Desktop",
            };
        }
        //--------CATEGORY: MOBILE -----------------
        public static MenuItem MobileWMS()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Mobile WMS",
                HrefArea = "iLOGIS",
                HrefAction = "MobileWMS",
                HrefController = "WMS",
                Class1 = "fas fa-mobile-alt",
                AccessCode = (int)MenuAccessCode.ILOGIS_WMS_ACCESS,
                Category = "Mobile",
            };
        }
        public static MenuItem StocksMobile()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Zapasy",
                HrefArea = "iLOGIS",
                HrefAction = "StockMobile",
                HrefController = "StockUnit",
                Class1 = "fas fa-boxes",
                AccessCode = (int)MenuAccessCode.ILOGIS_WMS_ACCESS,
                Category = "Mobile",
                //RequiredRole = "ILOGIS_OPERATOR_STOCK",
            };
        }
        public static MenuItem MovementMobile()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Przesunięcia",
                HrefArea = "iLOGIS",
                HrefAction = "MovementMobile",
                HrefController = "Movement",
                Class1 = "fas fa-people-carry",
                AccessCode = (int)MenuAccessCode.ILOGIS_WMS_ACCESS,
                Category = "Mobile",
                //RequiredRole = "ILOGIS_OPERATOR",
            };
        }
        public static MenuItem PickingList()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Picking",
                HrefArea = "iLOGIS",
                HrefAction = "TransporterList",
                HrefController = "PickingList",
                Class1 = "fas fa-cart-arrow-down",
                AccessCode = (int)MenuAccessCode.ILOGIS_PICKLIST_ACCESS,
                Category = "Mobile",
                //RequiredRole = "ILOGIS_OPERATOR_PICKING",
            };
        }
        public static MenuItem DeliveryListLineFeed()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Dowozy",
                HrefArea = "iLOGIS",
                HrefAction = "TransporterList",
                HrefController = "DeliveryListLineFeed",
                Class1 = "fas fa-dolly-flatbed",
                AccessCode = (int)MenuAccessCode.ILOGIS_PICKLIST_ACCESS,
                Category = "Mobile",
                //RequiredRole = "ILOGIS_OPERATOR_PICKING",
            };
        }
        public static MenuItem PickingList_direct_unused()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Przygotowanie",
                HrefArea = "iLOGIS",
                HrefAction = "MobileWMS",
                HrefController = "WMS",
                HashSuffix = "/iLOGIS/PickingList/TransporterList",
                Class1 = "fas fa-cart-arrow-down",
                AccessCode = (int)MenuAccessCode.ILOGIS_PICKLIST_ACCESS,
                Category = "Mobile",
            };
        }
        public static MenuItem DeliveryList()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Dostawy do Linii",
                HrefArea = "iLOGIS",
                HrefAction = "Index",
                HrefController = "DeliveryList",
                Class1 = "fas fa-dolly-flatbed",
                AccessCode = (int)MenuAccessCode.ILOGIS_DELIVERYLIST_ACCESS,
                Category = "Mobile",
            };
        }
        public static MenuItem DEF_Print()
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
                AccessCode = (int)MenuAccessCode.PFEP_LINEFEED_ACCESS,
                Category = "Mobile",
            };
        }
        public static MenuItem TransporterLog()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "TransporterLog",
                HrefArea = "iLOGIS",
                HrefAction = "TransporterLog",
                HrefController = "WMS",
                Class1 = "fas fa-history",
                AccessCode = (int)MenuAccessCode.ILOGIS_PICKLIST_ACCESS,
                Category = "Mobile",
            };
        }
        public static MenuItem DeliveryInspection()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Inspekcja dostaw",
                HrefArea = "iLOGIS",
                HrefAction = "DeliveryInspection",
                HrefController = "Delivery",
                Class1 = "fas fa-barcode",
                AccessCode = (int)MenuAccessCode.ILOGIS_WMS_ACCESS,
                Category = "Mobile",
                //RequiredRole = "ILOGIS_OPERATOR_INCOMING",
            };
        }
        public static MenuItem MobileSettings()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Ustawienia",
                HrefArea = "iLOGIS",
                HrefAction = "MobileSettings",
                HrefController = "WMS",
                Class1 = "fas fa-cogs",
                AccessCode = (int)MenuAccessCode.ILOGIS_WMS_ACCESS,
                Category = "Mobile",
            };
        }
    }
    public static class MenuiLOGISConfig
    {
        public static MenuItem MenuItem = BuildMenu();
        public static MenuItem BuildMenu()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Konfiguracja",
                NameEnglish = "Configuration",
                HrefArea = "MasterData",
                HrefAction = "Workstation",
                HrefController = "Workstation",
                Class1 = "fa fa-street-view",
                AccessCode = (int)MenuAccessCode.ILOGIS_ACCESS,
                //RequiredRole = DefRoles.Admin,
                Children = new MenuItem[] {
                    Resources(),
                    Workstations(),
                    Items(),
                    Packages(),
                    PackageItem(),
                    WorkstationItem(),
                    AutomaticRule(),
                    AutomaticRulePackage(),
                    Transporter(),
                    Warehouse(),
                    WarehouseLocation(),
                    WarehouseLocationType(),
                    WarehouseLocationSort(),
                    PFEPData(),
                    ClientsItem(),
                    ItemCopy()
                }
            };
        }

        public static MenuItem Resources()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Zasoby",
                NameEnglish = "Resources",
                HrefArea = "MasterData",
                HrefAction = "Resource",
                HrefController = "Resource",
                Class1 = "fa fa-microchip",
                AccessCode = (int)MenuAccessCode.ILOGIS_CONFIG_RESOURCE,
            };
        }
        public static MenuItem Workstations()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Stanowiska",
                NameEnglish = "Workstations",
                HrefArea = "MasterData",
                HrefAction = "Workstation",
                HrefController = "Workstation",
                Class1 = "fa fa-street-view",
                AccessCode = (int)MenuAccessCode.ILOGIS_CONFIG_WORKSTATIONS,
            };
        }
        public static MenuItem Items()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Artykuły",
                NameEnglish = "Items",
                HrefArea = "iLOGIS",
                HrefAction = "Item",
                HrefController = "Config",
                Class1 = "fa fa-puzzle-piece",
                AccessCode = (int)MenuAccessCode.ILOGIS_CONFIG_ITEMS,
            };
        }
        public static MenuItem Packages()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Opakowania",
                NameEnglish = "Packages",
                HrefArea = "iLOGIS",
                HrefAction = "Package",
                HrefController = "Config",
                Class1 = "fa fa-cubes",
                AccessCode = (int)MenuAccessCode.ILOGIS_CONFIG_PACKAGES,
            };
        }
        public static MenuItem PackageItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Opakowanie-Artykuł",
                NameEnglish = "Package-Item",
                HrefArea = "iLOGIS",
                HrefAction = "PackageItem",
                HrefController = "Config",
                Class1 = "fas fa-link",
                AccessCode = (int)MenuAccessCode.ILOGIS_CONFIG_PACKAGEITEM,
            };
        }
        public static MenuItem WorkstationItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Stanowsko-Artykuł",
                NameEnglish = "Workstation-Item",
                HrefArea = "iLOGIS",
                HrefAction = "WorkstationItem",
                HrefController = "Config",
                Class1 = "fas fa-link",
                AccessCode = (int)MenuAccessCode.ILOGIS_CONFIG_WORKSTATIONITEM,
            };
        }
        public static MenuItem Transporter()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Transporter",
                NameEnglish = "Transporter",
                HrefArea = "iLOGIS",
                HrefAction = "Transporter",
                HrefController = "Config",
                Class1 = "fas fa-dolly",
                AccessCode = (int)MenuAccessCode.ILOGIS_CONFIG_TRANSPORTER,
            };
        }
        public static MenuItem AutomaticRule()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Reguły Automatyczne",
                NameEnglish = "Automatic Rules",
                HrefArea = "iLOGIS",
                HrefAction = "AutomaticRule",
                HrefController = "Config",
                Class1 = "fas fa-magic",
                AccessCode = (int)MenuAccessCode.ILOGIS_CONFIG_AUTOMATICRULE,
            };
        }
        public static MenuItem AutomaticRulePackage()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Reguły Automatyczne Op.",
                NameEnglish = "Automatic Rules Pk.",
                HrefArea = "iLOGIS",
                HrefAction = "AutomaticRulePackage",
                HrefController = "Config",
                Class1 = "fas fa-magic",
                AccessCode = (int)MenuAccessCode.ILOGIS_CONFIG_AUTOMATICRULEPACKAGE,
            };
        }
        public static MenuItem PFEPData()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "PFEP Dane",
                NameEnglish = "PFEP Data",
                HrefArea = "iLOGIS",
                HrefAction = "Data",
                HrefController = "PFEP",
                Class1 = "fab fa-linode",
                AccessCode = (int)MenuAccessCode.ILOGIS_CONFIG_PFEPDATA,
            };
        }
        public static MenuItem Warehouse()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Magazyny",
                NameEnglish = "Warehouses",
                HrefArea = "iLOGIS",
                HrefAction = "Warehouse",
                HrefController = "Config",
                Class1 = "fas fa-warehouse",
                AccessCode = (int)MenuAccessCode.ILOGIS_CONFIG_WAREHOUSE,
            };
        }
        public static MenuItem WarehouseLocation()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Lokacje",
                NameEnglish = "Locations",
                HrefArea = "iLOGIS",
                HrefAction = "WarehouseLocation",
                HrefController = "Config",
                Class1 = "fas fa-pallet",
                AccessCode = (int)MenuAccessCode.ILOGIS_CONFIG_WAREHOUSELOCATION,
            };
        }
        public static MenuItem WarehouseLocationType()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Typ Lokacji",
                NameEnglish = "Location Type",
                HrefArea = "iLOGIS",
                HrefAction = "WarehouseLocationType",
                HrefController = "Config",
                Class1 = "fas fa-shapes",
                AccessCode = (int)MenuAccessCode.ILOGIS_CONFIG_WAREHOUSELOCATIONTYPE,
            };
        }
        public static MenuItem WarehouseLocationSort()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "WLSort",
                NameEnglish = "WLSort",
                HrefArea = "iLOGIS",
                HrefAction = "WarehouseLocationSort",
                HrefController = "Config",
                Class1 = "fas fa-dolly",
                AccessCode = (int)MenuAccessCode.ILOGIS_CONFIG_WAREHOUSELOCATIONSORT,
            };
        }
        private static MenuItem ClientsItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Kontrahenci",
                NameEnglish = "(Contractors)",
                HrefArea = "MASTERDATA",
                HrefAction = "Contractor",
                HrefController = "Contractor",
                Class1 = "fas fa-handshake",
                AccessCode = (int)MenuAccessCode.ILOGIS_CONFIG_CONTRACTORS
            };
        }
        public static MenuItem ItemCopy()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Kopiuj Param.",
                NameEnglish = "Copy Params",
                HrefArea = "#",
                HrefAction = "",
                HrefController = "",
                Class1 = "far fa-clone",
                ElementId = "btnMenuCopyItemSettings",
                AccessCode = (int)MenuAccessCode.ILOGIS_CONFIG_ITEMCOPY,
            };
        }
        public static MenuItem PrintLabels()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Druk Etykiet",
                NameEnglish = "Print Labels",
                HrefArea = "iLOGIS",
                HrefAction = "PrintLabels",
                HrefController = "Config",
                Class1 = "far fa-print",
                ElementId = "btnPrintLabels",
                AccessCode = (int)MenuAccessCode.ILOGIS_CONFIG_ITEMCOPY,
            };
        }
    }
}

