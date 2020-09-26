using MDL_BASE.Interfaces;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentENERGY.Entities
{
    [Table("ENERGY_EnergyConsumptionData", Schema = "ONEPROD")]
    public class EnergyConsumptionData : IModelDeletableEntity
    {
        public int Id { get; set; }

        public virtual EnergyMeter EnergyMeter { get; set; }
        public int EnergyMeterId { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public DateTime ImportDate { get;set; }

        public decimal Qty { get; set; }
        public decimal PricePerUnit { get; set; }
        [Obsolete]
        public decimal Cost { get; set; }
        public decimal? TotalValue { get; set; }

        [Obsolete]
        public decimal ProductionQty { get; set; }
        [Obsolete]
        public decimal TotalStopTime { get; set; }

        public bool Deleted { get; set; }
    }
}