//using MDL_ONEPROD.Model.Scheduling;
using MDLX_MASTERDATA._Interfaces;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;

namespace MDLX_MASTERDATA.Repos
{
    public class ProcessRepo : RepoGenericAbstract<Process>
    {
        protected new IDbContextMasterData db;
        public ProcessRepo(IDbContextMasterData db, IAlertManager alertManager) : base(db)
        {
            this.db = db;
        }

        public override Process GetById(int id)
        {
            return db.Processes.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<Process> GetList()
        {
            return db.Processes.Where(x=>x.Deleted == false).OrderBy(x => x.Name);
        }
        public IQueryable<Process> GetList(Process item)
        {
            return db.Processes.Where(x => (x.Deleted == false) &&
                                              (x.Name.StartsWith(item.Name) || item.Name == null) &&
                                              (x.ParentId == item.ParentId || item.ParentId == -1)).
                                              OrderBy(x => x.Name);
        }
        public List<Process> GetListAsNoTracking()
        {
            return db.Processes.AsNoTracking().OrderBy(x => x.Name).ToList();
        }
        public int GetParentProcessId(int processId)
        {
            Process pcg = db.Processes.FirstOrDefault(x => x.Id == processId);
            return pcg != null ? pcg.ParentId : 0;
        }
        public List<int> GetChildrenProcessesIds(int processId)
        {
            List<Process> pcg = db.Processes.Where(x => x.ParentId == processId).ToList();
            return pcg != null ? pcg.Select(b => b.Id).ToList() : new List<int>();
        }
    }
}