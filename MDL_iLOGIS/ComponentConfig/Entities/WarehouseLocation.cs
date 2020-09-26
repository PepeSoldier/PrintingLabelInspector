using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using XLIB_COMMON.Model;

namespace MDL_iLOGIS.ComponentConfig.Entities
{
    [Table("CONFIG_WarehouseLocation", Schema = "iLOGIS")]
    public class WarehouseLocation : IModelEntity, IModelDeletableEntity
    {
        public WarehouseLocation() {
            UpdateDate = DateTime.Now;
        }

        public int Id { get; set; }

        public virtual Warehouse Warehouse { get; set; }
        public int WarehouseId { get; set; }

        public virtual WarehouseLocation ParentWarehouseLocation { get; set; }
        public int? ParentWarehouseLocationId { get; set; }

        //public WarehouseLocationTypeEnum TypeEnum { get; set; }
        public virtual WarehouseLocationType Type { get; set; }
        public int? TypeId { get; set; }

        [MaxLength(6)]
        public string RegalNumber { get; set; }
        public int ColumnNumber { get; set; }
        public int ShelfNumber { get; set; }
        public bool AvailableForPicker { get; set; }

        [MaxLength(25)]
        public string Name { get; set; }
        [NotMapped]
        public string NameFormatted
        {
            get
            {
                if (Type != null && Type.DisplayFormat != null)
                {
                    return StringFormatter.Format(Type.DisplayFormat, Name);
                }
                else
                {
                    return Name;
                }
            }
        }       

        public decimal Utilization { get; set; }

        public int InsertCounter { get; set; }        
        public int RemoveCounter { get; set; }
        public DateTime UpdateDate { get; set; }

        public ClassificationABCEnum ABC { get; set; }
        public ClassificationXYZEnum XYZ { get; set; }

        public bool Deleted { get; set; }

        //TODO: wyrzucić ten property (QtyOfSubLocations)
        public int QtyOfSubLocations { get; set; }

        //public int W { get; set; } //Grouping Parameter 
        //public int V { get; set; } //Location Parameter 
    }
    
}