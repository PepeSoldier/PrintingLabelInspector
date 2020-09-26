using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MDLX_MASTERDATA.Entities
{
    [Table("MASTERDATA_Process")]
    public class Process : IModelEntity, IModelDeletableEntity
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }

        public int ParentId { get; set; }
        
        public bool Deleted { get; set; }
    }
}
