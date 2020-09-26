using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_AP.Models.DEF
{
    [Table("DEF_Category", Schema = "AP")]
    public class Category : IModelEntity, IDefModel
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Display(Name = "Nazwa Kategorii")]
        public string Name { get; set; }

        public bool Deleted { get; set; }
    }
}