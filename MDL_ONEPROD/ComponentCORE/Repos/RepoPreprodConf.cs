using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD
{
    public class RepoPreprodConf : XLIB_COMMON.Repo.Repo
    {
        protected new IDbContextOneprod db;
        public RepoPreprodConf(IDbContextOneprod db) : base(db)
        {
            this.db = db;
        }
               
        //PartGroups
        public List<Process> GetProcesss()
        {
            return db.Processes.ToList();
        }
        public Process GetProcess(int id)
        {
            return db.Processes.FirstOrDefault(p=>p.Id == id);
        }
        
       
    }
}