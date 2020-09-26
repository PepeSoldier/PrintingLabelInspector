using MDL_BASE.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XLIB_COMMON.Enums;

namespace MDL_iLOGIS.ComponentConfig.Entities
{
    [Table("CONFIG_Package", Schema = "iLOGIS")]
    public class Package : IModelDeletableEntity, IDefModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Nazwa")]
        public string Name { get; set; }

        [DisplayName("Kod Opakowania")] //Nr.SAP
        public string Code { get; set; }

        [Display(Name="Cena")]
        public decimal UnitPrice { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }
        public EnumPackageType Type { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }
        public decimal Weight { get; set; }

        public int PackagesPerPallet { get; set; }
        public int FullPalletHeight { get; set; }

        public bool Returnable { get; set; }
        public bool Deleted { get; set; }
        
        //public User Creator { get; set; }       //tworzy foreign key
        //public string CreatorId { get; set; }   //pole nieobowiazkowe ale ulatwia wiazanie i pobieranie danych. nie trzeba podawac obiektu, wystarczy ID
    }
}