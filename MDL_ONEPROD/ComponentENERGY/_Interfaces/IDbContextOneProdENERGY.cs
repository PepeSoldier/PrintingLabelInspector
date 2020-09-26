using MDL_ONEPROD.ComponentENERGY.Entities;
using MDL_ONEPROD.Interface;
using System.Data.Entity;

namespace MDL_ONEPROD.ComponentENERGY
{
    public interface IDbContextOneProdENERGY : IDbContextOneprod
    {
        DbSet<EnergyMeter> EnergyMeters { get; set; }
        DbSet<EnergyCost> EnergyCosts { get; set; }
        DbSet<EnergyConsumptionData> EnergyConsumptionDatas { get; set; }
    }
}
