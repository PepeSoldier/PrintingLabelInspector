using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace MDL_PRD.Model
{
    [Table("PSI_Reason", Schema = "PRD")]
    public class ReasonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public bool Active { get; set; }

        [NotMapped]
        public string ReasonDetails { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
