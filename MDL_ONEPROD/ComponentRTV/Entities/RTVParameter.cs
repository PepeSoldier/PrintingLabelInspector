using MDL_BASE.Interfaces;
using MDL_CORE.ComponentCore.Enums;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentRTV.Entities
{
    [Table("RTV_RTVOEEParameter", Schema = "ONEPROD")]
    public class RTVOEEParameter : IModelDeletableEntity
    {
        public int Id { get; set; }

        public ResourceOP Resource { get; set; }
        public int ResourceId { get; set; }

        public EnumVariableType DataType { get; set; }
        public int Decimals { get; set; }
        public decimal MinChangeValue { get; set; }

        [MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Target { get; set; }
        [MaxLength(50)]
        public string Min { get; set; }
        [MaxLength(50)]
        public string Max { get; set; }

        public string MemoryAddress { get; set; }

        public bool Deleted { get; set; }
    }
}