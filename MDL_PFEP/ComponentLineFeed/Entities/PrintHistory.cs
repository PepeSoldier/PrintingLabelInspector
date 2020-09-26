using MDL_BASE.Interfaces;
using MDL_BASE.Models.Base;
using MDL_BASE.Models.IDENTITY;
using MDL_PRD.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_PFEP.Model.PFEP
{
    [Table("PrintHistory", Schema = "PFEP")]
    public class PrintHistory : IModelEntity
    {
        public int Id { get; set; }
        public int Printnumber { get; set; }
        public DateTime PrintDate { get; set; }

        public virtual User User { get; set; }
        public string UserId { get; set; }

        public virtual Prodorder20 Order20 {get;set;}
        public int Order20Id { get; set; }

        public int RoutineId { get; set; }
    }
}