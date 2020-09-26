using MDL_BASE.ComponentBase.Repos;
using MDL_ONEPROD.Common;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo.Scheduling;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Model;

namespace MDL_ONEPROD.Repo
{
    public class UnitOfWorkOneprod : UnitOfWorkCore
    {
        IDbContextOneprod db;
        public UnitOfWorkOneprod(IDbContextOneprod dbContext) : base (dbContext)
        {
            db = dbContext;
        }


        private ClientOrderRepo clientOrderRepo;
        private ResourceOPRepo resourceRepo;
        private ResourceGroupRepo resourceGroupRepo;
        private ItemOPRepo itemOPRepo;
        private ItemGroupRepo itemGroupRepo;
        private MDLX_MASTERDATA.Repos.ProcessRepo processRepo;
        private CycleTimeRepo cycleTimeRepo;
        private ItemInventoryRepo itemInventoryRepo;
        private RepoPreprodConf confRepo;
        private WorkorderRepo workorderRepo;
        private RepoLabourBrigade labourBrigadeRepo;


        public ClientOrderRepo ClientOrderRepo
        {
            get
            {
                clientOrderRepo = (clientOrderRepo != null) ? clientOrderRepo : new ClientOrderRepo(db, this);
                return clientOrderRepo;
            }
        }
        public new ResourceOPRepo ResourceRepo
        {
            get
            {
                resourceRepo = (resourceRepo != null) ? resourceRepo : new ResourceOPRepo(db, AlertManager.Instance);
                return resourceRepo;
            }
        }
        public ResourceGroupRepo ResourceGroupRepo
        {
            get
            {
                resourceGroupRepo = (resourceGroupRepo != null) ? resourceGroupRepo : new ResourceGroupRepo(db, AlertManager.Instance, this);
                return resourceGroupRepo;
            }
        }
        public ItemOPRepo ItemOPRepo
        {
            get
            {
                itemOPRepo = (itemOPRepo != null) ? itemOPRepo : new ItemOPRepo(db);
                return itemOPRepo;
            }
        }
        public new ItemGroupRepo ItemGroupRepo
        {
            get
            {
                itemGroupRepo = (itemGroupRepo != null) ? itemGroupRepo : new ItemGroupRepo(db, AlertManager.Instance);
                return itemGroupRepo;
            }
        }
        public MDLX_MASTERDATA.Repos.ProcessRepo ProcessRepo
        {
            get
            {
                processRepo = (processRepo != null) ? processRepo : new MDLX_MASTERDATA.Repos.ProcessRepo(db, AlertManager.Instance);
                return processRepo;
            }
        }
        public ItemInventoryRepo ItemInventoryRepo
        {
            get
            {
                itemInventoryRepo = (itemInventoryRepo != null) ? itemInventoryRepo : new ItemInventoryRepo(db, AlertManager.Instance, this);
                return itemInventoryRepo;
            }
        }
        public RepoPreprodConf ConfRepo
        {
            get
            {
                confRepo = (confRepo != null) ? confRepo : new RepoPreprodConf(db);
                return confRepo;
            }
        }
        public WorkorderRepo WorkorderRepo
        {
            get
            {
                workorderRepo = (workorderRepo != null) ? workorderRepo : new WorkorderRepo(db);
                return workorderRepo;
            }
        }
        public CycleTimeRepo CycleTimeRepo
        {
            get
            {
                cycleTimeRepo = (cycleTimeRepo != null) ? cycleTimeRepo : new CycleTimeRepo(db, AlertManager.Instance, this);
                return cycleTimeRepo;
            }
        }
        public RepoLabourBrigade LabourBrigadeRepo
        {
            get
            {
                labourBrigadeRepo = (labourBrigadeRepo != null) ? labourBrigadeRepo : new RepoLabourBrigade(db);
                return labourBrigadeRepo;
            }
        }


    }
}