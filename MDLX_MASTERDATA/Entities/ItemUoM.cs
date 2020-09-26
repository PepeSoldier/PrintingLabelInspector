using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using MDL_BASE.Interfaces;
using MDLX_MASTERDATA.Enums;
using XLIB_COMMON.Enums;

namespace MDLX_MASTERDATA.Entities
{
    [Table("MASTERDATA_ItemUoM")]
    public class ItemUoM : IModelEntity, IModelDeletableEntity
    {
        public ItemUoM() { }

        public int Id { get; set; }
        
        public Item Item { get; set; }
        public int ItemId { get; set; }

        public UnitOfMeasure DefaultUnitOfMeasure { get; set; }
        public decimal QtyForDefaultUnitOfMeasure { get; set; }
        public UnitOfMeasure AlternativeUnitOfMeasure { get; set; }
        public decimal QtyForAlternativeUnitOfMeasure { get; set; }


        public bool Deleted { get; set; }

    }

}
