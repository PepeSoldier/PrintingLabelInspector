using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Model.Scheduling.Interface
{
    public enum EnumEntryType
    {
        Undefined = -1,
        TimeAvailable = 0,
        TimeClosed = 2,
        ReasonNotSelected = 5,

        Production = 10,
        ScrapMaterial = 11,
        ScrapProcess = 12,
        //ScrapProcessScratch = 13,   //rysy
        //ScrapProcessDent= 14,       //wgnioty
        //ScrapProcessCrack = 15,     //pekniecia
        //ScrapProcessFold = 16,      //marszczenia
        ScrapLabel = 19,            //Scrap Etykiety

        StopPlanned = 20,
        //StopPlannedChangeOver = 21,

        StopUnplanned = 30,
        //StopUnplannedBreakdown = 31,
        //StopUnplannedPreformance = 32,
        //StopUnplannedChangeOver = 33,
        StopPerformance = 32,

        StopQuality = 40
    }
}