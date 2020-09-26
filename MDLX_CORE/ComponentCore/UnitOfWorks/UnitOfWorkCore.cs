using MDL_BASE.Interfaces;
using MDLX_CORE.ComponentCore.Repos;
using MDL_ONEPROD.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Repo.Base;

namespace MDLX_CORE.ComponentCore.UnitOfWorks
{
    public class UnitOfWorkCore : UnitOfWorkMasterData
    {
        IDbContextCore db;
        public UnitOfWorkCore(IDbContextCore db) : base(db)
        {
            this.db = db;
        }

        private AttachmentRepo attachmentRepo;
        private BomRepo bomRepo;
        private BomWorkorderRepo bomWorkorderRepo;
        private ChangeLogRepo changeLogRepo;
        private NotificationDeviceRepo notificationDeviceRepo;
        private PrinterRepo printerRepo;
        private ProductionOrderRepo productionOrderRepo;
        private SystemVariableRepo systemVariableRepo;

        public AttachmentRepo AttachmentRepo
        {
            get
            {
                attachmentRepo = (attachmentRepo != null) ? attachmentRepo : new AttachmentRepo(db);
                return attachmentRepo;
            }
        }
        public BomRepo BomRepo
        {
            get
            {
                bomRepo = (bomRepo != null) ? bomRepo : new BomRepo(db);
                return bomRepo;
            }
        }
        public BomWorkorderRepo BomWorkorderRepo
        {
            get
            {
                bomWorkorderRepo = (bomWorkorderRepo != null) ? bomWorkorderRepo : new BomWorkorderRepo(db);
                return bomWorkorderRepo;
            }
        }
        public ChangeLogRepo ChangeLogRepo
        {
            get
            {
                changeLogRepo = (changeLogRepo != null) ? changeLogRepo : new ChangeLogRepo(db);
                return changeLogRepo;
            }
        }
        public NotificationDeviceRepo NotificationDeviceRepo
        {
            get
            {
                notificationDeviceRepo = (notificationDeviceRepo != null) ? notificationDeviceRepo : new NotificationDeviceRepo(db);
                return notificationDeviceRepo;
            }
        }
        public PrinterRepo PrinterRepo
        {
            get
            {
                printerRepo = (printerRepo != null) ? printerRepo : new PrinterRepo(db);
                return printerRepo;
            }
        }
        public ProductionOrderRepo ProductionOrderRepo
        {
            get
            {
                if (this.productionOrderRepo == null)
                {
                    this.productionOrderRepo = new ProductionOrderRepo(db);
                }
                return productionOrderRepo;
            }
        }
        public SystemVariableRepo SystemVariableRepo
        {
            get
            {
                if (this.systemVariableRepo == null)
                {
                    this.systemVariableRepo = new SystemVariableRepo(db);
                }
                return systemVariableRepo;
            }
        }
    }
}