using MDL_BASE.Interfaces;
using MDL_CORE.ComponentCore.Models;
using MDL_iLOGIS.ComponentConfig.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentCore
{
    public static class iLogisSerialNumberManager
    {
        public static string GetNext(IDbContextCore db, Warehouse warehouse)
        {
            int serialRaw = new SerialNumberManager(db).GetNextForILOGIS(GetWarehouseId(warehouse));

            return FormatSerialNumber(serialRaw);
        }
        public static List<string> GetMany(IDbContextCore db, int qty, Warehouse warehouse)
        {
            int[] serialsRaw = new SerialNumberManager(db).GetManyForILOGIS(qty, GetWarehouseId(warehouse));
            List<string> serialNumbers = new List<string>();

            foreach (int serialRaw in serialsRaw)
            {
                serialNumbers.Add(FormatSerialNumber(serialRaw));
            }

            return serialNumbers;
        }

        private static int? GetWarehouseId(Warehouse warehouse)
        {
            return warehouse != null && warehouse.IndependentSerialNumber == true ? (int?)warehouse.Id : null;
        }
        private static string FormatSerialNumber(int serialRaw)
        {
            return SerialNumberManager.Format(serialRaw, XLIB_COMMON.Enums.SerialNumberType.YWWD6);
        }
    }
}
