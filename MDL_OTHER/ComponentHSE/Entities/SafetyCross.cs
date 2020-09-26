using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_OTHER.ComponentHSE.Entities
{
    [Table("HSE_SafetyCross")]
    public class SafetyCross : IModelEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public SafetyCrossState State { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual User User { get; set; }
        public string UserId { get; set; }

    }
}
