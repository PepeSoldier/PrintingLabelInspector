using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using MDL_BASE.Interfaces;
using MDLX_MASTERDATA.Enums;
using MDL_BASE.Models.MasterData;
using MDL_BASE.Interface;

namespace MDLX_MASTERDATA.Entities //MDL_ONEPROD.Model.Scheduling
{
    [Table("MASTERDATA_Resource")]
    public class Resource2 : IModelEntity, IModelDeletableEntity, IModelEntityName, IDefComboModel
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }

        public ResourceTypeEnum Type { get; set; }

        public virtual Resource2 ResourceGroup { get; set; }
        public int? ResourceGroupId { get; set; }

        public Area Area { get; set; }
        public int? AreaId { get; set; }

        [MaxLength(35)]
        public string Color { get; set; }

        public int FlowTime { get; set; } //w sekudach

        public int OldId { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }
        
        public override string ToString()
        {
            return Name;
        }
    }
}
