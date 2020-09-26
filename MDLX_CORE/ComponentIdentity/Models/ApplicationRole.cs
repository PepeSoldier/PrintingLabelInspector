using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MDL_BASE.Models.IDENTITY
{
    public class ApplicationRole : IdentityRole<string, UserRole>
    {
        public ApplicationRole() : base()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(RoleManager<ApplicationRole> manager)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.(this, DefaultAuthenticationTypes.ApplicationCookie);
        //    // Add custom user claims here
        //    return userIdentity;
        //}
    }
}