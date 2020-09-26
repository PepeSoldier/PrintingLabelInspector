using MDL_PFEP.Model.ELDISY_PFEP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_PFEP.ViewModel
{
    public class CorrectionViewModel
    {
        public CorrectionViewModel()
        {
            this.Correction = new Correction();
        }

        public Correction Correction { get; set; }
    }
}