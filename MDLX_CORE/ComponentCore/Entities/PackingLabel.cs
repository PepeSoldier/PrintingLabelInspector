using MDL_BASE.Interfaces;
using MDLX_MASTERDATA.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_CORE.ComponentCore.Entities
{
    [Table("MASTERDATA_PackingLabel")]
    public class PackingLabel : IModelEntity
    {
        public int Id { get; set; }

        [MaxLength(10)]
        public string OrderNo { get; set; }

        public virtual Item Pnc { get; set; }
        public int PncId { get; set; }

        [MaxLength(50)]
        public string SerialNumber { get; set; }

        public DateTime TimeStamp { get; set; }

        public override string ToString()
        {
            return OrderNo.ToString();
        }
    }
}