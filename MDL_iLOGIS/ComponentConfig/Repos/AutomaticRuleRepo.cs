using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;

namespace MDL_iLOGIS.ComponentConfig.Repos
{
    public class AutomaticRuleRepo : RepoGenericAbstract<AutomaticRule>
    {
        protected new IDbContextiLOGIS db;
        //private UnitOfWorkOneprod unitOfWork;

        public AutomaticRuleRepo(IDbContextiLOGIS db, IAlertManager alertManager = null)
            : base(db)
        {
            this.db = db;
        }

        public override AutomaticRule GetById(int id)
        {
            return db.AutomaticRules.Where(x => x.Id == id).FirstOrDefault(x => x.Id == id);
        }
        public IQueryable<AutomaticRule> GetList(AutomaticRule filter)
        {
            return db.AutomaticRules.Where(x => 
                    (filter.Name == null || x.Name.Contains(filter.Name)) &&
                    (filter.PREFIX == null || x.PREFIX == filter.PREFIX) &&
                    (filter.IsPackageType == x.IsPackageType) &&
                    (filter.WorkstationName == null || x.WorkstationName.Contains(filter.WorkstationName)))
                .OrderBy(x => x.Name);
        }
        public bool Apply()
        {
            bool done = false;

            try
            {
                db.Database.ExecuteSqlCommand("EXEC [dbo].[iLOGIS_AutomaticRules_Apply]");
                done = true;
            }
            catch
            {
                done = false;
            }

            return done;
        }
        public bool ApplyById(int id)
        {
            bool done = false;

            try
            {
                AutomaticRule rule = GetById(id);
                db.Database.ExecuteSqlCommand("EXEC [dbo].[iLOGIS_AutomaticRules_ApplyById] " +
                    "@ruleId = " + id + ", " +
                    "@prefix = N'" + rule.PREFIX + "', " +
                    "@workstationName = N'" + rule.WorkstationName + "', " +
                    "@lineNames = N'" + rule.LineNames + "'");
                done = true;
            }
            catch
            {
                done = false;
            }

            return done;
        }
        public bool ApplyPackageById(int id)
        {
            bool done = false;

            try
            {
                AutomaticRule rule = GetById(id);
                db.Database.ExecuteSqlCommand("EXEC [dbo].[iLOGIS_AutomaticRules_ApplyPackageById] " +
                    "@ruleId = " + id + ", " +
                    "@prefix = N'" + rule.PREFIX + "', " +
                    "@packageId = N'" + rule.PackageId+ "'");
                done = true;
            }
            catch
            {
                done = false;
            }

            return done;
        }
    }
}
