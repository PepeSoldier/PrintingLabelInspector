using MDL_BASE.Enums;
using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using XLIB_COMMON.Enums;

namespace MDL_ONEPROD.ComponentENERGY.Entities
{
    [Table("ENERGY_EnergyCost", Schema = "ONEPROD")]
    public class EnergyCost : IModelDeletableEntity
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public EnumEnergyType EnergyType { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public decimal PricePerUnit { get; set; }

        public decimal kWhConverter { get; set; }

        public bool UseConverter { get; set; }

        public bool Deleted { get; set; }
    }
}