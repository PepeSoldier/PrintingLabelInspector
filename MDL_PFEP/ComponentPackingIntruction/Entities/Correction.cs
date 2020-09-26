using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MDL_PFEP.Model.ELDISY_PFEP
{
    public class Correction : IModelEntity
    {
        public int Id { get; set; }
        
        public int PackingInstructionId { get; set; }

        public DateTime ApplicationStart { get; set; }

        public DateTime? Applicationfinished { get; set; }

        public string ApplicantId { get; set; }

        public bool CorrectionClosed { get; set; }

        public bool CorrectionOpen { get; set; }

        public bool Deleted { get; set; }

        [Display(Name ="Uwagi do instrukcji: ")]
        public string CorrectionText { get; set; }

        public virtual PackingInstruction PackingInstruction { get; set; }
        public virtual User Applicant { get; set; }
    }
}