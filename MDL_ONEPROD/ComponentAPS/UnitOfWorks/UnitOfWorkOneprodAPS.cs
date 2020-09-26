using MDL_ONEPROD.Common;
using MDL_ONEPROD.Interface;
using MDL_ONEPROD.Repo.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Model;

namespace MDL_ONEPROD.Repo
{
    public class UnitOfWorkOneprodAPS : UnitOfWorkOneprod
    {
        IDbContextOneprodAPS db;
        public UnitOfWorkOneprodAPS(IDbContextOneprodAPS dbContext) : base(dbContext)
        {
            db = dbContext;
        }

        //private
        private CalendarRepo calendarRepo;
        private ChangeOverRepo changeOverRepo;
        private ItemGroupToolRepo partCategoryToolRepo;
        private ToolChangeOverRepo toolChangeOverRepo;
        private ToolRepo toolRepo;
        private ToolMachineRepo toolMachineRepo;

        
        public CalendarRepo CalendarRepo
        {
            get
            {
                calendarRepo = (calendarRepo != null) ? calendarRepo : new CalendarRepo(db, AlertManager.Instance, this);
                return calendarRepo;
            }
        }
        public ChangeOverRepo ChangeOverRepo
        {
            get
            {
                changeOverRepo = (changeOverRepo != null) ? changeOverRepo : new ChangeOverRepo(db, AlertManager.Instance, this);
                return changeOverRepo;
            }
        }
        public ItemGroupToolRepo ItemGroupToolRepo
        {
            get
            {
                partCategoryToolRepo = (partCategoryToolRepo != null) ? partCategoryToolRepo : new ItemGroupToolRepo(db, AlertManager.Instance, this);
                return partCategoryToolRepo;
            }
        }
        public ToolChangeOverRepo ToolChangeOverRepo
        {
            get
            {
                toolChangeOverRepo = (toolChangeOverRepo != null) ? toolChangeOverRepo : new ToolChangeOverRepo(db, AlertManager.Instance, this);
                return toolChangeOverRepo;
            }
        }
        public ToolRepo ToolRepo
        {
            get
            {
                toolRepo = (toolRepo != null) ? toolRepo : new ToolRepo(db, AlertManager.Instance, this);
                return toolRepo;
            }
        }
        public ToolMachineRepo ToolMachineRepo
        {
            get
            {
                toolMachineRepo = (toolMachineRepo != null) ? toolMachineRepo : new ToolMachineRepo(db, AlertManager.Instance, this);
                return toolMachineRepo;
            }
        }

    }
}