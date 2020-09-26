using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using MDL_BASE.Interfaces;
using MDLX_MASTERDATA.Entities;

namespace MDL_BASE.Models.MasterData
{
    [Table("MASTERDATA_Joblist")]
    public class JobList : IModelEntity
    {
        public int Id { get; set; }
        [MaxLength(10)]
        public string OrderNo { get; set; }

        public virtual Item Pnc { get; set; }
        public int? PncId { get; set; }

        public virtual Item Anc { get; set; }
        public int? AncId { get; set; }

        public int OrderQty { get; set; }

        public decimal ANCQty { get; set; }
        [MaxLength(9)]
        public string DirectParent { get; set; }
        [MaxLength(9)]
        public string InterChangeAble { get; set; }
        [MaxLength(3)]
        public string DEF { get; set; }

        public int BC { get; set; }
        public int LV { get; set; }
        [MaxLength(12)]
        public string Prefix { get; set; }

        public DateTime OrderDate { get; set; }


        public override string ToString()
        {
            return OrderNo.ToString();
        }

    }
}
