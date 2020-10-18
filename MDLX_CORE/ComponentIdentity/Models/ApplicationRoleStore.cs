using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MDLX_CORE.Models.IDENTITY
{
    public class ApplicationRoleStore : RoleStore<ApplicationRole, string, UserRole>, IQueryableRoleStore<ApplicationRole, string>, IRoleStore<ApplicationRole, string>, IDisposable //where TRole : ApplicationRole
    {
        //public ApplicationRoleStore() : base(new IdentityDbContext())
        //{
        //    base.DisposeContext = true;
        //}
        public ApplicationRoleStore(DbContext context) : base(context)
        {

        }
    }
}