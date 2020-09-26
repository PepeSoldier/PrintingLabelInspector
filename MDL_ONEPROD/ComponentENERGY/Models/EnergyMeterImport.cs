using MDL_ONEPROD.ComponentENERGY.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentENERGY.Models
{
    public class EnergyMeterImport
    {
        public EnergyMeterImport()
        {
            Data = new List<EnergyMeterImportData>();
        }
        public int EnergyMeterId { get; set; }
        public int ColumnIndexCsv { get; set; }
        public EnumEnergyType EnergyType { get; set; }
        public List<EnergyMeterImportData> Data { get; set; }
    }


    public class EnergyMeterImportData
    {
        public decimal ReadValue { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}