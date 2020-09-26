using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentMes.Models
{
    public class TreantJsNode
    {
        public TreantJsNode()
        {
            children = new List<TreantJsNode>();
            text = new TreantJsNodeText();
        }

        public string image { get; set; }
        public TreantJsNodeText text { get; set; }
        public List<TreantJsNode> children { get; set; }
    }
    
    public class TreantJsNodeText
    {
        public string name { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public string prodLogId { get; set; }
        public string declaredQty { get; set; }
        public string workOrderTotalQty { get; set; }
        //public string picture { get; set; }
        public string datetime { get; set; }
        public string serialNumber { get; set; }

    }
}