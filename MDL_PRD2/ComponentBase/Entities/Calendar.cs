using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MDL_PRD.Entity
{
    [Table("CALENDAR", Schema = "PRD")]
    public class Calendar : IModelEntity
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string LineName { get; set; }
        
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int Hours { get; set; }
        public int Quantity { get; set; }
        public decimal CycleTime { get; set; }

        public decimal CycleTime1 {
            get
            {
                return (decimal)((EndTime-StartTime).TotalMinutes * 60) / (decimal)Quantity;
            }
        }


    }
}