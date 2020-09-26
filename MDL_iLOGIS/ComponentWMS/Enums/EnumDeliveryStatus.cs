using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.Enums
{
    public enum EnumDeliveryStatus
    {
        Unknow = -1,
        Created = 0,
        /// <summary>Rozpoczęta, w trakcie realizacji przez operatora lub administratora</summary>
        Init = 10, 
        /// <summary>Zakończona przez operatora </summary>
        AfterInspection = 20,
        /// <summary>Zakończona przez operatora z niezgodnymi ilościami</summary>
        AfterInspectionWithProlem = 25,
        /// <summary>Potwierdzona przez administratora, lub sprawdzona przez administratora bez błędu</summary>
        Finished = 30,


    }
}
