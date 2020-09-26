using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_PFEP.Model.ELDISY_PFEP
{
    [Table("Calculation", Schema = "PFEP")]
    public class Calculation : IModelEntity
    {
        public int Id { get; set; }

        public int PackingInstructionId { get; set; }

        public string ClientProfileCode { get; set; }

        public string ProfileCode { get; set; }

        public string ProfileName { get; set; }

        public decimal PackingInstructionPrice { get; set; }

        public decimal CalculatedInstructionPrice { get; set; }

        public decimal SetInstructionPrice { get; set; }

        public virtual PackingInstruction PackingInstruction { get; set; }

        [NotMapped]
        public int pageIndex { get; set; }
        [NotMapped]
        public int pageSize { get; set; }

    }
}