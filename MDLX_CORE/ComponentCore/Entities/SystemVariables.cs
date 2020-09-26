using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_CORE.ComponentCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDLX_CORE.ComponentCore.Entities
{
    [Table("SystemVariables", Schema = "CORE")]
    public class SystemVariable : IModelEntity
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string Value { get; set; }
        public EnumVariableType Type { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}