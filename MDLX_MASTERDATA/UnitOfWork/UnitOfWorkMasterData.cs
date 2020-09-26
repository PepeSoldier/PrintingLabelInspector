using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA._Interfaces;
using MDLX_MASTERDATA.Repos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Model;

namespace MDL_ONEPROD.Repo
{
    public class UnitOfWorkMasterData : UnitOfWorkAbstract
    {
        IDbContextMasterData db;
        public UnitOfWorkMasterData(IDbContextMasterData dbContext) : base(dbContext)
        {
            db = dbContext;
        }

        private ResourceRepo  resourceRepo;
        private ItemRepo itemRepo;
        private ItemGroupRepo itemGroupRepo;
        private RepoWorkstation workstationRepo;
        private RepoContractor repoContractor;        

        public ResourceRepo  ResourceRepo
        {
            get
            {
                resourceRepo = (resourceRepo != null) ? resourceRepo : new ResourceRepo (db);
                return resourceRepo;
            }
        }
        public ItemRepo ItemRepo
        {
            get
            {
                itemRepo = (itemRepo != null) ? itemRepo : new ItemRepo(db);
                return itemRepo;
            }
        }
        public ItemGroupRepo ItemGroupRepo
        {
            get
            {
                itemGroupRepo = (itemGroupRepo != null) ? itemGroupRepo : new ItemGroupRepo(db, AlertManager.Instance);
                return itemGroupRepo;
            }
        }
        public RepoWorkstation WorkstationRepo
        {
            get
            {
                workstationRepo = (workstationRepo != null) ? workstationRepo : new RepoWorkstation(db);
                return workstationRepo;
            }
        }
        public RepoContractor RepoContractor
        {
            get
            {
                repoContractor = (repoContractor != null) ? repoContractor : new RepoContractor(db);
                return repoContractor;
            }
        }
    }
}