using MDL_BASE.Interfaces;
using MDL_BASE.Models.Base;
using MDL_BASE.Models.IDENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MDL_AP.Models.ActionPlan
{
    [Table("ActionActivity", Schema = "AP")]
    public class ActionActivity : IModelEntity
    {
        public int Id { get; set; }

        public virtual ActionModel Action { get; set; }
        public int ActionId { get; set; }

        [Display(Name = "Opis działania")]
        public string ActivityDescription { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual User Creator { get; set; }
        public string CreatorId { get; set; }

        public ActionActivityTypeEnum ActivityEnum { get; set; }

        [NotMapped]
        public List<Attachment> Attachments { get; set; }
    }
}