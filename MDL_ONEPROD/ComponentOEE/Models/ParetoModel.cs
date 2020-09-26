using MDL_ONEPROD.Model.OEEProd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentOEE.Models
{
    public class ParetoModel
    {
        public string Name { get; set; }
        public string NameEnglish { get; set; }
        public string Color { get; set; }
        public decimal Value { get; set; }
        public int EntryType { get; set; }

        public string GetName(int languageId = 1)
        {
            return languageId == 1 ? Name : NameEnglish;
        }

    }
}