using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Model.Scheduling
{
    [Table("WMS_ItemInventory", Schema = "ONEPROD")]
    public class ItemInventory : IModelEntity
    {
        public int Id { get; set; }

        public virtual ItemOP Item { get; set; }
        public int ItemId { get; set; }

        public int StockCalculated { get; set; }
        public int Stock { get; set; }
        public int ScrapQty { get; set; }

        public int UserId { get; set; }
        public DateTime ReportDate { get; set; }
        public DateTime TimeStamp  { get; set; }

        public bool IsScrap { get; set; }
        public bool isScrapApplied { get; set; }
        public bool isStockApplied { get; set; }


        public override string ToString()
        {
            return "[" + Item.Id + "] " + Item.Code + ": " + Stock.ToString();
        }
    }
}
