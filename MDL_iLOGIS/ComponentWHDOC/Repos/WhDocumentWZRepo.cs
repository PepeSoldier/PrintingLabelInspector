using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWHDOC.Entities;
using MDL_iLOGIS.ComponentWHDOC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;

namespace MDL_iLOGIS.ComponentWHDOC.Repos
{
    public class WhDocumentWZRepo : RepoGenericAbstract<WhDocumentWZ>
    {
        protected new IDbContextiLOGIS db;
        //private UnitOfWorkOneprod unitOfWork;

        public WhDocumentWZRepo(IDbContextiLOGIS db, IAlertManager alertManager = null)
            : base(db)
        {
            this.db = db;
        }

        public override WhDocumentWZ GetById(int id)
        {
            return db.WhDocumentWZs.FirstOrDefault(x => x.Id == id);
        }
        public override WhDocumentWZ GetByIdAsNoTracking(int id)
        {
            return db.WhDocumentWZs.AsNoTracking().Where(x => x.Deleted == false).FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<WhDocumentWZ> GetList()
        {
            return db.WhDocumentWZs.Where(x => x.Deleted == false).OrderBy(x => x.Id);
        }

        public IQueryable<WhDocumentWZ> GetList(WhDocumentAbstractViewModel filter)
        {
            DateTime nullDatetime = new DateTime();
            return db.WhDocumentWZs.Where(x =>
                    (filter.ContractorName == null || x.Contractor.Name.Contains(filter.ContractorName)) && 
                    (filter.DocumentNumber == null || x.DocumentNumber.Contains(filter.DocumentNumber)) &&
                    (filter.DocumentDate == nullDatetime || x.DocumentDate >= filter.DocumentDate) &&
                    (filter.StampTime == nullDatetime || x.StampTime >= filter.StampTime) &&
                    (filter.ItemCode == null || x.WhDocumentItems.Any(y => y.ItemCode == filter.ItemCode)) &&
                    (filter.Notice == null || x.Notice.Contains(filter.Notice)) &&
                    (filter.Status == Enums.EnumWhDocumentStatus.undefined || x.Status == filter.Status)
                )
                .OrderBy(x => x.Id);
        }

    }
}
