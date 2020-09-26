using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_iLOGIS.ComponentWMS.ViewModels
{
    public class DeliveryListItemViewModel
    {
        public int Id { get; set; }
        public int ItemWMSId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Workstation { get; set; }
        public int? WorkstationId { get; set; }
        public int WorkstationOrder { get; set; }
        public string WorkstationName { get; set; }
        public int WorkstationProductsFromIn { get; set; }
        public int WorkstationProductsFromOut { get; set; }
        public int TransporterId { get; set; }
        public string TransporterName { get; set; }
        public int WorkorderId { get; set; }
        public string WorkorderNumber { get; set; }
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }

        public decimal QtyRequested { get; set; }
        public decimal QtyDelivered { get; set; }
        public decimal QtyUsed { get; set; }
        public decimal QtyPerPackage { get; set; }
        public decimal BomQty { get; set; }
        public int CoveredSeconds { get; set; }
        public DateTime MaxCoveredTime { get; set; }
        public DateTime MaxConsideredTime { get; set; }

        public int DeliveryListId { get; set; }
        public int StockUnitId { get; set; }
        public int WarehouseLocationId { get; set; }
        public string WarehouseLocationName { get; set; }
        public string WorkstationPutTo { get; set; }
        public int ParameterH { get; set; }
        public bool IsSubstituteData { get; set; }

        public EnumDeliveryListItemStatus Status { get; set; }

        public override string ToString()
        {
            return Code + " - " + Name;
        }
    }
}