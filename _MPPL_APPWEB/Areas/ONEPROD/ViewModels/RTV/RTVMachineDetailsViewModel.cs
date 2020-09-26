using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.ViewModels.RTV
{
    public class RTVMachineDetailsViewModel
    {
        public int ProducedQty { get; set; }
        public decimal CycleTime { get; set; }
        public string PartCode { get; set; }
        public string ProgramName { get; set; }
        public int ProgramNo { get; set; }

        public List<RTVPirometersViewModel> Pirometers { get; set; }
    }
    public class RTVPirometersViewModel
    {
        public decimal? PirometerTemp { get; set; }
        public decimal? PirometerTempMin { get; set; }
        public decimal? PirometerTempMax { get; set; }
    }
}