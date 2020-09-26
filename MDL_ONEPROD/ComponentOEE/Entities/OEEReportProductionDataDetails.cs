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
    [Table("OEE_OEEReportProductionDataDetails", Schema = "ONEPROD")]
    public class OEEReportProductionDataDetails : IModelDeletableEntity
    {
        public OEEReportProductionDataDetails()
        {
        }

        public int Id { get; set; }
        public bool Deleted { get; set; }

        [MaxLength(200)]
        public string Comment { get; set; }

        [DataType("decimal(18,2")]
        public decimal ProductionCycleTime { get; set; }

        public int CoilId { get; set; }
        public decimal FormWeightProcess { get; set; }
        public decimal FormWeightScrap { get; set; }
        public decimal PaperWeight { get; set; }
        public decimal TubeWeight { get; set; }

        public decimal TimeOptInMin { get; set; }
        public decimal TimeUrInMin { get; set; }

        public override string ToString()
        {
            return "ct:" + ProductionCycleTime.ToString("0.##") + "; comment:" + Comment;
        }

    }
}