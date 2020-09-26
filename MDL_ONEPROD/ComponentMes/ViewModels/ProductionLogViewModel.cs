using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentMes.ViewModels
{
    public class ProductionLogViewModel
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
            
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }

        public int WorkplaceId { get; set; }
        public string WorkplaceName { get; set; }

        public int ItemId { get; set; }                 //produkowany item        
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        
        public int? ReasonTypeId { get; set; }
        public string ReasonTypeName { get; set; }

        public int? ReasonId { get; set; }
        public string ReasonName { get; set; }

        public string ClientWorkOrderNumber { get; set; }        
        public string InternalWorkOrderNumber { get; set; }

        public decimal WorkorderTotalQty { get; set; }  //Suma zamówienia wewnętrznego 
        public decimal DeclaredQty { get; set; }        //Zadeklarowana ilość
        public decimal UsedQty { get; set; }        //Zadeklarowana ilość
        
        public string SerialNo { get; set; }            //Wygenerowany numer na Barkodzie
        public string UserName { get; set; }
        public string CostCenter { get; set; }      //Centrum kosztów
        public string TransferNumber { get; set; }  //Plik do komunikacji z innym systemem

        public bool Deleted { get; set; }

        public decimal CycleTime { get; set; }        
        public decimal QtyAvailable { get; set; }
    }
}