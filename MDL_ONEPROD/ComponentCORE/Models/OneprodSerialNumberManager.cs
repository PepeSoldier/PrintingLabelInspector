using MDL_BASE.Interfaces;
using MDL_CORE.ComponentCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentCORE.Models
{
    public class OneprodSerialNumberManager
    {
        public static string GenerateSerialNumber(IDbContextCore db, int resourceId)
        {
            return new SerialNumberManager(db).GetNextForONEPROD(resourceId).ToString();

            //return SerialNumberManager.FormatSerialNumber(serialRaw, XLIB_COMMON.Enums.SerialNumberType.YWWD6);
        }
    }
}