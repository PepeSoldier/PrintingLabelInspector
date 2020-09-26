using MDL_ONEPROD.ComponentQuality.Entities;
using MDL_ONEPROD.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_ONEPROD.ComponentQuality._Interfaces
{
    public interface IDbContextOneprodQuality : IDbContextOneprod
    {
        DbSet<ItemParameter> ItemParameters { get; set; }
        DbSet<ItemMeasurement> ItemMeasurements { get; set; }
    }
}
