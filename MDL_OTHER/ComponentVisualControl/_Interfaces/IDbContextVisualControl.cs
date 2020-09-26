using MDL_OTHER.ComponentHSE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MDL_BASE.Interfaces;
using MDL_OTHER.ComponentVisualControl.Entities;

namespace MDL_OTHER.ComponentHSE._Interfaces
{
    public interface IDbContextVisualControl : IDbContextCore
    {
        DbSet<JobItemConfig> JobItemConfigs { get; set; }
    }
}
