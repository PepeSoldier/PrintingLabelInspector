using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_BASE.Models.Base
{
    [Table("ChangeLog", Schema = "CORE")]
    public class ChangeLog : IModelEntity
    {
        public int Id { get; set; }
        [MaxLength(70)]
        public string ObjectName { get; set; }
        [MaxLength(100)]
        public string ObjectDescription { get; set; }
        [MaxLength(70)]
        public string FieldName { get; set; }
        [MaxLength(140)]
        public string FieldDisplayName { get; set; }
        [MaxLength(255)]
        public string NewValue { get; set; }
        [MaxLength(255)]
        public string OldValue { get; set; }

        public int ObjectId { get; set; }
        
        public int ParentObjectId { get; set; }
        [MaxLength(70)]
        public string ParentObjectName { get; set; }
        [MaxLength(100)]
        public string ParentObjectDescription { get; set; }

        public virtual User User { get; set; }
        [MaxLength(128)]
        public string UserId { get; set; }

        public DateTime Date { get; set; }
    }
}