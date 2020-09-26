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
    [Table("OEE_ReasonType", Schema = "ONEPROD")]
    public class ReasonType : IModelDeletableEntity
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string NameEnglish { get; set; }

        public EnumEntryType EntryType { get; set; }
       
        public bool GenerateCharts { get; set; }

        public int SortOrder { get; set; }

        public string Color { get; set; }

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