using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWHDOC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Repo;

namespace MDL_iLOGIS.ComponentWHDOC.Repos
{
    public class WhDocumentCMRRepo : RepoGenericAbstract<WhDocumentCMR>
    {
        protected new IDbContextiLOGIS db;
        //private UnitOfWorkOneprod unitOfWork;

        public WhDocumentCMRRepo(IDbContextiLOGIS db, IAlertManager alertManager = null)
            : base(db)
        {
            this.db = db;
        }

        public override WhDocumentCMR GetById(int id)
        {
            return db.WhDocumentCMRs.Where(x => x.Deleted == false).FirstOrDefault(x => x.Id == id);
        }
        public override WhDocumentCMR GetByIdAsNoTracking(int id)
        {
            return db.WhDocumentCMRs.AsNoTracking().Where(x => x.Deleted == false).FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<WhDocumentCMR> GetList()
        {
            return db.WhDocumentCMRs.Where(x => x.Deleted == false).OrderBy(x => x.Id);
        }
       

       
    }
}
