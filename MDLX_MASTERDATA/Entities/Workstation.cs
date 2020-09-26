using MDL_BASE.Interfaces;
using MDLX_MASTERDATA.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_BASE.Models.MasterData
{
    [Table("MASTERDATA_Workstation")]
    public class Workstation : IModelEntity, IDefComboModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Nazwa Stanowiska")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Kolejność")]
        public int SortOrder { get; set; }

        [DisplayName("Kolejność Pociąg")]
        public int SortOrderTrain { get; set; }

        [DisplayName("Nadpisz FlowRack L")]
        public int FlowRackLOverride { get; set; }

        public virtual Resource2 Line { get; set; }
        [DisplayName("Linia")]
        public int? LineId { get; set; }

        public Area Area { get; set; }
        public int? AreaId { get; set; }

        [DefaultValue(false)]
        [DisplayName("Usunięty")]
        public bool Deleted { get; set; }

        public int ProductsFromIn { get; set; }
        public int ProductsFromOut { get; set; }

        //public virtual PncType Type { get; set; }
        //[DisplayName("Typ")]
        //public int? TypeId { get; set; }
    }
}