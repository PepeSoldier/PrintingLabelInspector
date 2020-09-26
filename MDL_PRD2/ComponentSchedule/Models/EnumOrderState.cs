using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_PRD.ComponentSchedule.Models
{
    public enum EnumOrderState
    {
        Init = 0,
        Processing = 10,
        Ready = 50,
        Incomplete = 90,
    }
}