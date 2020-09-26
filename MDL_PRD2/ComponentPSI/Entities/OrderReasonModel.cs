using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace MDL_PRD.Model
{
    [Table("PSI_OrderReason", Schema = "PRD")]
    class OrderReasonModel
    {
        public int Id { get; set; }
        public virtual OrderArchiveModel OrderArchive { get; set; }
        public virtual ReasonModel Reason { get; set; }
    }
}
