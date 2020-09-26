using MDL_BASE.Interfaces;
using MDL_BASE.Models.Base;
using MDLX_CORE.ComponentCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_PRD.Model
{
    [Table("ProdOrder_Sequence", Schema = "PRD")]
    public class ProdOrderSequence : IModelEntity
    {
        public int Id { get; set; }

        public virtual ProductionOrder Order { get; set; }
        public int OrderId { get; set; }

        public int OriginalSequence { get; set; }
        public DateTime OriginalStartDate { get; set; }
        [MaxLength(50)]
        public string OriginalLineName { get; set; }
        public int OriginalLineId { get; set; }

        public int SnapshotNo { get; set; }
        public int SnapshotSeq { get; set; }
        [MaxLength(50)]
        public string SnapshotLineName { get; set; }
        public DateTime SnapshotStartDate { get; set; }

        public DateTime CreationDate { get; set; }
        public string CreatorUserName { get; set; }

        public bool Applied { get; set; }
        public bool Active { get; set; }

        //[MaxLength(50)]
        //public string Info { get; set; }
    }

    public class ProdOrderSeqGrid
    {
        public int OrderId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Line { get; set; }
        public string OrderNo { get; set; }
        public string PNC { get; set; }
        public int QtyPlanned { get; set; }
        public int QtyRemain { get; set; }
        public int QtyProducedInPast { get; set; }
        public string Notice { get; set; }
        public int SeqTemp { get; set; }
        public int SeqOriginal { get; set; }
        public bool IsSeqChenged { get; set; }
        public DateTime? FirstProductIn { get; set; }
        public int StateA { get; set; }
        public int StateB { get; set; }
        public int ResourceGroupId { get; set; }

    }
    public class SnapshotInfo
    {
        public int SnapshotNo { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatorUserName { get; set; }
        public string LineName { get; set; }
        public int TotalChanges { get; set; }
    }
}