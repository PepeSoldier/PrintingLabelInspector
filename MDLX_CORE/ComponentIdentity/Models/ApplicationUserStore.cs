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
    public class ApplicationUserStore<TUser> : UserStore<TUser, ApplicationRole, string, UserLogin, UserRole, UserClaim>, IUserStore<TUser, string>, IDisposable where TUser : User
    {
        //public ApplicationUserStore() : this(new IdentityDbContext())
        //{
        //    base.DisposeContext = true;
        //}
        public ApplicationUserStore(DbContext context) : base(context)
        {

        }
    }
}