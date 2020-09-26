using MDL_CORE.ComponentCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentRTV.Models
{
    public class RTVOEEProductionDataParameterModel
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }

        public EnumVariableType DataType { get; set; }
        
        public string Name { get; set; }
        public string Target { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }

        public DateTime Date { get; set; }
        public string Value { get; set; }
    }
}