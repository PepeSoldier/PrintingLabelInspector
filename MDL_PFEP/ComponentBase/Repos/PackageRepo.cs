using MDL_BASE.Enums;
using XLIB_COMMON.Repo;
using MDL_PFEP.Interface;
using MDL_PFEP.Model.ELDISY_PFEP;
using MDL_PFEP.Models.DEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MDL_iLOGIS.ComponentConfig.Entities;

namespace MDL_PFEP.Repo.DEF
{
    public class PackageRepo : RepoCommon
    {
        IDbContextPFEP_Eldisy db;

        public PackageRepo(IDbContextPFEP_Eldisy db) :base(db)
        {
            this.db = db;
        }
        public Package GetById(int id)
        {
            return db.Packages.FirstOrDefault(d => d.Id == id);
        }

        public Package GetBySapNumber(string code)
        {
            return db.Packages.FirstOrDefault(d => d.Code == code);
        }

        public IQueryable<Package> GetList()
        {
            return db.Packages.Where(x => x.Deleted == false).OrderByDescending(x => x.Id);
        }
        public IQueryable<Package> GetListWithFilters(Package filterObject)
        {
            return db.Packages.Where(x => 
                                    (x.Deleted == false) &&
                                    (x.Code.Contains(filterObject.Code) || filterObject.Code == null) &&
                                    (x.Name.Contains(filterObject.Name) || filterObject.Name == null) &&
                                    (x.UnitOfMeasure == filterObject.UnitOfMeasure || (int)filterObject.UnitOfMeasure < 0))
                               .OrderByDescending(x => x.Id);
        }
        

        public List<Package> GetPackageAutocompleteList(string Prefix)
        {
            return db.Packages.Where(x => x.Code.StartsWith(Prefix) && x.Deleted == false).Distinct().Take(5).ToList();
        }
    }

}