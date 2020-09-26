using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentConfig.Entities
{
    [Table("CONFIG_Item", Schema = "iLOGIS")]
    public class ItemWMS : IModelEntity //: Item
    {
        public int Id { get; set; }
        public virtual Item Item { get; set; }
        public int ItemId { get; set; }

        [DisplayName("Waga"), Range(0, Int32.MaxValue)]
        public decimal Weight { get; set; }

        ////public int W { get; set; } //Grouping Parameter 
        //public Warehouse Warehouse { get; set; }
        //public int? WarehouseId { get; set; }
        ////public int V { get; set; } //Location Parameter
        //public WarehouseLocationType WarehouseLocationType { get; set; }
        //public int? WarehouseLocationTypeId { get; set; }

        //public int T { get; set; } //intensive picking - sposób dodawania i wyciągania z lokalizacji: intensive-można dorzucać kolejne sztuki, picking:nie można dorzycać sztuk do zajętej lokalizacji.
        //public PickingStrategyEnum PickingStrategy { get; set; }
        
        public int H { get; set; } //podzial dla picking listy.

        public int PickerNo { get; set; }
        public int TrainNo { get; set; }

        public ClassificationABCEnum ABC { get; set; }
        public ClassificationXYZEnum XYZ { get; set; }
    }
}
