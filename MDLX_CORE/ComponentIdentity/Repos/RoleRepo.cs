using Microsoft.AspNet.Identity.EntityFramework;
using MDLX_CORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDLX_CORE.Interfaces;
using Microsoft.AspNet.Identity;
using MDLX_CORE.Models.IDENTITY;
using System.Diagnostics;

namespace XLIB_COMMON.Repo.IDENTITY
{
    public class RoleRepo : RoleManager<ApplicationRole>
    {
        IDbContextCore db;
        public RoleRepo(IRoleStore<ApplicationRole, string> store, IDbContextCore dbContext) : base(store)
        {
            db = dbContext;
        }

        public string AddRole(string roleName)
        {
            ApplicationRole role = db.Roles.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
            {
                role = new ApplicationRole { Name = roleName };
                try
                {
                    CreateAsync(role).Wait();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                //db.Roles.Add(role);
                //db.SaveChanges();
            }
            
            return role.Id;
        }
        public ApplicationRole FindById(string roleId)
        {
            return db.Roles.FirstOrDefault(x => x.Id == roleId);
        }
        public IQueryable<ApplicationRole> GetRoles()
        {
            return db.Roles.OrderBy(x => x.Name);
        }
        public IQueryable<ApplicationRole> GetRoles(string userId, bool list)
        {
            return db.Roles.Where(u => u.Users.Select(y => y.UserId).Contains(userId)).OrderBy(x => x.Name);
        }

        //To jest tylko do seedów, nie może być w REPO
        //public void AddAccountAdminToAdmins()
        //{
        //    List<string> AdminRolesIds = db.Roles.Where(x => x.Name == "ADMIN" || x.Name == "Admin").Select(x => x.Id).ToList();
        //    List<string> AdminUserIds = db.UserRoles.Where(x => AdminRolesIds.Contains(x.RoleId)).Select(x => x.UserId).ToList();

        //    ApplicationRole applicationRole = db.Roles.Where(x => x.Name == DefRoles.ACCOUNT_ADMIN).FirstOrDefault();

        //    foreach(var admUserId in AdminUserIds)
        //    {
        //        UserRole userRole = new UserRole();
        //        userRole.RoleId = applicationRole.Id;
        //        userRole.UserId = admUserId;
        //        db.UserRoles.Add(userRole);
        //        db.SaveChanges();
        //    }
        //}

    }
}