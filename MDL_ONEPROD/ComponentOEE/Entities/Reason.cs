using MDL_BASE.Interfaces;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Model.OEEProd
{
    [Table("OEE_Reason", Schema = "ONEPROD")]
    public class Reason : IModelDeletableEntity
    {
        public Reason(){}

        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string NameEnglish { get; set; }
        [MaxLength(35)]
        public string Color { get; set; }
        [MaxLength(35)]
        public string ColorGroup { get; set; }

        public bool IsGroup { get; set; }

        public virtual Reason Group { get; set; }
        public int? GroupId { get; set; }

        //public EnumEntryType EntryType { get; set; }
        public virtual ReasonType ReasonType { get; set; }
        public int ReasonTypeId { get; set; }
        
        public string GetName(int languageId)
        {
            return languageId == 1 ? Name : NameEnglish;
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Deleted { get; set; }
    }
}