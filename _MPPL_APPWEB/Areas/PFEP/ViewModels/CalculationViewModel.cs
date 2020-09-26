using MDL_PFEP.Model.ELDISY_PFEP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_PFEP.ViewModel
{
    public class CalculationViewModel
    {
        public Calculation NewObject { get; set; }
        public List<Calculation> calculations { get; set; }
    }
}