using MDL_BASE.Interfaces;
using MDL_OTHER.ComponentPickByLight.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_OTHER.ComponentPickByLight._Interfaces
{
    public interface IDbContextPickByLight : IDbContextCore
    {
        DbSet<PickByLightInstance> PickByLightInstances { get; set; }
        DbSet<PickByLightInstanceElement> PickByLightInstanceElements { get; set; }
    }
}
