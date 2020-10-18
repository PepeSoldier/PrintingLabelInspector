using MDLX_CORE.Models.Base;
using MDLX_CORE.Models.IDENTITY;
using MDLX_CORE.ComponentCore.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace MDLX_CORE.Interfaces
{
    public interface IDbContextCore : IDbContext
    {
        //ID
        IDbSet<User> Users { get; set; }
        IDbSet<ApplicationRole> Roles { get; set; }
        IDbSet<UserRole> UserRoles { get; set; }
        DbSet<SystemVariable> SystemVariables { get; set; }
    }
}