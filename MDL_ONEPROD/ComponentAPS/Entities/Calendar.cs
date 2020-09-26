using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Model.Scheduling
{
    [Table("APS_Calendar", Schema = "ONEPROD")]
    public class Calendar2 : IModelEntity
    {
        public int Id { get; set; }

        public virtual ResourceOP Machine { get; set; }
        public int MachineId { get; set; }

        public DateTime DateClose { get; set; }
        public DateTime DateOpen { get; set; }


        public DateTime Date { get; set; }
        public int Hours { get; set; }
        public int MaxQty { get; set; }
        public int MaxCycleTime { get; set; }
        public decimal Efficiency { get; set; }
    }
}
