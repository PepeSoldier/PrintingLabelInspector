using MDL_BASE.Enums;
using MDL_BASE.Interfaces;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using XLIB_COMMON.Enums;

namespace MDL_ONEPROD.ComponentENERGY.Entities
{
    [Table("ENERGY_EnergyMeter", Schema = "ONEPROD")]
    public class EnergyMeter : IModelDeletableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        public string MarkedName { get; set; }

        public string Description { get; set; }

        public virtual ResourceOP Resource { get; set; }
        public int? ResourceId { get; set; }

        public EnumEnergyType EnergyType { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public bool Deleted { get; set; }
    }
}