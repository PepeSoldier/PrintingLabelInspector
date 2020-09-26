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
    [Table("OEE_OEEReportEmployee", Schema = "ONEPROD")]
    public class OEEReportEmployee : IModelDeletableEntity
    {
        public OEEReportEmployee() { }


        public int Id { get; set; }

        public virtual OEEReport Report { get; set; }
        public int ReportId { get; set; }

        public string EmployeeName { get; set; }
        public int SkillsCount { get; set; }
        public EnumOperatorType enumOperatorType { get; set; }

        public bool Deleted { get; set; }
    }

    public enum EnumOperatorType
    {
        Ustawiacz = 1,
        Operator = 2,
        Prowadzacy = 3
    }
}