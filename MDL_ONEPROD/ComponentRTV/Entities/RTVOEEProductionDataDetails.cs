using MDL_BASE.Interfaces;
using MDL_CORE.ComponentCore.Enums;
using MDL_ONEPROD.Model.OEEProd;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentRTV.Entities
{
    [Table("RTV_RTVOEEReportProductionDataDetails", Schema = "ONEPROD")]
    public class RTVOEEProductionDataDetails : IModelDeletableEntity
    {
        public RTVOEEProductionDataDetails()
        {
            this.CycleTime = 0;
            //this.PirometerTemp1 = 0;
            //this.PirometerTemp2 = 0;
            //this.PirometerTemp3 = 0;
        }
    
        public int Id { get; set; }
        public bool Deleted { get; set; }

        public string PartName { get; set; }
        public string ProgramName { get; set; }
        public int ProgramNo { get; set; }
        
        public decimal? CycleTime { get; set; }
        public decimal? PirometerTemp1 { get; set; }
        public decimal? PirometerTemp2 { get; set; }
        public decimal? PirometerTemp3 { get; set; }
        public decimal? PirometerTemp4 { get; set; }
        public decimal? PirometerTemp5 { get; set; }
        public decimal? PirometerTemp6 { get; set; }
        public decimal? PirometerTemp7 { get; set; }
        public decimal? PirometerTemp8 { get; set; }

        public decimal? PirometerMin1 { get; set; }
        public decimal? PirometerMin2 { get; set; }
        public decimal? PirometerMin3 { get; set; }
        public decimal? PirometerMin4 { get; set; }
        public decimal? PirometerMin5 { get; set; }
        public decimal? PirometerMin6 { get; set; }
        public decimal? PirometerMin7 { get; set; }
        public decimal? PirometerMin8 { get; set; }

        public decimal? PirometerMax1 { get; set; }
        public decimal? PirometerMax2 { get; set; }
        public decimal? PirometerMax3 { get; set; }
        public decimal? PirometerMax4 { get; set; }
        public decimal? PirometerMax5 { get; set; }
        public decimal? PirometerMax6 { get; set; }
        public decimal? PirometerMax7 { get; set; }
        public decimal? PirometerMax8 { get; set; }

        public virtual RTVOEEProductionData RTVOEEProductionData { get; set; }
    }
}