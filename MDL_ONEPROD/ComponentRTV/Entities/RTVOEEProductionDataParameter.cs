using MDL_BASE.Interfaces;
using MDL_CORE.ComponentCore.Enums;
using MDL_ONEPROD.Model.OEEProd;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentRTV.Entities
{
    [Table("RTV_RTVOEEReportProductionDataParameter", Schema = "ONEPROD")]
    public class RTVOEEProductionDataParameter : IModelDeletableEntity
    {
        public int Id { get; set; }
        public int MachineId { get; set; }

        public DateTime Date { get; set; }

        public RTVOEEParameter RTVParameter { get; set; }
        public int RTVParameterId { get; set; }

        [MaxLength(50)]
        public string Value { get; set; }
  

        public bool Deleted { get; set; }
    }
}