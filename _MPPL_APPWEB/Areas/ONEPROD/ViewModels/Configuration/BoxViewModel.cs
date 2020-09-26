using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas.ONEPROD.Base.ViewModels
{
    public class BoxViewModel
    {
        public IQueryable<Warehouse> Boxes { get; set; }
        public Warehouse NewObject { get; set; }
    }
}