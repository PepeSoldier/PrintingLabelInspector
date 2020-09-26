using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MDL_BASE.Interfaces;

namespace MDL_PRD.Model
{
    [Table("PSI_OrderArchive", Schema = "PRD")]
    public class OrderArchiveModel : IModelEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        [MaxLength(5)]
        public string Line { get; set; }
        [MaxLength(10)]
        public string OrderNo { get; set; }
        [MaxLength(9)]
        public string PNC { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int QtyPlanned { get; set; }
        public int QtyRemain { get; set; }

        public DateTime FreezeDate { get; set; }
        [MaxLength(3)]
        public string Type { get; set; }
        public DateTime FirstScanTime { get; set; }

        public virtual ReasonModel Reason { get; set; }
        [MaxLength(150)]
        public string ReasonDetails { get; set; }

        [NotMapped]
        public int ProdSeq { get; set; }
        [NotMapped]
        public int ProdSeqTmp { get; set; }
        [NotMapped]
        public string BackColor { get; set; }
        [NotMapped]
        public int WArch { get; set; }
        [NotMapped]
        public int W { get; set; }
        [NotMapped]
        public int Seq { get; set; }

        [MaxLength(255)]
        public string CommentText { get; set; }
        [MaxLength(30)]
        public string CommentAnc { get; set; }
        [MaxLength(30)]
        public string CommentSupplier { get; set; }
        
        public override string ToString()
        {
            return OrderNo.ToString();
        }

    }
}
