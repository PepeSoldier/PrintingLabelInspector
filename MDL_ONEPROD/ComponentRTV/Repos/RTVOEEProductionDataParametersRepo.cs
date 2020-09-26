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
    public class RTVOEEProductionDataParameterRepo : RepoGenericAbstract<RTVOEEProductionDataParameter>
    {
        protected new IDbContextOneProdRTV db;

        public RTVOEEProductionDataParameterRepo(IDbContextOneProdRTV db) : base(db)
        {
            this.db = db;
        }

        public override RTVOEEProductionDataParameter GetById(int id)
        {
            return db.RTVOEEProductionDataParameters.FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<RTVOEEProductionDataParameter> GetList()
        {
            return db.RTVOEEProductionDataParameters.Where(x => x.Deleted == false).OrderByDescending(x => x.Id);
        }

        public List<RTVOEEProductionDataParameterModel> GetActualValues(int machineId)
        {
            int parametersCount = db.RTVOEEParameters.Where(x => x.ResourceId == machineId).Count();

            var query = db.RTVOEEProductionDataParameters
                .Where(x => (x.Deleted == false && x.MachineId == machineId))
                .GroupBy(x => x.RTVParameter)
                .Select(y => new RTVOEEProductionDataParameterModel()
                {
                    Id = y.Max(x => x.Id),
                    ResourceId = y.Key.ResourceId,
                    Name = y.Key.Name ?? "?",
                    //Value = y.Key.Value,
                    DataType = y.Key.DataType,
                    Min = y.Key.Min,
                    Max = y.Key.Max,
                    Target = y.Key.Target,
                    //Date = y.Max(x => x.Date)
                })
                .OrderByDescending(x => x.Id)
                .Take(parametersCount);

            var list = query.ToList();

            var query2 = from p in list
                         join p2 in db.RTVOEEProductionDataParameters on p.Id equals p2.Id
                         select new RTVOEEProductionDataParameterModel()
                         {
                             Id = p.Id,
                             Name = p.Name,
                             DataType = p.DataType,
                             Max = p.Max,
                             Min = p.Min,
                             Target = p.Target,
                             ResourceId = p.ResourceId,
                             Date = p2.Date,
                             Value = p2.Value
                         };

            var list2 = query2.ToList();

            return list2;
        }

    }
}