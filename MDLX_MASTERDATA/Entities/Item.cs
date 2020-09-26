using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using MDL_BASE.Interfaces;
using MDLX_MASTERDATA.Enums;
using XLIB_COMMON.Enums;

namespace MDLX_MASTERDATA.Entities
{
    [Table("MASTERDATA_Item")]
    public class Item : IModelEntity, IModelDeletableEntity
    {
        public Item() {
            this.StartDate = DateTime.Now;
            this.CreatedDate = DateTime.Now;
        }

        public int Id { get; set; }
        [MaxLength(100)] //, Index(IsUnique = true)]
        public string Code { get; set; }
        [MaxLength(100)]
        public string OriginalName { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Comment { get; set; }

        [MaxLength(25)]
        public string Color1 {get;set;}
        [MaxLength(25)]
        public string Color2 {get;set;}
        [MaxLength(25)]
        public string Specific1 { get; set; }
        [MaxLength(25)]
        public string Specific2 { get; set; }
        [MaxLength(25)]
        public string Specific3 { get; set; }
        [MaxLength(25)]
        public string Specific4 { get; set; }

        [MaxLength(25)]
        public string DEF { get; set; }
        [MaxLength(25)]
        public string BC { get; set; }
        [MaxLength(25)]
        public string PREFIX { get; set; }
        
        public int Width { get; set; }
        public int Height { get; set; }
        
        public ItemTypeEnum Type { get; set; }

        //public virtual ResourceGroup Area { get; set; }
        //public int? AreaId { get; set; }

        public virtual Process Process { get; set; }
        public int? ProcessId { get; set; }

        public virtual Item ItemGroup { get; set; }
        public int? ItemGroupId { get; set; }

        //public virtual Item Group { get; set; }
        //public int? GroupId { get; set; }
        public virtual Resource2 ResourceGroup { get; set; }
        public int? ResourceGroupId { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public bool Deleted { get; set; }

        //Z ItemGroup:
        public int? Id_old { get; set; }


        [MaxLength(25)]
        public string Color { get; set; }

        //Nowe Pola
        [DisplayName("Nowy")]
        public bool New { get; set; }
        public decimal Lenght { get; set; } //[mb] 
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsCommon { get; set; }
        public int Old_Id { get; set; }

        public virtual ICollection<ItemUoM> UnitOfMeasures { get; set; }

        [NotMapped]
        public bool Used { get; set; }

        public override string ToString()
        {
            return GetName;
        }       

        [NotMapped]
        public string GetName { get { return Name != null ? Name : OriginalName; } }
        [NotMapped]
        public string GetCodeAndName { get { return Name != null ? Code + " - " + Name : Code + " - " + OriginalName; } }
    }

}
