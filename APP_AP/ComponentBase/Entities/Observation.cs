using MDL_BASE.Interfaces;
using MDL_AP.Models.DEF;
using MDL_BASE.Models.IDENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA.Entities;
using MDL_BASE.ComponentBase.Entities;

namespace MDL_AP.Models.ActionPlan
{
    [Table("Observation", Schema = "AP")]
    public class Observation : IModelEntity
    {
        public int Id { get; set; }

        [Display(Name = "Data")]
        public DateTime Date { get; set; }

        [Display(Name = "Problem")]
        public string Problem { get; set; }

        [Display(Name = "Obszar")]
        public virtual Area Area { get; set; }
        public int AreaId { get; set; }

        [Display(Name = "Linia")]
        public virtual Resource2 Line { get; set; }
        public int LineId { get; set; }

        [Display(Name = "Zmiana")]
        public virtual LabourBrigade ShiftCode { get; set; }
        public int ShiftCodeId { get; set; }
        
        [Display(Name = "Stanowisko")]
        public virtual Workstation Workstation { get; set; }
        public int WorkstationId { get; set; }

        [Display(Name = "Kategoria")]
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }

        public virtual User Creator { get; set; }
        [Display(Name = "Zgłaszający")]
        public string CreatorId { get; set; }

        public ObservationTypeEnum Type { get; set; }
    }
}