using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_OTHER.ComponentPickByLight.Entities
{
    [Table("PICKBYLIGHT_InstanceElement", Schema = "OTHER")]
    public class PickByLightInstanceElement : IModelEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string ItemCode { get; set; }
        public string PLCMemoryAdress { get; set; }
        public bool Value { get; set; }

        public DateTime LastChange { get; set; }
        public string UserName { get; set; }

        public PickByLightInstance PickByLightInstance { get; set; }
        public int PickByLightInstanceId { get; set; }

    }
}
