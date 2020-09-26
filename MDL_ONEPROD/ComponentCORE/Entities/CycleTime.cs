using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MDL_ONEPROD.Model.Scheduling
{
    [Table("CORE_CycleTime", Schema = "ONEPROD")]
    public class MCycleTime : IModelEntity
    {
        public int Id { get; set; }

        public virtual ResourceOP Machine { get; set; }
        public int MachineId { get; set; }

        public virtual ItemOP ItemGroup {get; set; }
        public int ItemGroupId { get; set; }

        //public virtual Item Item {get; set; }
        //public int ItemId { get; set; }

        //public virtual Process Process { get; set; }
        //public int? ProcessId { get; set; }

        public bool Preferred { get; set; }

        public bool Active { get; set; }

        [DataType("decimal(18,2")]
        public decimal CycleTime { get; set; }
        public int ProgramNumber { get; set; }
        [MaxLength(50)]
        public string ProgramName { get; set; }

        public int PiecesPerPallet { get; set; }
    }
}
