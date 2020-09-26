using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_PFEP.Models.DEF
{
    public static class Routines
    {
        public static List<Routine> GetRoutines()
        {
            List<Routine> routines = new List<Routine>();
            routines.Add(new Routine {
                            Id = 1, Hours = 12, DEFs = new string[] { "155", "364" }, Name = "KANBAN",
                            //Workstaitons = new string[] { "D4", "D5", "9", "9A" },
                            ShowOnlyChanges = false,
                            ShowLocation = true,
                            ShowFullOrders = true,
                            AutoprintShiftHours = -2
            });
            //routines.Add(new Routine {
            //                Id = 10, Hours = 10, DEFs = new string[] { "154" }, Name = "KANBAN-154",
            //                Workstaitons = new string[] { "D4", "D5", "9", "9A" },
            //                ShowOnlyChanges = false,
            //                ShowLocation = true,
            //                ShowFullOrders = true
            //});
            routines.Add(new Routine {
                            Id = 8, Hours = 12, DEFs = new string[] { "164", "374" }, Name = "EURO WOZEK (PRZYG.)",
                            AddPrefixes = new string[2]{ "1010100", "1010300" },
                            ShowWorkst = true,
                            ShowLocation = true,
                            ShowFullOrders = true,
                            AutoprintShiftHours = -2
            });

            routines.Add(new Routine{
                            Id = 9, Hours = 12, DEFs = new string[] { "164" }, Name = "EURO WOZEK (POCIĄG)",
                            ShowFullOrders = true,
                            AutoprintShiftHours = -2
            });

            routines.Add(new Routine {
                            Id = 3, Hours = 10, DEFs = new string[] { "174" }, Name = "MSK",
                            AddLevel = 2,
                            ShowOrders = false,
                            ShowPackagesCount = true,
                            ShowFullOrders = true,
                            AutoprintShiftHours = -2,
                            PrintAllLinesTogether = true
            });
            routines.Add(new Routine {
                            Id = 4, Hours = 12, DEFs = new string[] { "134", "374" }, Name = "WSP.",
                            AddPrefixes = new string[1]{ "0300010" },
                            ShowFullOrders = true,
                            AutoprintShiftHours = -2
                            //AddLevel = 2,
            });
            routines.Add(new Routine {
                            Id = 5, Hours = 8, DEFs = new string[] { "194" }, Name = "PODMTŻ. KOSZA G.i D.",
                            AddLevel = 2,
                            ShowOrders = false,
                            ShowLocation = true,
                            ShowPackagesCount = true,
                            ShowFullOrders = true,
                            AutoprintShiftHours = 2,
                            PrintAllLinesTogether = true
            });
            //routines.Add(new Routine {
            //                Id = 6, Hours = 1, DEFs = new string[] { "114" }, Name = "BRYGADZISTA",
            //                ShowFullOrders = true,
            //                AutoprintShiftHours = 0
            //});
            routines.Add(new Routine {
                            Id = 7, Hours = 12, DEFs = new string[] { "120", "125" }, Name = "TECHNOLOGIA",
                            ShowFullOrders = true,
                            AutoprintShiftHours = -2,
                            AddPrefixes = new string[] { "1610100" }
            });

            return routines;
        }
    }

    public class Routine
    {
        public Routine()
        {
            AddLevel = 0;
            PrintAllLinesTogether = false;
            ShowOrders = true;
            ShowPackagesCount = false;
            ShowOnlyChanges = false;
            ShowWorkst = true;
            ShowFullOrders = false;
            PrintType = 1; //PrintType - nie ma wpływu na wydruk typu matrix.
            AddPrefixes = new string[0] { };
            RemovePrefixes = new string[0] { };
        }

        public int Id { get; set; }
        public int Hours { get; set; }
        public string Name { get; set; }
        public string[] DEFs { get; set; }
        public string[] IDCOs { get; set; }
        public string[] AddPrefixes { get; set; }
        public string[] RemovePrefixes { get; set; }
        public string[] Workstaitons { get; set; }
        public int PrintType { get; set; }
        public int AddLevel { get; set; }
        public bool PrintAllLinesTogether { get; set; }
        public bool ShowOrders { get; set; }
        public bool ShowLocation { get; set; }
        public bool ShowPackagesCount { get; set; }
        public bool ShowFullOrders { get; set; }
        public bool ShowWorkst { get; set; }
        public bool ShowOnlyChanges { get; set; }
        public int AutoprintShiftHours { get; set; }

        public override string ToString()
        {
            return Hours.ToString() + "h-" + string.Join(",",DEFs) + "-" + Name;
        }
    }
}