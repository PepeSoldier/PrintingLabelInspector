using MDL_BASE.Interfaces;
using MDL_ONEPROD.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Model.Scheduling
{
    [Table("CORE_Workorder", Schema = "ONEPROD")]
    public class Workorder : IModelEntity
    {
        public Workorder()
        {
            //Batches = "";
            Batches = new WorkorderBatch();
        }

        public int Id { get; set; }
        [MaxLength(100)]
        public string UniqueNumber { get; set; }

        public virtual Workorder ParentWorkorder { get; set; }
        public int? ParentWorkorderId { get; set; }

        public virtual ClientOrder ClientOrder { get; set; }
        public int? ClientOrderId { get; set; }
                
        public virtual ItemOP Item { get; set; }
        public int? ItemId { get; set; }

        public virtual ResourceOP Resource { get; set; }
        public int? ResourceId { get; set; }

        public virtual Tool Tool { get; set; }
        public int? ToolId { get; set; }

        public int LV { get; set; }

        public DateTime ReleaseDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }


        public int Qty_Total { get; set; }
        public int Qty_Produced { get; set; }
        public int Qty_Used { get; set; }
        public int Qty_Scrap { get; set; }
        public int Qty_ControlLabel { get; set; }


        public int ProcessingTime { get; set; }
        public int OrderSeq { get; set; }
        public int BatchNumber { get; set; }
        //public int LV { get; set; }
        [DataType("decimal(18,2")]
        public double Index { get; set; }
        public TaskScheduleStatus Status { get; set; }

        public int Param1 { get; set; }
        public int Param2 { get; set; }

        [NotMapped]
        public int Qty_Remain { get { return Math.Max(Qty_Total - Math.Max(Qty_Produced, Qty_Used), 0); } }
        [NotMapped]
        public int Weight { get; set; }
        [NotMapped]
        public WorkorderBatch Batches { get; set; }
        //[NotMapped]
        //public double MarginRight { get; set; }
        //[NotMapped]
        //public double MarginLeft { get; set; }
        //[NotMapped]
        //public string MarginLeftStr { get { return MarginLeft.ToString("0.0").Replace(',','.'); } }
        //[NotMapped]
        //public double Width { get; set; }
        //[NotMapped]
        //public string WidthStr { get { return Width.ToString("0.0").Replace(',', '.'); } }
        //[NotMapped]
        //public string BackgroundColor { get; set; }
        //[NotMapped]
        //public string SpecialCssClass { get; set; }
        //[NotMapped]
        //public string FontColor { get; set; }

        public Workorder Clone()
        {
            return new Workorder
            {
                //Batches = this.Batches,
                DueDate = this.DueDate,
                EndTime = this.EndTime,
                Id = this.Id,
                Index = this.Index,
                //LV = this.LV,
                Resource = this.Resource,
                ResourceId = this.ResourceId,
                ClientOrder = this.ClientOrder,
                ClientOrderId = this.ClientOrderId,
                OrderSeq = this.OrderSeq,
                Item = this.Item,
                ItemId = this.ItemId,
                //Planned = this.Planned,
                ProcessingTime = this.ProcessingTime,
                Qty_Total = this.Qty_Total,
                Qty_Produced = this.Qty_Produced,
                Qty_Used = this.Qty_Used,
                ReleaseDate = this.ReleaseDate,
                StartTime = this.StartTime,
                Status = this.Status,
                Tool = this.Tool,
                ToolId = this.ToolId,
                UniqueNumber = this.UniqueNumber,
                Weight = this.Weight
            };
        }

        public override string ToString()
        {
            return Id.ToString() + ". " + Item.Code + "-" + Item.Name;
        }

    }



    public enum TaskScheduleStatus
    {
        initial = 0,
        batched = 5,
        covered = 10,
        partiallyCovered = 15,
        planned = 50,
        ready = 55, //buffered
        inProduction = 60,
        produced = 70,
        inUse = 80,
        used = 90
    }



}
