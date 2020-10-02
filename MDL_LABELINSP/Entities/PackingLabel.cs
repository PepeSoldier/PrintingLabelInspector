using MDL_BASE.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_LABELINSP.Entities
{
    [Table("LABELINSP_PackingLabel")]
    public class PackingLabel : IModelEntity
    {
        public int Id { get; set; }

        [MaxLength(10)]
        public string OrderNo { get; set; }

        [MaxLength(50)]
        public string ItemCode { get; set; }

        [MaxLength(50)]
        public string ItemName { get; set; }

        [MaxLength(50)]
        public string SerialNumber { get; set; }

        public DateTime TimeStamp { get; set; }

        public override string ToString()
        {
            return OrderNo.ToString();
        }
    }
}