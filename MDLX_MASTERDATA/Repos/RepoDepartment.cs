using MDL_BASE.Interfaces;
using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA._Interfaces;
using System.Collections.Generic;
using System.Linq;
using XLIB_COMMON.Repo;

namespace MDLX_MASTERDATA.Repos
{
    public class RepoDepartment : RepoGenericAbstract<Department>
    {
        protected new IDbContextMasterData db;
        
        public RepoDepartment(IDbContextMasterData db) : base(db)
        {
            this.db = db;
        }

        public override Department GetById(int id)
        {
            return db.Departments.FirstOrDefault(d => d.Id == id);
        }
        public override IQueryable<Department> GetList()
        {
            return db.Departments.Where(x => x.Deleted == false).OrderBy(x => x.Name);
        }
        public Department GetByName(string name)
        {
            return db.Departments.FirstOrDefault(d => d.Name == name);
        }
        public int AddIfNotExists(string name)
        {
            Department dpt = GetByName(name);

            if (dpt == null)
            {
                dpt = new Department { Name = name, Deleted = false };
                AddOrUpdate(dpt);
            }

            return dpt.Id;
        }

        //public List<User> GetManagers(int departmentId)
        //{
        //    //string roleId = db.Roles.FirstOrDefault(r => r.Name == DefRoles.Manager).Id;
        //    //return db.Users.Where(u => u.DepartmentId == departmentId && u.Roles.Select(y => y.RoleId).Contains(roleId)).ToList();
        //    return new List<User>();
        //}
    }
}