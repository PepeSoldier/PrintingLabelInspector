using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_PFEP.Model
{
    public class PrintOrderModel
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public string OrderNo { get; set; }
        public string LineName { get; set; }
        public string PNCCode { get; set; }
        public int Qty { get; set; }
        public int QtyOrder { get; set; }
        public int QtyPrinted { get; set; }
        public bool Printed { get; set; }
        public int RoutineId { get; set; }
    }
}