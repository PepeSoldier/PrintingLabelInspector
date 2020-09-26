using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_AP.Models.Reports
{
    public class MainReport
    {
        public int id { get; set; }
        public int value { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public string hoverColor { get; set; }
        public string chartType { get; set; }
        public string[] Colors
        {
            get
            {
                return colors;
            }
        } 
        private string[] colors = new string[] { "#F65D4C", "#F8AE53", "#204F61", "#1F8585", "#2F3585", "#3F9555" };

    }
}