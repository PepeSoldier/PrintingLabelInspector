using MDLX_CORE.Interfaces;
using MDLX_CORE.Models.IDENTITY;
using MDL_LABELINSP.Entities;
using MDLX_CORE.ComponentCore.Entities;
using System.Data.Entity;

namespace MDL_LABELINSP.Interfaces
{
    public interface IDbContextLabelInsp : IDbContextCore
    {
        //ID
        //IDbSet<User> Users { get; set; }
        //IDbSet<ApplicationRole> Roles { get; set; }
        //IDbSet<UserRole> UserRoles { get; set; }

        //MasterData
        DbSet<ItemData> ItemData { get; set; }
        DbSet<Workorder> Workorders { get; set; }
        DbSet<WorkorderLabel> WorkorderLabels { get; set; }
        DbSet<WorkorderLabelInspection> WorkorderLabelInspections { get; set; }

        //DbSet<SystemVariable> SystemVariables { get; set; }
    }
}