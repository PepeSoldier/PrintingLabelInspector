using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MDL_BASE.Models.IDENTITY;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MDL_BASE.Interfaces;
using System.Data;
using System.Data.Entity.Migrations;

namespace XLIB_COMMON.Repo.IDENTITY
{
    public class UserRepo : UserManager<User, string>
    {
        private IDbContextCore db;
        
        public UserRepo(IUserStore<User, string> store, IDbContextCore dbContext) : base(store)
        {
            db = dbContext;
        }

        public string AddOperator(string userName)
        {
            string userN = userName;

            User user = this.FindByName(userN);
            if (user == null)
            {
                user = new User { UserName = userN };
                this.Create(user, "12345678");
                this.AddToRole(user.Id, DefRoles.ONEPROD_MES_OPERATOR);
            }
            return user.Id;
        }
        public string AddUser(string userName, string roleName = "", string password = "12345678")
        {
            string userN = userName;

            User user = this.FindByName(userN);
            if (user == null)
            {
                user = new User { UserName = userN };
                try
                {
                    this.Create(user, password);
                    this.AddToRole(user.Id, DefRoles.USER);
                    if (roleName != "")
                        this.AddToRole(user.Id, roleName);
                }
                catch (Exception e)
                {
                    throw e;
                }
                
            }
            return user.Id;
        }
        public User FindById(string UserId)
        {
            return db.Users.FirstOrDefault(x => x.Id == UserId && x.Deleted == false);
        }
        public IQueryable<User> GetUsers()
        {
            return db.Users.Where(x=>x.Deleted == false);
        }
        public List<User> GetUsersByRole(ApplicationRole role)
        {
            List<User> UserList = new List<User>();
            IQueryable<string> UsersId =  db.UserRoles
                .Where(x => x.RoleId == role.Id)
                .Select(x => x.UserId);

            foreach(string userId in UsersId)
            {
                User user = FindById(userId);
                UserList.Add(user);
            }
            return UserList;
        }
        public List<UserLight> GetUserListAutocomplete(string Prefix)
        {
            List<UserLight> ac = new List<UserLight>();
            
            List<User> usreList = db.Users.ToList();
            var queue = (from t in db.Users
                         where t.UserName.StartsWith(Prefix)
                         select new UserLight()
                         {
                             FullName = t.UserName,
                             Id = t.Id
                         }).Take(5);

            ac.AddRange(queue.ToList());

            return ac;
        }
        public List<UserRole> GetUserRoleList()
        {
            return db.UserRoles.ToList();
        }
        public List<User> GetSubordinates(string userId)
        {
            User user = FindById(userId);
            if (user.Department != null)
            {
                return db.Users.Where(x => x.Department.Id == user.Department.Id).ToList();
            }
            return new List<User>();
        }
        public List<User> GetManagers()
        {
            string[] managerRoleNames = DefRoles.Managers.Split(',');
            var rolesIds = db.Roles.Where(x => managerRoleNames.Contains(x.Name)).Select(x=>x.Id).ToList();
            List<User> Users = new List<User>();

            foreach (string roleId in rolesIds)
            {
                Users.AddRange(db.Users.Where(u => u.Roles.Select(y => y.RoleId).Contains(roleId)).ToList());
            }
            return Users.Distinct().ToList();
        }
        public List<User> GetManagers(int departmentId)
        {
            //string roleId = db.Roles.FirstOrDefault(r => r.Name == DefRoles.Manager).Id;
            //return db.Users.Where(u => u.DepartmentId == departmentId && u.Roles.Select(y => y.RoleId).Contains(roleId)).ToList();
            return new List<User>();
        }
        public List<User> GetQualityEngineers()
        {
            ApplicationRole role = db.Roles.FirstOrDefault(rr => rr.Name == DefRoles.PFEP_PACKINGINSTR_EXAMINER);
            string qualityEngineerRoleId = (role != null) ? role.Id.Trim() : string.Empty;

            List<User> Users = db.Users.Where(u => u.Roles.Select(y => y.RoleId).Contains(qualityEngineerRoleId)).ToList();

            return Users;
        }
        public List<User> GetInstructionConfirmers()
        {
            ApplicationRole role = db.Roles.FirstOrDefault(rr => rr.Name == DefRoles.PFEP_PACKINGINSTR_CONFIRMER);
            ApplicationRole role2 = db.Roles.FirstOrDefault(rr => rr.Name == DefRoles.ONEPROD_MES_SUPEROPERATOR);

            string managerrRoleId = (role != null) ? role.Id.Trim() : string.Empty;
            string prodLeaderRoleId = (role2 != null) ? role2.Id.Trim() : string.Empty;

            List<User> Users = db.Users.Where(u => u.Roles.Select(x => x.RoleId).Contains(managerrRoleId) || u.Roles.Select(x => x.RoleId).Contains(prodLeaderRoleId)).ToList();

            return Users;
        }
        public List<UserRolesName> UserRoleNameList(List<UserRole> Ur)
        {
            List<UserRolesName> list = new List<UserRolesName>();
            foreach (var k in Ur)
            {
                UserRolesName URnames = new UserRolesName();
                URnames.user = FindById(k.UserId);
                URnames.role = db.Roles.FirstOrDefault(x => x.Id == k.RoleId); //RoleRepo.FindById(k.RoleId);
                list.Add(URnames);
            }
            return list;
        }
        public List<UserLight> GetUserListByDepartmentAutocomplete(string prefix, string departmentId)
        {
            List<UserLight> ac = new List<UserLight>();
            int DepartmentId = Convert.ToInt32(departmentId);

            List<User> usreList = db.Users.ToList();
            var queue = (from t in db.Users
                         where (t.FirstName.StartsWith(prefix) || t.LastName.StartsWith(prefix)) &&
                          (DepartmentId == 0 || t.DepartmentId == DepartmentId)
                         select new UserLight()
                         {
                             FullName = t.LastName + " " + t.FirstName,
                             Id = t.Id
                         }).Take(5);

            ac.AddRange(queue.ToList());
            ac = ac.Distinct().ToList();
            return ac;
        }
        public ApplicationRole FindByRoleName(string roleName)
        {
            return db.Roles.FirstOrDefault(x => x.Name == roleName);
        }

