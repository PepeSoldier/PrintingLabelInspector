using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_CORE.ComponentCore.Entities
{
    public class JsonUpdateGridModel
    {
        public JsonUpdateGridModel()
        {
            this.Error = false;
        }
        public object Item { get; set; }
        public string Message { get; set; }
        public bool Error { get; set; }
    }
}