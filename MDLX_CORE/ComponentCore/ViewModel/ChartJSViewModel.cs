using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_BASE.ViewModel
{
    public class ChartJSViewModel
    {
        public ChartJSViewModel()
        {
            Labels = new List<string>();
            Data = new List<int>();
        }
        public string Label { get; set; }
        public List<string> Labels { get; set; }
        public List<int> Data { get; set; }
    }
}