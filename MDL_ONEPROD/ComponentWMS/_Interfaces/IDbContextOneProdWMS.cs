using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentWMS._Interfaces
{
    public interface IDbContextOneprodWMS : IDbContextOneprod, IDbContextiLOGIS
    {
        DbSet<BufforLog> BufforLog { get; set; }
    }
}