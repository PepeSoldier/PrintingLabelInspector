using MDL_ONEPROD.ComponentMes.Etities;
using MDL_ONEPROD.ComponentMes.Etities.MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.ComponentQuality.Entities;
using MDL_ONEPROD.ComponetMes.Entities;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_ONEPROD.Interface
{
    public interface IDbContextOneprodMes : IDbContextOneprod
    {
        DbSet<Workplace> Workplaces { get; set; }
        DbSet<WorkplaceBuffer> WorkplaceBuffers { get; set; }
        DbSet<ProductionLog> ProductionLogs { get; set; }
        DbSet<ProductionLogTraceability> ProductionLogTraceabilities { get; set; }
        DbSet<ProductionLogRawMaterial> ProductionLogRawMaterials { get; set; }
    }
}
