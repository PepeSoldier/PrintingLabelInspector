using MDL_BASE.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XLIB_COMMON.Enums;

namespace MDL_CORE.ComponentCore.Entities
{
    [Table("Printer", Schema = "CORE")]
    public class Printer : IModelEntity
    {
        public int Id { get; set; }

        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string User { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }

        [MaxLength(50), Index(IsUnique = true), Required]
        public string IpAdress { get; set; }

        [MaxLength(150)]
        public string Model { get; set; }

        [MaxLength(150)]
        public string SerialNumber { get; set; }
        
        [Required]
        public PrinterType PrinterType { get; set; }
    }
}