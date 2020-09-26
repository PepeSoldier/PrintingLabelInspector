using MDL_BASE.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_BASE.Models.MasterData
{
    [Table("MASTERDATA_Department")]
    public class Department : IModelEntity, IDefModel
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Display(Name = "Nazwa Działu")]
        public string Name { get; set; }

        public bool Deleted { get; set; }
    }
}