using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.Enums
{
    public enum ClassificationXYZEnum
    {
        Unknow = -1,
        Undefined = 0,
        /// <summary>X – duże tempo zużycia</summary>
        X = 1,
        /// <summary>Y – średnie tempo zużycia</summary>
        Y = 2,
        /// <summary>Z – małe tempo zużycia</summary>
        Z = 3,
    }
}
