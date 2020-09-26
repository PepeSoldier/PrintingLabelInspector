using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using XLIB_COMMON.Enums;

namespace MDL_ONEPROD.Model.OEEProd
{
    public abstract class OEEReportProductionDataAbstract : IModelDeletableEntity
    {
        public OEEReportProductionDataAbstract()
        {   
        }

        public int Id { get; set; }
        public bool Deleted { get; set; }

        public DateTime ProductionDate { get; set; }

        public virtual ItemOP Item { get; set; }
        public int? ItemId { get; set; }

        public decimal ProdQty { get; set; }

        [DataType("decimal(18,2")]
        public decimal CycleTime { get; set; }
        
        //<summary>value in seconds</summary>
        public decimal UsedTime { get; set; }
        
        //public EnumEntryType EntryType { get; set; }

        public virtual ReasonType ReasonType { get; set; }
        public int? ReasonTypeId { get; set; }


        public virtual Reason Reason { get; set; }
        public int? ReasonId { get; set; }
                
        public virtual User User { get; set; }
        public string UserId { get; set; }

        public DateTime TimeStamp { get; set; }
        
        public virtual OEEReportProductionDataDetails Detail { get; set; }
        public int? DetailId { get; set; }

        public override string ToString()
        {
            return User.UserName + "-" + UsedTime.ToString();
        }
    }
}