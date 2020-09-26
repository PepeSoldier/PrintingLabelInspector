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
    public class WhDocumentItemRepo : RepoGenericAbstract<WhDocumentItem>
    {
        protected new IDbContextiLOGIS db;
        //private UnitOfWorkOneprod unitOfWork;

        public WhDocumentItemRepo(IDbContextiLOGIS db, IAlertManager alertManager = null)
            : base(db)
        {
            this.db = db;
        }

        public override WhDocumentItem GetById(int id)
        {
            return db.WhDocumentItems.Where(x => x.Deleted == false).FirstOrDefault(x => x.Id == id);
        }
        public override WhDocumentItem GetByIdAsNoTracking(int id)
        {
            return db.WhDocumentItems.AsNoTracking().Where(x => x.Deleted == false).FirstOrDefault(x => x.Id == id);
        }
        public override IQueryable<WhDocumentItem> GetList()
        {
            return db.WhDocumentItems.Where(x => x.Deleted == false).OrderBy(x => x.Id);
        }

        public IQueryable<WhDocumentItem> GetByDocumentId(int documentId)
        {
            return db.WhDocumentItems.Where(x => x.WhDocumentId == documentId && x.Deleted == false);
        }

        public List<WhDocumentItemLight> GetLastDocumentItemsForContractor(int id)
        {
            var whDoc = db.WhDocumentWZs.Where(x => x.ContractorId == id).OrderByDescending(x => x.Id).FirstOrDefault();
            int whDocId = whDoc != null? whDoc.Id : 0;

            return db.WhDocumentItems.Where(x => x.WhDocumentId == whDocId).Select(y => new WhDocumentItemLight()
            {
                Id = y.Id,
                ItemCode = y.ItemCode,
                ItemName = y.ItemName,
                DisposedQty = y.DisposedQty,
                IssuedQty = y.IssuedQty,
                UnitOfMeasure = y.UnitOfMeasure,
                UnitPrice = y.UnitPrice,
                Value = y.IssuedQty + y.UnitPrice
            }).ToList();
        }

        public void DeleteByDocumentId(int id)
        {
            List<WhDocumentItem> listToRemove = db.WhDocumentItems.Where(x => x.WhDocumentId == id).ToList();
            db.WhDocumentItems.RemoveRange(listToRemove);
        }
    }
}
