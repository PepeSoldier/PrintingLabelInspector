using MDL_BASE.Interfaces;

using MDL_BASE.Models.MasterData;
using MDL_PFEP.Models.DEF;
using MDLX_MASTERDATA.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_PFEP.Models.PFEP
{
    //[Table("AncWorkstation", Schema = "PFEP")]
    //public class WorkstationItem : IModelEntity
    //{
    //    [Key]
    //    public int Id { get; set; }


    //    public virtual Item Item { get; set; }
    //    [Required]
    //    [DisplayName("ANC")]
    //    public int ItemId { get; set; }

    //    public virtual Workstation Workstation { get; set; }
    //    [Required]
    //    [DisplayName("Stanowisko")]
    //    public int WorkstationId { get; set; }

    //    [Required]
    //    [Range(0, Int32.MaxValue)]
    //    [DisplayName("Liczba opakowań")]
    //    public int MaxPackages { get; set; }

    //    [Required]
    //    [Range(0, Int32.MaxValue)]
    //    [DisplayName("Sztuk w BOM")]
    //    public int MaxBomQty { get; set; }

    //    public bool CheckOnly { get; set; }

    //    //public virtual FeederType FeederType { get; set; }
    //    //[Required]
    //    //[DisplayName("Typ podajnika")]
    //    //public int FeederTypeId { get; set; }

    //    //public virtual BufferType BufferType { get; set; }
    //    //[Required]
    //    //[DisplayName("Rodzaj buforu")]
    //    //public int BufferTypeId { get; set; }

    //    //[NotMapped]
    //    //public int Reserve
    //    //{
    //    //    get { return MaxPackages - 1; }
    //    //}
    //}
}