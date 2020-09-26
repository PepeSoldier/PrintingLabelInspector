using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using MDL_ONEPROD.Model.Scheduling.Interface;
using MDL_ONEPROD.Common;
using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;
using MDLX_MASTERDATA.Entities;

namespace MDL_ONEPROD.ComponentMes.Etities
{
    namespace MDL_ONEPROD.Model.Scheduling
    {
        [Table("MES_WorkplaceBuffer", Schema = "ONEPROD")]
        public class WorkplaceBuffer : IModelEntity
        {
            public int Id { get; set; }

            public virtual Workorder ParentWorkorder { get; set; }
            public int ParentWorkorderId { get; set; }
            
            public virtual Workplace Workplace { get; set; }
            public int WorkplaceId { get; set; }

            public virtual ItemOP Parent { get; set; }
            public int ParentId { get; set; }

            public virtual ItemOP Child { get; set; }
            public int ChildId { get; set; }

            public virtual ProductionLog ProductionLog { get; set; }
            public int? ProductionLogId { get; set; }

            public int ProcessId { get; set; }

            public string Barcode { get; set; }

            public string SerialNumber { get; set; }

            public decimal QtyAvailable { get; set; }

            public decimal QtyInBom { get; set; }

            public string Code { get; set; }

            public string Name { get; set; }

            public DateTime TimeLoaded { get; set; }
        }
    }
}