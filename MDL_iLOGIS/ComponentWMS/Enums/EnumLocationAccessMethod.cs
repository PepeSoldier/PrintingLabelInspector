using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS.Enums
{
    public enum EnumLocationAccessMethod
    {
        Unknow = -1,
        Unassigned = 0,
        FIFO = 1,
        LIFO = 2,
        RandomAccess = 4,
        Stack = 6,

    }
}
