using MDL_ONEPROD.ComponentRTV.Models;
using MDL_ONEPROD.Model.OEEProd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentMes.ViewModels
{
    public class ReportOnlineViewModel
    {
        public OEEReport Report { get; set; }
        public List<RTVOEEProductionItem> RegisteredProduction { get; set; }
        public List<OEEReportProductionData> DeclaredProduction { get; set; }
        
        public List<RTVOEEProductionItem> RegisteredStoppages { get; set; }
        public List<OEEReportProductionData> DeclaredStoppages { get; set; }
    }
}