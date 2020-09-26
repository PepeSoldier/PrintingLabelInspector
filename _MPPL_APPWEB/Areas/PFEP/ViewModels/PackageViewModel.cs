using MDL_iLOGIS.ComponentConfig.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.PFEP.ViewModels
{
    public class PackageViewModel
    {
        public Package Package { get; set; }
        public IQueryable<Package> Packages { get; set; }

    }
}