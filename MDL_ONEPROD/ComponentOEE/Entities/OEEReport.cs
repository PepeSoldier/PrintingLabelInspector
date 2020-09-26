using MDL_BASE.ComponentBase.Entities;
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
    [Table("OEE_OEEReport", Schema = "ONEPROD")]
    public class OEEReport : IModelDeletableEntity
    {
        //Model Reprezentuje krok 1 formularza

        public OEEReport()
        {
            ReportDate = new DateTime().Date;
            TimeStamp = new DateTime();
        }

        public int Id { get; set; }
        public bool Deleted { get; set; }

        public DateTime ReportDate { get; set; }
        public DateTime TimeStamp { get; set; }
        public Shift Shift { get; set; }

        public virtual User User { get; set; }
        public string UserId { get; set; }

        public virtual ResourceOP Machine { get; set; }
        public int? MachineId { get; set; }

        public virtual LabourBrigade LabourBrigade { get; set; }
        public int? LabourBrigadeId { get; set; }

        public int TotalQtyCountedOnline { get; set; }
        public int TotalQtyDeclaredByOperator { get; set; }
        public int TotalStoppageTimeCountedOnline { get; set; }
        public int TotalStoppageTimeDeclaredByOperator { get; set; }

        public bool IsDraft { get; set; }
    }
}