using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_PFEP.Models.DEF;
using MDLX_MASTERDATA.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MDL_PFEP.Models.PFEP
{
    //[Table("AncPackage", Schema = "PFEP")]
    //public class PackageItem : IModelEntity
    //{
    //    [Key]
    //    public int Id { get; set; }

    //    public virtual Item Item { get; set; }
    //    [Required]
    //    [DisplayName("ANC")]
    //    public int ItemId { get; set; }

    //    public virtual Package Package { get; set; }
    //    [Required]
    //    [DisplayName("Opakowanie")]
    //    public int PackageId { get; set; }

    //    [Required]
    //    [DisplayName("Zwrotne")]
    //    public bool Returnable { get; set; }

    //    [Required]
    //    [Range(0, Int32.MaxValue)]
    //    [DisplayName("Ilość w opakowaniu")]
    //    public decimal QtyPerPackage { get; set; }

    //    [Range(0, Int32.MaxValue)]
    //    [DisplayName("Długość (w) [mm]")]
    //    public int Width { get; set; }

    //    [Range(0, Int32.MaxValue)]
    //    [DisplayName("Szerokość (d) [mm]")]
    //    public int Depth { get; set; }

    //    [Range(0, Int32.MaxValue)]
    //    [DisplayName("Wysokość (h) [mm]")]
    //    public int Height { get; set; }
        
    //    [Range(0, Int32.MaxValue)]
    //    [DisplayName("Waga opakowania brutto [kg]")]
    //    public decimal Weight { get; set; }

    //    [Range(0, Int32.MaxValue)]
    //    [DisplayName("Waga opakowania netto [kg]")]
    //    public decimal NetWeight { get; set; }

    //    [Range(0, Int32.MaxValue)]
    //    [DisplayName("Liczba opakowań na palecie")]
    //    public int PackagesPerPallet { get; set; }

    //    [Required]
    //    [DisplayName("Piętrowanie")]
    //    public bool Stackable { get; set; }

    //    [DisplayName("Karta pakowa")]
    //    public bool PackagingCard { get; set; }

    //    [DisplayName("Karta pakowa (plik)")]
    //    public string PackagingCardFile { get; set; }
    //}
}