using MDL_BASE.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_OTHER.ComponentPickByLight.Entities
{
    [Table("PICKBYLIGHT_Instance", Schema = "OTHER")]
    public class PickByLightInstance : IModelEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string PLCDriverIPAdress { get; set; }
        public string TCPPort { get; set; }

        public DateTime LastChange { get; set; }
        public string UserName { get; set; }
    }
}
