using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentWHDOC.Enums;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_iLOGIS.ComponentWHDOC.Entities
{
    [Table("WHDOC_WhDocument", Schema = "iLOGIS")]
    public class WhDocumentWZ : WhDocumentAbstract, IModelEntity
    {
        public WhDocumentWZ()
        {
            DocumentDate = new DateTime(1900,1,1);
            StampTime = DateTime.Now;
        }

        public virtual User Approver { get; set; }
        [MaxLength(128)]
        public string ApproverId { get; set; }

        public virtual User SecurityApprover { get; set; }
        [MaxLength(128)]
        public string SecurityApproverId { get; set; }

    }
}