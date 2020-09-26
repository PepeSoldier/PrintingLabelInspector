using MDL_ONEPROD.ComponentRTV.Entities;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.OEEProd;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentRTV._Interfaces
{
    public interface IDbContextOneProdRTV : IDbContextOneprod
    {
        DbSet<RTVOEEProductionData> RTVOEEProductionData { get; set; }
        DbSet<RTVOEEProductionDataDetails> RTVOEEProductionDataDetails { get; set; }
        DbSet<RTVOEEProductionDataParameter> RTVOEEProductionDataParameters { get; set; }
        DbSet<RTVOEEParameter> RTVOEEParameters { get; set; }
        DbSet<RTVOEEPLCData> RTVOEEPLCData { get; set; }
        DbSet<ReasonType> ReasonTypes { get; set; }
    }
}
