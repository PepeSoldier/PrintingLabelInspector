using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentWHDOC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWHDOC.Models
{
    public class ContractorWithDocumentItems
    {
        public ContractorWithDocumentItems()
        {
            WhDocumentItemLightList = new List<WhDocumentItemLight>();
        }
        public Contractor Contractor { get; set; }
        public List<WhDocumentItemLight> WhDocumentItemLightList { get; set; }
    }
}
