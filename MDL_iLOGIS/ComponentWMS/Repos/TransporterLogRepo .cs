using MDL_BASE.Interfaces;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_iLOGIS.ComponentWMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;

namespace MDL_iLOGIS.ComponentWMS.Repos
{
    public class TransporterLogRepo : RepoGenericAbstract<TransporterLog>
    {
        protected new IDbContextiLOGIS db;
        //private UnitOfWorkOneprod unitOfWork;

        public TransporterLogRepo (IDbContextiLOGIS db) : base(db)
        {
            this.db = db;
        }
        
        public override TransporterLog GetById(int id)
        {
            return db.TransporterLogs.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<TransporterLog> GetList(TransporterLog filter)
        {
            string itemCode = filter.ItemWMS != null ? filter.ItemWMS.Item.Code : null;
            DateTime dateDefault = new DateTime();

            return db.TransporterLogs.Where(x => 
                    (filter.TransporterId == 0 || filter.TransporterId == x.TransporterId) &&
                    (filter.Location == null || filter.Location == x.Location) &&
                    (filter.WorkorderNumber == null || filter.WorkorderNumber == x.WorkorderNumber) &&
                    (filter.ProductItemCode == null || filter.ProductItemCode == x.ProductItemCode) &&
                    ((itemCode == null) || (x.ItemWMS != null && x.ItemWMS.Item.Code != null && x.ItemWMS.Item.Code.Contains(itemCode))) &&
                    (filter.DateFrom == dateDefault || x.TimeStamp >= filter.DateFrom) &&
                    (filter.DateTo == dateDefault || x.TimeStamp < filter.DateTo) &&
                    (filter.EntryType == 0 || filter.EntryType == x.EntryType))
                .OrderByDescending(x => x.Id);
        }

        public TransporterLog GetTransporterLogByRelatedObjectId(int realtedObjectId, EnumTransporterLogEntryType entryType)
        {
            return db.TransporterLogs.Where(x => x.EntryType == entryType && x.RelatedObjectId == realtedObjectId).FirstOrDefault();
        }

        public TransporterLog GetById_AsNoTracking(int id)
        {
            return db.TransporterLogs.AsNoTracking().FirstOrDefault(x => x.Id == id);
        }

    }
}
