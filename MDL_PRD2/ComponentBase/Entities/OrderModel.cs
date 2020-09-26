using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using MDL_BASE.Interfaces;

namespace MDL_PRD.Model
{
    [Table("PSI_Order", Schema = "PRD")]
    public class OrderModel : IModelEntity
    {
        public int Id { get; set; }
        [MaxLength(5)]
        public string Line { get; set; }
        [MaxLength(10)]
        public string OrderNo { get; set; }
        [MaxLength(9)]
        public string PNC { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int QtyPlanned { get; set; }
        public int QtyRemain { get; set; }

        public override string ToString()
        {
            return OrderNo.ToString();
        }

    }
}
