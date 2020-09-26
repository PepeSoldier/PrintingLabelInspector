using MDL_BASE.Enums;
using MDL_BASE.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MDL_PFEP.Models.DEF
{
    //[Table("DEF_Package", Schema = "PFEP")]
    //public class Package : IDefModel
    //{
    //    [Key]
    //    public int Id { get; set; }

    //    [Required]
    //    [StringLength(100)]
    //    [DisplayName("Nazwa")]
    //    public string Name { get; set; }

    //    [DisplayName("Kod Opakowania")] //Nr.SAP
    //    public string Code { get; set; }

    //    [Display(Name="Cena")]
    //    public decimal UnitPrice { get; set; }

    //    public UnitOfMeasure UnitOfMeasure { get; set; }

    //    public bool Deleted { get; set; }
        
    //    //public User Creator { get; set; }       //tworzy foreign key
    //    //public string CreatorId { get; set; }   //pole nieobowiazkowe ale ulatwia wiazanie i pobieranie danych. nie trzeba podawac obiektu, wystarczy ID
    //}
}