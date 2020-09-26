using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_AP.Models.ActionPlan
{
    [Table("Meeting", Schema = "AP")]
    public class Meeting : IModelEntity
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa Spotkania")]
        public string MeetingName { get; set; }

        public virtual User Owner { get; set; }
        [Display(Name = "Prowadzący")]
        public string OwnerId { get; set; }

        [Display(Name = "Data początkowa")]
        public DateTime BeginnigDate { get; set; }

        [Display(Name = "Data końcowa")]
        public DateTime? EndDate { get; set; }
        
    }
}