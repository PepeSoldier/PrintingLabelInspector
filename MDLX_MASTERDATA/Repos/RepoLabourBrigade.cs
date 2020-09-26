using MDL_BASE.ComponentBase.Entities;
using MDL_BASE.Interfaces;
using XLIB_COMMON.Repo;
using MDLX_MASTERDATA._Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MDL_BASE.ComponentBase.Repos
{
    public class RepoLabourBrigade : RepoGenericAbstract<LabourBrigade>
    {
        protected new IDbContextMasterData db;

        public RepoLabourBrigade(IDbContextMasterData db) : base(db)
        {
            this.db = db;
        }

        public override LabourBrigade GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<LabourBrigade> GetList()
        {
            return db.LabourBrigades.Where(x => x.Deleted == false).OrderBy(x => x.Name);
        }

        public List<LabourBrigade> GetLabourBrigades()
        {
            return db.LabourBrigades.Where(x => x.Deleted == false).OrderByDescending(x => x.Id).ToList();
        }

        public List<LabourBrigade> GetLabourBrigadeOrderList(string prefix)
        {
            return db.LabourBrigades.Where(x => x.Name.StartsWith(prefix) && x.Deleted == false).Distinct().Take(5).ToList();
        }

        public List<string> LabourEmployeeAutocomplete(string prefix, string shiftCode)
        {
            var _prefix = new SqlParameter("@prefix", prefix);
            var _shiftCode = new SqlParameter("@shiftCode", shiftCode);

            List<AcResult> results = new List<AcResult>();
            try
            {
                results = db.Database.SqlQuery<AcResult>("" +
                        "EXEC [dbo].[ONEPROD_OEE_EmployeeAutocomplete]  @prefix, @shiftCode",
                        _prefix, _shiftCode).ToList();
            }
            catch (Exception)
            {
            }

            return results.Select(x => x.Name).ToList();
        }

        private class AcResult
        {
            public string Name { get; set; }
            public string ShiftCode { get; set; }
            public string Mode { get; set; }
        }

    }
}