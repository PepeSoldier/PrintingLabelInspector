using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_AP.Models.DEF
{
    [Table("DEF_Type", Schema = "AP")]
    public class Type : IModelEntity, IDefModel
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Display(Name = "Nazwa Typu")]
        public string Name { get; set; }

        public bool Deleted { get; set; }
    }
}