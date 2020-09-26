using MDL_BASE.ComponentBase.Entities;
using MDL_BASE.Interfaces;
using MDL_BASE.Models.Base;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDL_CORE.ComponentCore.Entities;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentWMS.Entities;
using MDL_ONEPROD.ComponentMes.Etities;
using MDL_ONEPROD.Model.Scheduling;
using MDLX_CORE.ComponentCore.Entities;
using MDLX_MASTERDATA.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using Process = MDLX_MASTERDATA.Entities.Process;

namespace _MPPL_WEB_START.Migrations
{
    public class DbContextFake : DbContext
    {
        //public virtual IDbSet<FakeEntity> FakeEntities { get; set; }

        public DbContextFake()
        {
        }
    }

    public class FakeEntity
    {
        public int Id { get; set; }
    }
}