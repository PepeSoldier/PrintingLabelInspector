using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using MDL_ONEPROD.Model.Scheduling.Interface;
using MDL_ONEPROD.Common;
using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_ONEPROD.Model.OEEProd;

namespace MDL_ONEPROD.Model.Scheduling
{
    [Table("MES_ProductionLog", Schema = "ONEPROD")]
    public class ProductionLog : IModelEntity, IModelDeletableEntity
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual OEEReportProductionData OEEReportProductionData { get; set; }
        public int? OEEReportProductionDataId { get; set; }

        public virtual Workplace Workplace { get; set; }
        public int WorkplaceId { get; set; }

        public virtual ItemOP Item { get; set; }
        public int ItemId { get; set; }                 //produkowany item

        [NotMapped, MaxLength(50)]
        public string ItemCode { get; set; }

        //public EnumEntryType EntryType { get; set; }
        public virtual ReasonType ReasonType { get; set; }
        public int? ReasonTypeId { get; set; }

        public virtual Reason Reason { get; set; }
        public int? ReasonId { get; set; }

        [MaxLength(20)]
        public string ClientWorkOrderNumber { get; set; } 
        [MaxLength(100)]
        public string InternalWorkOrderNumber { get; set; }

        public decimal WorkorderTotalQty { get; set; }  //Suma zamówienia wewnętrznego 
        public decimal DeclaredQty { get; set; }        //Zadeklarowana ilość
        public decimal UsedQty { get; set; }        //Zadeklarowana ilość
        [MaxLength(25)]
        public string SerialNo { get; set; }            //Wygenerowany numer na Barkodzie
        [MaxLength(40)]
        public string UserName { get; set; }
        [MaxLength(15)]
        public string CostCenter { get; set; }      //Centrum kosztów
        [MaxLength(50)]
        public string TransferNumber { get; set; }  //Plik do komunikacji z innym systemem

        public bool Deleted { get; set; }

        [NotMapped]
        public decimal CycleTime { get; set; }

        [NotMapped]
        public decimal QtyAvailable { get { return DeclaredQty - UsedQty; } }
    }
}
