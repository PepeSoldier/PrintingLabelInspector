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
    public static class MenuONEPROD
    {
        public static MenuItem MenuItem = BuildMenu();
        private static MenuItem BuildMenu()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "ONEPROD",
                HrefArea = "ONEPROD",
                HrefAction = "Index",
                HrefController = "Home",
                Class1 = "fab fa-connectdevelop",
                PictureName = "mdl_oneprod.jpg",
                AccessCode = (int)MenuAccessCode.ONEPROD_ACCESS,
                Children = new MenuItem[] {
                    MenuMPPL_Home.MenuItem,
                    CalculationItem(),
                    GanttChartItem(),
                    PlanItem(),
                    ClientOrdersItem(),
                    //MonitorItem(),
                    OperatorScreenItem(),
                    StockItem(),
                    BufferItem(),
                    CalendarItem(),
                    TracebilityItem(),
                    OEEReportsItem(),
                    MenuONEPRODoee.MenuItem, //OEEAnalysisItem(),
                    RTVItem(),
                    JobListItem(),
                    ConfigItem(),
                    EnergyItem(),
                    BOMItem(),
                    QualityItem()
                }
            };
        }

        private static MenuItem CalculationItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Algorytm",
                HrefArea = "ONEPROD",
                HrefAction = "Calculation",
                HrefController = "APS",
                Class1 = "far fa-play-circle",
                AccessCode = (int)MenuAccessCode.ONEPROD_APS_ACCESS,
                RequiredRole = DefRoles.ONEPROD_VIEWER
            };
        }
        private static MenuItem GanttChartItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Wykres Gantta",
                HrefArea = "ONEPROD",
                HrefAction = "GanttChart",
                HrefController = "APS",
                Class1 = "fas fa-align-left",
                AccessCode = (int)MenuAccessCode.ONEPROD_APS_ACCESS,
                RequiredRole = DefRoles.ONEPROD_VIEWER
            };
        }
        private static MenuItem PlanItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Plan",
                HrefArea = "ONEPROD",
                HrefAction = "Plan",
                HrefController = "APS",
                Class1 = "far fa-file-alt",
                AccessCode = (int)MenuAccessCode.ONEPROD_APS_ACCESS
            };
        }
        private static MenuItem ClientOrdersItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Zamówienia Klienta",
                HrefArea = "ONEPROD",
                HrefAction = "Index",
                HrefController = "ClientOrder",
                Class1 = "fas fa-list-alt",
                AccessCode = (int)MenuAccessCode.ONEPROD_ACCESS,
                RequiredRole = DefRoles.ONEPROD_VIEWER
            };
        }
        private static MenuItem MonitorItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Monitor",
                HrefArea = "ONEPROD",
                HrefAction = "PlanMonitor",
                HrefController = "MES",
                Class1 = "fas fa-tablet-alt",
                AccessCode = (int)MenuAccessCode.ONEPROD_APS_ACCESS
            };
        }
        private static MenuItem OperatorScreenItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Ekran Operatora",
                HrefArea = "ONEPROD",
                HrefAction = "Workplace",
                HrefController = "MES",
                Class1 = "fas fa-tablet-alt",
                AccessCode = (int)MenuAccessCode.ONEPROD_MES_ACCESS,
                RequiredRole = DefRoles.ONEPROD_MES_OPERATOR
            };
        }
        private static MenuItem StockItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Stok",
                HrefArea = "ONEPROD",
                HrefAction = "Stock",
                HrefController = "WMS",
                Class1 = "fas fa-boxes",
                AccessCode = (int)MenuAccessCode.ONEPROD_APS_ACCESS,
                RequiredRole = DefRoles.ONEPROD_VIEWER
            };
        }
        private static MenuItem BufferItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Bufory",
                HrefArea = "ONEPROD",
                HrefAction = "StockMonitor",
                HrefController = "WMS",
                Class1 = "fas fa-calculator",
                AccessCode = (int)MenuAccessCode.ONEPROD_APS_ACCESS
            };
        }
        private static MenuItem BufferNewItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Bufory Nowe",
                HrefArea = "ONEPROD",
                HrefAction = "WIP",
                HrefController = "MES",
                Class1 = "fas fa-calculator",
                AccessCode = (int)MenuAccessCode.ONEPROD_APS_ACCESS,
                RequiredRole = DefRoles.ADMIN,
            };
        }
        private static MenuItem CalendarItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Kalendarz",
                HrefArea = "ONEPROD",
                HrefAction = "Calendar",
                HrefController = "APS",
                Class1 = "far fa-calendar-alt",
                AccessCode = (int)MenuAccessCode.ONEPROD_APS_ACCESS,
                RequiredRole = DefRoles.ONEPROD_VIEWER
            };
        }
        private static MenuItem TracebilityItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Traceability",
                HrefArea = "ONEPROD",
                HrefAction = "Traceability",
                HrefController = "MES",
                Class1 = "fas fa-tasks",
                AccessCode = (int)MenuAccessCode.ONEPROD_MES_TRACE_ACCESS
            };
        }
        private static MenuItem RTVItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "RTV",
                HrefArea = "ONEPROD",
                HrefAction = "Index",
                HrefController = "RTV",
                Class1 = "fas fa-heartbeat",
                AccessCode = (int)MenuAccessCode.ONEPROD_RTV_ACCESS
            };
        }
        private static MenuItem OEEAnalysisItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "OEE",
                HrefArea = "ONEPROD",
                HrefAction = "Index",
                HrefController = "OEE",
                Class1 = "fab fa-cloudscale",
                AccessCode = (int)MenuAccessCode.ONEPROD_OEE_DASHBOARD_ACCESS
            };
        }
        private static MenuItem OEEReportsItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Raporty Produkcyjne",
                HrefArea = "ONEPROD",
                HrefAction = "Reports",
                HrefController = "OEEBrowse",
                Class1 = "fas fa-file-alt",
                AccessCode = (int)MenuAccessCode.ONEPROD_OEE_REPORTS_ACCESS
            };
        }
        private static MenuItem JobListItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "JobList",
                HrefArea = "ONEPROD",
                HrefAction = "JobList",
                HrefController = "JobList",
                Class1 = "fas fa-digital-tachograph",
                AccessCode = (int)MenuAccessCode.ONEPROD_JOBLIST_ACCESS
            };
        }
        private static MenuItem ConfigItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Konfiguracja",
                HrefArea = "ONEPROD",
                HrefAction = "Resource",
                HrefController = "Configuration",
                Class1 = "fas fa-cogs",
                RequiredRole = DefRoles.ONEPROD_VIEWER
            };
        }
        private static MenuItem BOMItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "BOM",
                HrefArea = "ONEPROD",
                HrefAction = "BOM",
                HrefController = "BOM",
                Class1 = "far fa-list-alt",
                RequiredRole = DefRoles.USER
            };
        }
        private static MenuItem EnergyItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Media",
                HrefArea = "ONEPROD",
                HrefAction = "EnergyMeter",
                HrefController = "ConfigurationENERGY",
                Class1 = "fas fa-industry",
                RequiredRole = DefRoles.ONEPROD_VIEWER,
                AccessCode = (int)MenuAccessCode.ONEPROD_OEE_ENERGY_ACCESS
            };
        }
        private static MenuItem QualityItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Quality",
                HrefArea = "ONEPROD",
                HrefAction = "ItemMeasurement",
                HrefController = "Quality",
                Class1 = "fas fa-ruler-combined",
                AccessCode = (int)MenuAccessCode.ONEPROD_QUALITY_ACCESS,
                RequiredRole = DefRoles.ONEPROD_VIEWER
            };
        }
        
    }
    public static class MenuONEPRODoee
    {
        public static MenuItem MenuItem = BuildMenu();
        private static MenuItem BuildMenu()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "OEE",
                HrefArea = "ONEPROD",
                HrefAction = "Index",
                HrefController = "OEE",
                Class1 = "fab fa-cloudscale",
                PictureName = "mdl_oee.jpg",
                AccessCode = (int)MenuAccessCode.ONEPROD_OEE_DASHBOARD_ACCESS,
                Children = new MenuItem[] {
                    MenuMPPL_Home.MenuItem,
                    Dashboard(),
                    CreateReport(),
                    BrowseReports(),
                    BrowseOEEData(),
                    CompareResults(),
                    ConfigItem()
                }
            };
        }

        private static MenuItem Dashboard()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Dashboard",
                HrefArea = "ONEPROD",
                HrefAction = "Index",
                HrefController = "OEE",
                Class1 = "fas fa-tachometer-alt",
                AccessCode = (int)MenuAccessCode.ONEPROD_OEE_DASHBOARD_ACCESS
            };
        }
        private static MenuItem CompareResults()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Porównaj Wyniki",
                HrefArea = "ONEPROD",
                HrefAction = "CompareResults",
                HrefController = "OEE",
                Class1 = "fas fa-balance-scale-right",
                AccessCode = (int)MenuAccessCode.ONEPROD_OEE_COMPARE_ACCESS
            };
        }
        private static MenuItem BrowseOEEData()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Dane OEE",
                HrefArea = "ONEPROD",
                HrefAction = "OEEData",
                HrefController = "OEEBrowse",
                Class1 = "fas fa-table",
                AccessCode = (int)MenuAccessCode.ONEPROD_OEE_DASHBOARD_ACCESS
            };
        }
        private static MenuItem CreateReport()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Stwórz Raport",
                HrefArea = "ONEPROD",
                HrefAction = "Index",
                HrefController = "OEECreateReport",
                Class1 = "far fa-play-circle",
                AccessCode = (int)MenuAccessCode.ONEPROD_OEE_DASHBOARD_ACCESS
            };
        }
        private static MenuItem BrowseReports()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Przeglądaj Raporty",
                HrefArea = "ONEPROD",
                HrefAction = "Index",
                HrefController = "OEE",
                Class1 = "fas fa-th-list",
                HashSuffix = "browse",
                AccessCode = (int)MenuAccessCode.ONEPROD_OEE_DASHBOARD_ACCESS
            };
        }
        private static MenuItem ConfigItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Konfiguracja",
                HrefArea = "ONEPROD",
                HrefAction = "Reason",
                HrefController = "ConfigurationOEE",
                Class1 = "fas fa-cogs",
                AccessCode = (int)MenuAccessCode.ONEPROD_OEE_DASHBOARD_ACCESS
            };
        }
    }
    public static class MenuONEPRODConfig
    {
        public static MenuItem MenuItem = BuildMenu();
        private static MenuItem BuildMenu()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Config",
                HrefArea = "ONEPROD",
                HrefAction = "Resource",
                HrefController = "Configuration",
                Class1 = "fas fa-cogs",
                PictureName = "mdl_oneprod.jpg",
                AccessCode = (int)MenuAccessCode.ONEPROD_ACCESS,
                Children = new MenuItem[] {
                    ResourceGroupItem(),
                    ResourceItem(),
                    WorkplaceItem(),
                    ProcessItem(),
                    ItemGroupItem(),
                    ItemItem(),
                    CycleTimeItem(),
                    WarehouseItem(),
                    ToolItem(),
                    ToolMachineItem(),
                    ToolMatrixItem(),
                    ChangeOverItem(),
                    ReasonItem(),
                    QualityConfigItem()
                }
            };
        }

        private static MenuItem ResourceGroupItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Grupy Maszyn",
                NameEnglish = "(Machine Groups)",
                HrefArea = "ONEPROD",
                HrefAction = "ResourceGroup",
                HrefController = "Configuration",
                Class1 = "fa fa-clone",
                AccessCode = (int)MenuAccessCode.ONEPROD_ACCESS
            };
        }
        private static MenuItem ResourceItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Zasoby",
                NameEnglish = "(Resources)",
                HrefArea = "ONEPROD",
                HrefAction = "Resource",
                HrefController = "Configuration",
                Class1 = "fa fa-microchip",
                AccessCode = (int)MenuAccessCode.ONEPROD_ACCESS
            };
        }
        private static MenuItem WorkplaceItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Stanowiska",
                NameEnglish = "(Workplaces)",
                HrefArea = "ONEPROD",
                HrefAction = "Workplace",
                HrefController = "ConfigurationMES",
                Class1 = "fa fa-street-view",
                AccessCode = (int)MenuAccessCode.ONEPROD_MES_ACCESS
            };
        }
        private static MenuItem ProcessItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Procesy",
                NameEnglish = "(Processes)",
                HrefArea = "ONEPROD",
                HrefAction = "Process",
                HrefController = "Configuration",
                Class1 = "fa fa-tags",
                AccessCode = (int)MenuAccessCode.ONEPROD_ACCESS
            };
        }
        private static MenuItem ItemGroupItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Grupy Pozycji",
                NameEnglish = "(Item Groups)",
                HrefArea = "ONEPROD",
                HrefAction = "ItemGroup",
                HrefController = "Configuration",
                Class1 = "fa fa-tag",
                AccessCode = (int)MenuAccessCode.ONEPROD_ACCESS
            };
        }
        private static MenuItem ItemItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Pozycje",
                NameEnglish = "(Items)",
                HrefArea = "ONEPROD",
                HrefAction = "Item",
                HrefController = "Configuration",
                Class1 = "fa fa-puzzle-piece",
                AccessCode = (int)MenuAccessCode.ONEPROD_ACCESS
            };
        }
        private static MenuItem CycleTimeItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Czasy cykli",
                NameEnglish = "(Cycle Times)",
                HrefArea = "ONEPROD",
                HrefAction = "CycleTime",
                HrefController = "Configuration",
                Class1 = "fa fa-clock",
                AccessCode = (int)MenuAccessCode.ONEPROD_ACCESS
            };
        }
        private static MenuItem WarehouseItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Magazyny",
                NameEnglish = "(Warehouses)",
                HrefArea = "ONEPROD",
                HrefAction = "Box",
                HrefController = "ConfigurationWMS",
                Class1 = "fa fa-cubes",
                AccessCode = (int)MenuAccessCode.ONEPROD_WMS_ACCESS
            };
        }
        private static MenuItem ToolItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Narzędzia",
                NameEnglish = "(Tools)",
                HrefArea = "ONEPROD",
                HrefAction = "ResourceGroup",
                HrefController = "ConfigurationAPS",
                Class1 = "fa fa-wrench",
                AccessCode = (int)MenuAccessCode.ONEPROD_APS_ACCESS
            };
        }
        private static MenuItem ToolMachineItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Narzędzia-Maszyny",
                NameEnglish = "(Tools-Machines)",
                HrefArea = "ONEPROD",
                HrefAction = "ToolMachine",
                HrefController = "ConfigurationAPS",
                Class1 = "fa fa-stack fa-2x",
                Class2 = "fas fa-microchip fa-stack-2x",
                Class3 = "fas fa fa-wrench fa-stack-1x",
                AccessCode = (int)MenuAccessCode.ONEPROD_APS_ACCESS
            };
        }
        private static MenuItem ToolMatrixItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Matryca Przezbrojeń",
                NameEnglish = "(Tool Matrix)",
                HrefArea = "ONEPROD",
                HrefAction = "ToolMatrix",
                HrefController = "ConfigurationAPS",
                Class1 = "fa fa-table",
                AccessCode = (int)MenuAccessCode.ONEPROD_APS_ACCESS
            };
        }
        private static MenuItem ChangeOverItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Przezbrojenia",
                NameEnglish = "(Change Overs)",
                HrefArea = "ONEPROD",
                HrefAction = "ChangeOver",
                HrefController = "ConfigurationAPS",
                Class1 = "fa fa-random",
                AccessCode = (int)MenuAccessCode.ONEPROD_APS_ACCESS
            };
        }
        private static MenuItem ReasonItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Powody",
                NameEnglish = "(Reasons)",
                HrefArea = "ONEPROD",
                HrefAction = "Reason",
                HrefController = "ConfigurationOEE",
                Class1 = "fas fa-caret-down",
                AccessCode = (int)MenuAccessCode.ONEPROD_OEE_ACCESS
            };
        }
        private static MenuItem QualityConfigItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Quality Ustawienia",
                HrefArea = "ONEPROD",
                HrefAction = "ItemParameter",
                HrefController = "Quality",
                Class1 = "fas fa-sliders-h",
                AccessCode = (int)MenuAccessCode.ONEPROD_QUALITY_ACCESS,
                RequiredRole = DefRoles.ONEPROD_ADMIN
            };
        }

    }
    public static class MenuONEPRODEnergyConfig
    {
        public static MenuItem MenuItem = BuildMenu();
        private static MenuItem BuildMenu()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Media",
                HrefArea = "ONEPROD",
                HrefAction = "EnergyMeter",
                HrefController = "ConfigurationENERGY",
                Class1 = "fas fa-cogs",
                PictureName = "mdl_oneprod.jpg",
                AccessCode = (int)MenuAccessCode.ONEPROD_OEE_ENERGY_ACCESS,
                Children = new MenuItem[] {
                    EnergyItem(),
                    EnergySettlement(),
                    EnergyConsumption()
                }
            };
        }

        private static MenuItem EnergyItem()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Energy Meter",
                HrefArea = "ONEPROD",
                HrefAction = "EnergyMeter",
                HrefController = "ConfigurationENERGY",
                Class1 = "fas fa-shapes",
                RequiredRole = DefRoles.ONEPROD_VIEWER
            };
        }
        private static MenuItem EnergySettlement()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Energy Cost",
                HrefArea = "ONEPROD",
                HrefAction = "EnergyCost",
                HrefController = "ConfigurationENERGY",
                Class1 = "fas fa-money-check-alt",
                RequiredRole = DefRoles.ONEPROD_VIEWER
            };
        }
        private static MenuItem EnergyConsumption()
        {
            return new MenuItem
            {
                Id = 1,
                Name = "Energy Consumption",
                HrefArea = "ONEPROD",
                HrefAction = "EnergyConsumption",
                HrefController = "ConfigurationENERGY",
                Class1 = "fas fa-donate",
                RequiredRole = DefRoles.ONEPROD_VIEWER
            };
        }
    }
}

