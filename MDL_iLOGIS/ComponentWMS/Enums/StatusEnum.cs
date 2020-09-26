using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.Enums
{
    public enum StatusEnum
    {
        Unknow = -1,
        Undefined = 0,
        /// <summary>
        /// Location: The storage bin is accessible.
        /// StockUnit: The article is accessible at random.
        /// </summary>
        Available = 1,
        /// <summary>
        /// Location: The storage bin is accessible.
        /// StockUnit: The article is reserved for an order which will be executed later. 
        /// It is ideally reserved with a reference to the order
        /// </summary>
        Reserved = 2,
        /// <summary>
        /// Location: The storage bin is blocked for future  (e.g., because  maintenance work).
        /// StockUnit: For some reason (expiry date exceeded, article in quarantine) 
        /// the article cannot be accessed or is blocked for certain operations (e.g., restorage).
        /// </summary>
        Blocked = 3,
        /// <summary>
        /// Location: ----.
        /// StockUnit: To be checked by quality before available 
        /// </summary>
        QualityInspection = 4,

        PickerProblem = 50,
        //QualityInspection = 70,
    }
}
