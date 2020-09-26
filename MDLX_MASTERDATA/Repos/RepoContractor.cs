using MDL_BASE.Interfaces;
using System.Linq;
using MDL_BASE.Models.MasterData;
using System.Collections.Generic;
using MDLX_MASTERDATA._Interfaces;
using XLIB_COMMON.Repo;
using System;

namespace MDLX_MASTERDATA.Repos
{
    public class RepoContractor : RepoGenericAbstract<Contractor>
    {
        protected new IDbContextMasterData db;
        
        public RepoContractor(IDbContextMasterData db) : base(db)
        {
            this.db = db;
        }

        public override Contractor GetById(int id)
        {
            return db.Contractors.FirstOrDefault(d => d.Id == id);
        }

        public override IQueryable<Contractor> GetList()
        {
            return db.Contractors.Where(x => x.Deleted == false).OrderBy(x => x.Name);
        }
        public IQueryable<Contractor> GetList(Contractor filter)
        {
            return db.Contractors.Where(x => 
                    x.Deleted == false &&
                    (filter.Code == null || x.Code == filter.Code) &&
                    (filter.Name == null || x.Name.StartsWith(filter.Code)) &&
                    (filter.Country == null || x.Country.StartsWith(filter.Country))
                )
                .OrderBy(x => x.Name);
        }

        public List<Contractor> GetContractorAutocompleteList(string prefix)
        {
            return db.Contractors.Where(x => x.Name.StartsWith(prefix) && x.Deleted == false).Distinct().Take(5).ToList();
        }

        public List<Contractor> GetContractorWithDeliveryItemsAutocompleteList(string prefix)
        {
            return db.Contractors.Where(x => x.Name.StartsWith(prefix) && x.Deleted == false).Distinct().Take(5).ToList();
        }

        public Contractor GetOrCreate(string code, string name)
        {
            Contractor c = db.Contractors.Where(x => x.Code == code && x.Name == name).FirstOrDefault();
            if (c == null) {
                c = new Contractor() { Name = name, Code = code };
                AddOrUpdate(c);
            }

            return c;
        }

        public Contractor Get(string code, string name)
        {
            return db.Contractors.FirstOrDefault(x => 
                (code == null || x.Code == code ) &&
                (name == null || x.Name == name)
            );
        }

        public Contractor GetbyName(string contractorName)
        {
            return db.Contractors.FirstOrDefault(x => x.Name == contractorName);
        }
    }
}