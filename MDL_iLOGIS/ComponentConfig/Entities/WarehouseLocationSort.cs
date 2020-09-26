using MDL_BASE.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDL_iLOGIS.ComponentConfig.Entities
{
    [Table("CONFIG_WarehouseLocationSort", Schema = "iLOGIS")]
    public class WarehouseLocationSort : IModelEntity, IModelDeletableEntity
    {
        public WarehouseLocationSort()
        {

        }
        public int Id { get; set; }
        public string RegalNumber { get; set; } 
        public int SortOrder { get; set; } 
        public int SortColumnAscending { get; set; }
        
        public bool Deleted { get; set; }
    }
    
}