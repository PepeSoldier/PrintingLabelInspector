using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_iLOGIS.ComponentConfig.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XLIB_COMMON.Enums;

namespace MDL_iLOGIS.ComponentWHDOC.Entities
{
    [Table("WHDOC_WhDocumentItem", Schema = "iLOGIS")]
    public class WhDocumentItem : IModelEntity
    {
        public int Id { get; set; }

        public virtual User User { get; set; }
        [MaxLength(128)]
        public string UserId { get; set; }

        public virtual WhDocumentAbstract WhDocument { get; set; }
        public int WhDocumentId { get; set; }

        public virtual ItemWMS ItemWMS { get; set; }
        public int? ItemWMSId { get; set; }

        public virtual Package Package { get; set; }
        public int? PackageId { get; set; }

        [MaxLength(100)]
        public string ItemCode { get; set; }

        [MaxLength(100)]
        public string ItemName { get; set; }

        public decimal DisposedQty { get; set; }
        public decimal IssuedQty { get; set; }
        public decimal UnitPrice { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public bool Deleted { get; set; }

        [NotMapped]
        public decimal Value
        {
            get
            {
                return IssuedQty * UnitPrice;
            }
        }
    }


    public class WhDocumentItemLight
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string ItemCode { get; set; }

        [MaxLength(100)]
        public string ItemName { get; set; }

        public decimal DisposedQty { get; set; }
        public decimal IssuedQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Value { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
    }
}