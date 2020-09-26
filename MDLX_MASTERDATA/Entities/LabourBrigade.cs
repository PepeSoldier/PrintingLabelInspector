using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_BASE.ComponentBase.Entities
{
    [Table("MASTERDATA_LabourBrigade")]
    public class LabourBrigade : IModelEntity, IDefModel
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Display(Name = "Numer Brygady")]
        public string Name { get; set; }

        public bool Deleted { get; set; }

    }
}