        public bool AddUserToRole(string UserId, string RoleId)
        {
            UserRole ur = db.UserRoles.FirstOrDefault(x => x.UserId == UserId && x.RoleId == RoleId);

            if (ur == null)
            {
                db.UserRoles.Add(new UserRole { RoleId = RoleId, UserId = UserId });
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateUser(User usr)
        {
            db.Entry(usr).State = EntityState.Modified;
            db.SaveChanges();
        }
        public bool AddUserToRoleName(string UserId, string RoleName)
        {
            ApplicationRole role = db.Roles.FirstOrDefault(x => x.Name == RoleName);

            if(role != null)
            {
                UserRole ur = db.UserRoles.FirstOrDefault(x => x.UserId == UserId && x.RoleId == role.Id);

                if (ur == null)
                {
                    db.UserRoles.Add(new UserRole { RoleId = role.Id, UserId = UserId });
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool AddUserNameToRole(string UserName, string RoleName)
        {
            User user = this.FindByName(UserName);
            ApplicationRole role = db.Roles.FirstOrDefault(x => x.Name == RoleName);

            if (user != null)
            {
                UserRole ur = db.UserRoles.FirstOrDefault(x => x.UserId == user.Id && x.RoleId == role.Id);

                if (ur == null)
                {
                    db.UserRoles.Add(new UserRole { RoleId = role.Id, UserId = user.Id });
                    db.SaveChanges();
                    return true;
                }    
            }

            return false;

        }
        public void AddToRole(string userId, string roleId, bool v)
        {
            db.UserRoles.Add(new UserRole { RoleId = roleId, UserId = userId });
            db.SaveChanges();
        }

        public void SetFirstName(string id, string firstNAme)
        {
            User user = db.Users.FirstOrDefault(u => u.Id == id);
            user.FirstName = firstNAme;
            db.SaveChanges();
        }
        public void SetLastName(string id, string LastName)
        {
            User user = db.Users.FirstOrDefault(u => u.Id == id);
            user.LastName = LastName;
            db.SaveChanges();
        }
        public void SetSupervisor(string id, string supervisorUserId)
        {
            User user = db.Users.FirstOrDefault(u => u.Id == id);
            user.SuperVisorUserId = supervisorUserId;
            db.SaveChanges();
        }

        public void SetPhoneNumber(string id, string phoneNumber)
        {
            string mystring = phoneNumber;
            if(phoneNumber != null)
            {
                try
                {
                    phoneNumber = String.Concat(phoneNumber.Replace('-', ' ').Where(c => !Char.IsWhiteSpace(c)));
                    mystring = phoneNumber.Substring(Math.Max(0, phoneNumber.Length - 9));
                }
                catch (Exception)
                {
                }
            }
            User user = db.Users.FirstOrDefault(u => u.Id == id);
            user.PhoneNumber = mystring;
            db.SaveChanges();
        }

        public void SetDeputy(string id, string deputyUserId)
        {
            //User user = db.Users.FirstOrDefault(u => u.Id == id);
            //user.DeputyUserID = deputyUserId;
            //db.SaveChanges();
        }
        public void SetDepartment(string id, int departmentId)
        {
            User user = db.Users.FirstOrDefault(u => u.Id == id);
            user.Department = db.Departments.FirstOrDefault(x => x.Id == departmentId && x.Deleted != true);
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void SetDeleted(string id)
        {
            User user = db.Users.FirstOrDefault(u => u.Id == id);
            user.Deleted = true;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}