using XLIB_COMMON.Repo;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using System.Collections.Generic;
using System.Linq;
using XLIB_COMMON.Interface;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDLX_MASTERDATA.Entities;
using System.Net;
using Newtonsoft.Json.Linq;
using System;
using MDL_iLOGIS.ComponentWMS.Models;
using MDL_iLOGIS.ComponentWMS._Interfaces;

namespace MDL_iLOGIS.ComponentWMS.Repos
{
    public class StockUnitRepo_FAKE : ILocataionManager
    {
        public bool IsFake { get; set; }

        public StockUnitRepo_FAKE()
        {
            IsFake = true;
        }

        public List<ApiLocation> GetLocationsOfItem(string itemCode, int pageSize = 10, int pageNumber = 0)
        {
            return new List<ApiLocation>();
        }
    }
}