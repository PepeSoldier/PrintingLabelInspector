using MDL_BASE.Models.MasterData;
using MDL_PFEP.ComponentLineFeed.Models;
using MDL_PFEP.Interface;
using MDL_PFEP.Models.DEF;
using System.Collections.Generic;
using System.Linq;

namespace MDL_PFEP.Repo.PFEP
{
    public class JobListRepo : XLIB_COMMON.Repo.Base.JobListRepo
    {
        IDbContextPFEP db;

        public JobListRepo(IDbContextPFEP db) : base(db)
        {
            this.db = db;
        }

        //public List<PrintModel_ANC> GetAncsByPNCAndRoutine(string workorderNumber, Routine routine)
        //{
        //    routine.AddPrefixes = routine.AddPrefixes == null ? new string[0] { } : routine.AddPrefixes;

        //    List<JobList> boms = db.JobLists.AsNoTracking().Where(x => 
        //                              x.OrderNo == workorderNumber &&
        //                              routine.DEFs.Contains(x.DEF) &&
        //                              (x.LV == 1 || x.LV == routine.AddLevel || (routine.AddPrefixes.Contains(x.Prefix)))
        //                           ).ToList();

        //    List<PrintModel_ANC> ancs = new List<PrintModel_ANC>();

        //    foreach(JobList b in boms)
        //    {
        //        ancs.Add(new PrintModel_ANC(b.Anc, b.ANCQty));
        //    }

        //    return ancs;
        //}
    }
}