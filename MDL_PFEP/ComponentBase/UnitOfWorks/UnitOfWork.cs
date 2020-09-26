using MDL_BASE.Models.MasterData;
using MDLX_CORE.ComponentCore.Repos;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentConfig.Repos;
using MDL_PFEP.Interface;
using MDL_PFEP.Models.DEF;
using MDL_PFEP.Repo.PFEP;
using MDL_PRD.Interface;
using MDL_PRD.Repo;
using MDL_WMS.ComponentConfig.Repos;
using MDLX_BASE.Repo;
using MDLX_MASTERDATA.Entities;
using MDLX_MASTERDATA.Repos;
using System;

namespace MDL_PFEP.Repo
{
    public class UnitOfWork : IDisposable
    {
        private bool disposed = false;
        private readonly IDbContextPFEP context;
        private readonly IDbContextPRD context2;
        public IDbContextPFEP DbContext { get { return context; } }

        public UnitOfWork(IDbContextPFEP db, IDbContextPRD db2)
        {
            context = db;
            context2 = db2;
        }


        //private GenericRepository<AncType> repoAncType;
        private ResourceRepo repoLine;
        private GenericRepositoryName<Resource2> repoLineExtend;
        private GenericRepository<Package> repoPackage;
        //private GenericRepository<PncType> repoPncType;
        private GenericRepository<Workstation> repoWorkstation;
        private GenericRepository<MDLX_MASTERDATA.Entities.Item> repoAnc;
        private PackageItemRepo repoAncPackage;
        private WorkstationItemRepo repoAncWorkstation;
        private GenericRepository<MDLX_MASTERDATA.Entities.Item> repoPnc;
        private PFEP.BomRepo repoBom;
        private PFEP.BomWorkorderRepo repoBomWorkorder;
        private PrintHistoryRepo printHistoryRepo;
        private ProductionOrderRepo productionOrderRepo;
        private ProdOrder20Repo prodOrder20Repo;

        //public GenericRepository<AncType> RepoAncType
        //{
        //    get
        //    {
        //        if (this.repoAncType == null)
        //        {
        //            this.repoAncType = new GenericRepository<AncType>(context);
        //        }
        //        return repoAncType;
        //    }
        //}
        public ResourceRepo RepoLine
        {
            get
            {
                if (this.repoLine == null)
                {
                    this.repoLine = new ResourceRepo(context);
                }
                return repoLine;
            }
        }
        public GenericRepositoryName<Resource2> RepoLineExtend
        {
            get
            {
                if (this.repoLineExtend == null)
                {
                    this.repoLineExtend = new GenericRepositoryName<Resource2>(context);
                }
                return repoLineExtend;
            }
        }
        public GenericRepository<Package> RepoPackage
        {
            get
            {
                if (this.repoPackage == null)
                {
                    this.repoPackage = new GenericRepository<Package>(context);
                }
                return repoPackage;
            }
        }
        //public GenericRepository<PncType> RepoPncType
        //{
        //    get
        //    {
        //        if (this.repoPncType == null)
        //        {
        //            this.repoPncType = new GenericRepository<PncType>(context);
        //        }
        //        return repoPncType;
        //    }
        //}
        public GenericRepository<Workstation> RepoWorkstation
        {
            get
            {
                if (this.repoWorkstation == null)
                {
                    this.repoWorkstation = new GenericRepository<Workstation>(context);
                }
                return repoWorkstation;
            }
        }
        public GenericRepository<MDLX_MASTERDATA.Entities.Item> RepoAnc
        {
            get
            {
                if (this.repoAnc == null)
                {
                    this.repoAnc = new GenericRepository<MDLX_MASTERDATA.Entities.Item>(context);
                }
                return repoAnc;
            }
        }
        public PackageItemRepo RepoAncPackage
        {
            get
            {
                if (this.repoAncPackage == null)
                {
                    this.repoAncPackage = new PackageItemRepo(context);
                }
                return repoAncPackage;
            }
        }
        public WorkstationItemRepo RepoAncWorkstation
        {
            get
            {
                if (this.repoAncWorkstation == null)
                {
                    this.repoAncWorkstation = new WorkstationItemRepo(context);
                }
                return repoAncWorkstation;
            }
        }
        public GenericRepository<MDLX_MASTERDATA.Entities.Item> RepoPnc
        {
            get
            {
                if (this.repoPnc == null)
                {
                    this.repoPnc = new GenericRepository<MDLX_MASTERDATA.Entities.Item>(context);
                }
                return repoPnc;
            }
        }
        public PFEP.BomRepo RepoBom
        {
            get
            {
                if (this.repoBom == null)
                {
                    this.repoBom = new PFEP.BomRepo(context);
                }
                return repoBom;
            }
        }
        public PFEP.BomWorkorderRepo RepoBomWorkorder
        {
            get
            {
                if (this.repoBomWorkorder == null)
                {
                    this.repoBomWorkorder = new PFEP.BomWorkorderRepo(context);
                }
                return repoBomWorkorder;
            }
        }
        public ProductionOrderRepo ProductionOrderRepo
        {
            get
            {
                if (this.productionOrderRepo == null)
                {
                    this.productionOrderRepo = new ProductionOrderRepo(context2);
                }
                return productionOrderRepo;
            }
        }
        public PrintHistoryRepo PrintHistoryRepo
        {
            get {
                if (this.printHistoryRepo == null)
                {
                    this.printHistoryRepo = new PrintHistoryRepo(context);
                }
                return printHistoryRepo;
            }
        }
        public ProdOrder20Repo ProdOrder20Repo
        {
            get
            {
                if (this.prodOrder20Repo == null)
                {
                    this.prodOrder20Repo = new ProdOrder20Repo(context2);
                }
                return prodOrder20Repo;
            }
        }


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
