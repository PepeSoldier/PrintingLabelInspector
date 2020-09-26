using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MDL_AP.Models.ActionPlan
{
    public enum ObservationTypeEnum
    {
        [Display(Name = "Akcja/Zadanie")]
        Task = 0,
        [Display(Name = "Audyt")]
        Audit = 1,
        [Display(Name = "5D")]
        FiveD = 2,
        [Display(Name = "RedTag")]
        RedTag = 3,
        [Display(Name = "Blokada")]
        Blockade = 4,
        [Display(Name = "Problem Solving")]
        ProblemSolving = 5,
        [Display(Name = "A3/PDCA")]
        A3PDCA = 6,
        [Display(Name = "Małe Aktywności")]
        SmallActivity = 7,
    }
}