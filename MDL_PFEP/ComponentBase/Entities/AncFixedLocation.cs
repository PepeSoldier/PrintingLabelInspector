using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_PFEP.Model.PFEP
{
    [Table("AncFixedLocation", Schema = "PFEP")]
    public class AncFixedLocation : IModelEntity
    {
        public int Id { get; set; }
        public Item Anc { get; set; }
        public int AncId { get; set; }
        public string FixedLocation { get; set; }
    }
}