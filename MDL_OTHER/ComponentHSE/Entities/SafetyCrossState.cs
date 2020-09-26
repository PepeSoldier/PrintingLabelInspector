using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_OTHER.ComponentHSE.Entities
{
    public enum SafetyCrossState
    {
        None = 0,
        NoAccident = 1,
        Holiday = 2,
        NearMiss = 4,
        Accident = 8,
        Today = 16,
    }
}
