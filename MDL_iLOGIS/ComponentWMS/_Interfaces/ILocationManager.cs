using MDL_iLOGIS.ComponentWMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_iLOGIS.ComponentWMS._Interfaces
{
    public interface ILocataionManager
    {
        bool IsFake { get; set; }
        List<ApiLocation> GetLocationsOfItem(string itemCode, int pageSize = 10, int pageNumber = 0);
    }
}
