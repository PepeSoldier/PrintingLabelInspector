using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MDL_iLOGIS.ComponentConfig.Entities
{
    [Table("CONFIG_Transporter", Schema = "iLOGIS")]
    public class Transporter : IModelEntity, IModelDeletableEntity
    {
        public int Id { get; set; }
        [MaxLength(25)]
        public string Name { get; set; }
        [MaxLength(25)]
        public string Code { get; set; }

        public string DedicatedResources { get; set; }

        public bool Deleted { get; set; }

        public string ConnectedTransporters { get; set; }

        public EnumTransporterType Type { get; set; }

        public int LoopQty { get; set; }
        public string ConnectedStatus { get; set; }

        public List<int> ConnectedTransportersArray {
            get
            {
               return ConnectedTransporters != null? ConnectedTransporters.Split(',').Select(Int32.Parse).ToList() : new List<int>();
            }
        }
        public string[] DedicatedResourcesArray
        {
            get
            {
                return DedicatedResources != null? DedicatedResources.Split(',') : new string[0];
            }
        }


    }
}
