using MDL_BASE.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_LABELINSP.Entities
{
    [Table("WorkorderLabel", Schema = "LABELINSP")]
    public class WorkorderLabel : IModelEntity
    {
        public WorkorderLabel()
        {
            //WorkorderLabelInspections = new List<WorkorderLabelInspection>();
        }

        public int Id { get; set; }

        public virtual Workorder Workorder { get; set; }
        public int WorkorderId { get; set; }

        //[MaxLength(10)]
        //public string OrderNo { get; set; }

        //[MaxLength(50)]
        //public string ItemCode { get; set; }

        //[MaxLength(50)]
        //public string ItemName { get; set; }

        [MaxLength(50)]
        public string SerialNumber { get; set; }

        public DateTime TimeStamp { get; set; }

        //public virtual ICollection<WorkorderLabelInspection> WorkorderLabelInspections { get; set; }

        public override string ToString()
        {
            return Workorder.WorkorderNumber.ToString() + ". " + SerialNumber; 
        }
    }
}