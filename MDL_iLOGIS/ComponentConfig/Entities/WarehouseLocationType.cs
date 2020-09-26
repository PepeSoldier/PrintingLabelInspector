using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MDL_iLOGIS.ComponentConfig.Entities
{
    [Table("CONFIG_WarehouseLocationType", Schema = "iLOGIS")]
    public class WarehouseLocationType : IModelEntity, IModelDeletableEntity
    {
        public WarehouseLocationType()
        {
        }

        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string DisplayFormat { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }

        public decimal MaxWeight { get; set; }

        public WarehouseLocationTypeEnum TypeEnum { get; set; }

        public bool Deleted { get; set; }

    }
    
}