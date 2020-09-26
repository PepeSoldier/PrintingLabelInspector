using MDL_BASE.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_BASE.Models.MasterData
{
    [Table("MASTERDATA_Area")]
    public class Area : IModelEntity, IDefModel
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Display(Name = "Nazwa Obszaru")]
        public string Name { get; set; }

        public bool Deleted { get; set; }
    }
}