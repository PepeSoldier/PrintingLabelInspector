using MDLX_CORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Repo.Base;
using XLIB_COMMON.Interface;

namespace MDLX_CORE.ComponentCore.UnitOfWorks
{
    public class UnitOfWorkCore : UnitOfWorkAbstract
    {
        IDbContextCore db;
        public UnitOfWorkCore(IDbContextCore db) : base(db)
        {
            this.db = db;
        }

        private AttachmentRepo attachmentRepo;
        private SystemVariableRepo systemVariableRepo;
        
        public AttachmentRepo AttachmentRepo
        {
            get
            {
                attachmentRepo = (attachmentRepo != null) ? attachmentRepo : new AttachmentRepo(db);
                return attachmentRepo;
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