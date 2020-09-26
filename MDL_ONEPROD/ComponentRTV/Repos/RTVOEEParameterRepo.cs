using XLIB_COMMON.Repo;
using MDL_ONEPROD.ComponentRTV._Interfaces;
using MDL_ONEPROD.Model.OEEProd;
using MDL_ONEPROD.Model.Scheduling.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lextm.SharpSnmpLib.Security;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.ComponentRTV.Models;
using MDL_ONEPROD.ComponentMes.Models;
using MDL_ONEPROD.ComponentRTV.Entities;

namespace MDL_ONEPROD.ComponentRTV.Repos
{
    public class RTVOEEParameterRepo : RepoGenericAbstract<RTVOEEParameter>
    {
        protected new IDbContextOneProdRTV db;

        public RTVOEEParameterRepo(IDbContextOneProdRTV db) : base(db)
        {
            this.db = db;
        }

        public override RTVOEEParameter GetById(int id)
        {
            return db.RTVOEEParameters.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<RTVOEEParameter> GetList()
        {
            return db.RTVOEEParameters.Where(x => x.Deleted == false).OrderByDescending(x => x.Id);
        }
    }
}