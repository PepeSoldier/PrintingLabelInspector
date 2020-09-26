using MDL_BASE.Enums;
using MDL_BASE.Interfaces;
using MDL_BASE.Models.IDENTITY;
using XLIB_COMMON.Repo;
using MDL_PFEP.ComponentPackingIntruction.Models;
using MDL_PFEP.Interface;
using MDL_PFEP.Model.ELDISY_PFEP;
using MDL_PFEP.Models.DEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_PFEP.Repo.ELDISY_PFEP
{
    public class PackingInstructionRepo : RepoGenericAbstract<PackingInstruction>
    {
        protected new IDbContextPFEP_Eldisy db;

        public PackingInstructionRepo(IDbContextPFEP_Eldisy db) : base(db)
        {
            this.db = db;
        }

        public int Add(PackingInstruction entity)
        {
            PackingInstruction item = db.PackingInstructions.OrderByDescending(x => x.Id).FirstOrDefault();
            entity.InstructionNumber = (item != null) ? item.Id + 1 : 1;
            base.Add(entity);

            return entity.InstructionNumber;
        }

        public override PackingInstruction GetById(int id)
        {
            return db.PackingInstructions.FirstOrDefault(x => x.Id == id);
        }
        public int GetNextId(int id)
        {
            try
            {
                return db.PackingInstructions.Where(d => d.Id > id).Take(1).Select(x => x.Id).FirstOrDefault();
            }
            catch
            {
                return 0;
            }
        }
        public int GetPrevId(int id)
        {
            try
            {
                return db.PackingInstructions.Where(d => d.Id < id).OrderByDescending(o=>o.Id).Take(1).Select(x => x.Id).FirstOrDefault();
            }
            catch
            {
                return 0;
            }
        }

        public override IQueryable<PackingInstruction> GetList()
        {
            return db.PackingInstructions.Where(x => x.Deleted == false).OrderByDescending(x => x.Id);
        }

        public PackingInstruction GetById_NoTracking(int id)
        {
            return db.PackingInstructions.AsNoTracking().FirstOrDefault(d => d.Id == id);
        }
        
        //repo nie powinno byc zależna od viewmodelu. Nie powinno w ogole wiedzieć że DataToJSGridViewModel istnieje.
        //trzeba tu zwrócić Iqueryable i zrobić troche w 
        public IQueryable<PackingInstructionFilterObject> GetListWithCorrections(PackingInstruction filter)
        {
            DateTime dt = new DateTime();

            IQueryable<PackingInstruction> filteredInstructions = db.PackingInstructions;

            if (filter.PackageCode != null && filter.PackageCode.Length > 0)
            {
                filteredInstructions = db.PackingInstructionPackages.Where(x => x.Package.Code == filter.PackageCode && !(x.Package.Deleted)).Select(p => p.PackingInstruction).Distinct();
            }

            filteredInstructions = filteredInstructions.Where(o =>
                            (o.Id == filter.Id || filter.Id == 0) &&
                            (o.ClientName == filter.ClientName || filter.ClientName == null) &&
                            (o.ClientProfileCode == filter.ClientProfileCode || filter.ClientProfileCode == null) &&
                            (o.CreatedDate >= filter.CreatedDate || filter.CreatedDate == dt) &&
                            (o.ProfileCode.Contains(filter.ProfileCode) || filter.ProfileCode == null) &&
                            (o.ProfileName.Contains(filter.ProfileName) || filter.ProfileName == null) &&
                            (o.InstructionVersion == (InstructionVersion)filter.InstructionVersion || (int)filter.InstructionVersion < 0));

            var instructions = (from packInstr in filteredInstructions
                                where packInstr.Deleted == false
                                join correction in db.Corrections on packInstr.Id equals correction.PackingInstructionId into corrList
                                select new PackingInstructionFilterObject
                                {
                                    Id = packInstr.Id,
                                    ClientName = packInstr.ClientName,
                                    ClientProfileCode = packInstr.ClientProfileCode,
                                    ProfileCode = packInstr.ProfileCode,
                                    ProfileName = packInstr.ProfileName,
                                    InstructionVersion = packInstr.InstructionVersion,
                                    CreatedDate = packInstr.CreatedDate,
                                    Examined = packInstr.Examined,
                                    Confirmed = packInstr.Confirmed,
                                    NumberOfCorrections = corrList.Where(x => x.Deleted == false).Count(),
                                });
            
             instructions = GetSortedQuery(instructions, filter.sortField, filter.sortOrder);

            return instructions;
        }
        private IQueryable<PackingInstructionFilterObject> GetSortedQuery(IQueryable<PackingInstructionFilterObject> query, string sortField, string sortOrder)
        {
            if(sortField != null)
            {
                if (sortOrder == "desc")
                {
                    switch (sortField)
                    {
                        case "CreatedDate":
                            return query.OrderByDescending(x => x.CreatedDate);
                        case "ProfileName":
                            return query.OrderByDescending(x => x.ProfileName);
                        case "Id":
                            return query.OrderByDescending(x => x.Id);
                        case "ClientName":
                            return query.OrderByDescending(x => x.ClientName);
                        case "ClientProfileCode":
                            return query.OrderByDescending(x => x.ClientProfileCode);
                        case "ProfileCode":
                            return query.OrderByDescending(x => x.ProfileCode);
                    }
                }
                else
                {
                    switch (sortField)
                    {
                        case "CreatedDate":
                            return query.OrderBy(x => x.CreatedDate);
                        case "ProfileName":
                            return query.OrderBy(x => x.ProfileName);
                        case "Id":
                           return query.OrderBy(x => x.Id);
                        case "ClientName":
                            return query.OrderBy(x => x.ClientName);
                        case "ClientProfileCode":
                            return query.OrderBy(x => x.ClientProfileCode);
                        case "ProfileCode":
                            return query.OrderBy(x => x.ProfileCode);
                    }
                }
            }
            return query;             
        }
        public IQueryable<PackingInstructionFilterObject> GetListWithCorrectionsBySap(string packageSapNumber)
        {
            //DataToJSGridViewModel item = new DataToJSGridViewModel();
            //var packages = (from packageList in db.Packages where packageList.Code == sapNumber where packageList.Deleted == false select packageList).AsEnumerable();
            //var instrItemsWithPackages = db.PackingInstructionPackages.Where(x => packages.Select(k => k.Id).Contains(x.PackageId)).AsEnumerable();
            //var instructions = (from packInstr in db.PackingInstructions
            //                  where packInstr.Deleted == false
            //                  join packingItems in db.PackingInstructionPackages on packInstr.Id equals packingItems.PackingInstructionId into packageItemList
            //                  join correction in db.Corrections on packInstr.Id equals correction.PackingInstructionId into corrList
            //                  select new PackingInstructionBrowseViewModel
            //                  {
            //                      Id = packInstr.Id,
            //                      ClientName = packInstr.ClientName,
            //                      ClientProfileCode = packInstr.ClientProfileCode,
            //                      ProfileCode = packInstr.ProfileCode,
            //                      InstructionVersion = packInstr.InstructionVersion,
            //                      CreatedDate = packInstr.CreatedDate,
            //                      Examined = packInstr.Examined,
            //                      Confirmed = packInstr.Confirmed,
            //                      NumberOfCorrections = corrList.Where(x => x.Deleted == false).Count(),
            //                  }).OrderByDescending(x => x.Id).AsEnumerable();

            var instructionsWithPackage = db.PackingInstructionPackages.Where(x => x.Package.Code == packageSapNumber && !(x.Package.Deleted)).Select(p=>p.PackingInstruction).Distinct();

            var instructions = (from packInstr in instructionsWithPackage
                                where !(packInstr.Deleted)
                                join correction in db.Corrections on packInstr.Id equals correction.PackingInstructionId into corrList
                                select new PackingInstructionFilterObject
                                {
                                    Id = packInstr.Id,
                                    ClientName = packInstr.ClientName,
                                    ClientProfileCode = packInstr.ClientProfileCode,
                                    ProfileCode = packInstr.ProfileCode,
                                    InstructionVersion = packInstr.InstructionVersion,
                                    CreatedDate = packInstr.CreatedDate,
                                    Examined = packInstr.Examined,
                                    Confirmed = packInstr.Confirmed,
                                    NumberOfCorrections = corrList.Where(x => x.Deleted == false).Count(),
                                });            
            
            return instructions;
        }

        public List<string> GetClientNameList(string prefix)
        {
            return db.PackingInstructions.Where(x => x.ClientName.StartsWith(prefix)).Distinct().Take(5).Select(x => x.ClientName).ToList();
        }

        //TODO: correction powinno byc w osobnym repo
        public int AddCorrection(Correction newObject)
        {
            base.Add(newObject);
            return newObject.Id;

        }
        public List<Correction> GetCorrectionsByInstructionId(int Id)
        {
            return db.Corrections.Where(x => x.PackingInstructionId == Id && x.Deleted == false).ToList();
        }
        public PackingInstruction Create(PackingInstruction item, User creator)
        {
            PackingInstruction packingInstr = item;

            packingInstr.CreatedDate = DateTime.Now;
            packingInstr.ConfirmedDate = new DateTime(1900, 01, 01);
            packingInstr.ExaminedDate = new DateTime(1900, 01, 01);
            packingInstr.Creator = creator;

            AddOrUpdate(packingInstr);

            return packingInstr;
        }
        public void AssignEditted(PackingInstruction old, PackingInstruction edit, User creator)
        {
            old.Creator = creator;
            old.CreatorId = creator.SuperVisorUserId;
            old.CreatedDate = DateTime.Now;
            old.Confirm = edit.Confirm;
            old.Confirmed = false;
            old.ConfirmId = edit.ConfirmId;
            old.Examiner = edit.Examiner;
            old.ExaminerId = edit.ExaminerId;
            old.Examined = false;

            old.InstructionVersion = edit.InstructionVersion;
            old.Description = edit.Description;
            old.AmountOnLayer = edit.AmountOnLayer;
            old.AmountOnBox = edit.AmountOnBox;
            old.AmountOnPallet = edit.AmountOnPallet;
            old.UnitOfMeasure = edit.UnitOfMeasure;
            old.AreaId = edit.AreaId;
            old.ProfileCode = edit.ProfileCode;
            old.ClientProfileCode = edit.ClientProfileCode;
            old.ProfileName = edit.ProfileName;
            old.ClientName = edit.ClientName;

            AddOrUpdate(old);
        }

        

    }
}