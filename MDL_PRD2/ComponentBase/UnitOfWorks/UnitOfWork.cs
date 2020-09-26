
using MDL_BASE.Models.MasterData;
using XLIB_COMMON.Repo.Base;
using MDL_PRD.Interface;
using MDLX_BASE.Repo;
using System;
using MDLX_MASTERDATA.Entities;
using MDLX_CORE.ComponentCore.Repos;
using MDLX_CORE.ComponentCore.UnitOfWorks;

namespace MDL_PRD.Repo
{
    public class UnitOfWork : UnitOfWorkCore, IDisposable
    {
        private bool disposed = false;
        private IDbContextPRD context;
        public IDbContextPRD DbContext { get { return context; } }

        public UnitOfWork(IDbContextPRD db) : base(db)
        {
            context = db;
        }

        private GenericRepository<Resource2> repoLine;
        private GenericRepository<Item> repoPnc;
        private BomRepo repoBom;
        //private ProductionOrderRepo productionOrderRepo;
        //private ProdOrder20Repo prodOrder20Repo;

        public GenericRepository<Resource2> RepoLine
        {
            get
            {
                if (this.repoLine == null)
                {
                    this.repoLine = new GenericRepository<Resource2>(context);
                }
                return repoLine;
            }
        }
       
        public GenericRepository<Item> RepoPnc
        {
            get
            {
                if (this.repoPnc == null)
                {
                    this.repoPnc = new GenericRepository<Item>(context);
                }
                return repoPnc;
            }
        }
        public BomRepo RepoBom
        {
            get
            {
                if (this.repoBom == null)
                {
                    this.repoBom = new BomRepo(context);
                }
                return repoBom;
            }
        }
        //public ProductionOrderRepo ProductionOrderRepo
        //{
        //    get
        //    {
        //        if (this.productionOrderRepo == null)
        //        {
        //            this.productionOrderRepo = new ProductionOrderRepo(context);
        //        }
        //        return productionOrderRepo;
        //    }
        //}
        //public ProdOrder20Repo ProdOrder20Repo
        //{
        //    get
        //    {
        //        if (this.prodOrder20Repo == null)
        //        {
        //            this.prodOrder20Repo = new ProdOrder20Repo(context);
        //        }
        //        return prodOrder20Repo;
        //    }
        //}


        public void Save()
        {
            context.SaveChanges();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}