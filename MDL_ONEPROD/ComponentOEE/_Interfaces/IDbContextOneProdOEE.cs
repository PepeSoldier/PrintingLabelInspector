using MDL_ONEPROD.Model.OEEProd;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL_ONEPROD.Interface
{
    public interface IDbContextOneProdOEE : IDbContextOneprod
    {
        DbSet<OEEReport> OEEReports { get; set; }
        DbSet<OEEReportEmployee> OEEReportEmployees { get; set; }
        DbSet<OEEReportProductionData> OEEReportProductionData { get; set; }
        DbSet<OEEReportProductionDataDetails> OEEReportProductionDataDetails { get; set; }
        DbSet<Reason> Reasons { get; set; }
        DbSet<ReasonType> ReasonTypes { get; set; }
        DbSet<MachineReason> MachineReasons { get; set; }
        DbSet<MachineTarget> MachineTargets { get; set; }
    }
}
