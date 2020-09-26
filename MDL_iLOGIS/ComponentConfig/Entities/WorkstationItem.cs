using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentConfig.Entities
{
    [Table("CONFIG_WorkstationItem", Schema = "iLOGIS")]
    public class WorkstationItem : IModelEntity
    {
        public int Id { get; set; }

        public virtual Workstation Workstation { get; set; }
        [Required, DisplayName("Stanowisko")]
        public int WorkstationId { get; set; }

        public virtual ItemWMS ItemWMS { get; set; }
        [Required, DisplayName("Item")]
        public int ItemWMSId { get; set; }
        
        [Required, Range(0, Int32.MaxValue), DisplayName("Max Liczba opakowań")]
        public int MaxPackages { get; set; }

        [Required, Range(0, Int32.MaxValue), DisplayName("Zapas Bezpieczeństwa")]
        public int SafetyStock { get; set; }

        [Required, Range(0, Int32.MaxValue), DisplayName("Sztuk w BOM")]
        public int MaxBomQty { get; set; }

        public string PutTo { get; set; }

        public bool CheckOnly { get; set; }
    }
}
