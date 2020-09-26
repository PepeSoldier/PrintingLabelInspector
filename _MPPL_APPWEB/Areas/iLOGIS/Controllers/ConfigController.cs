using _MPPL_WEB_START.Areas._APPWEB.Models;
using _MPPL_WEB_START.Areas.iLOGIS.ViewModels;
using MDL_BASE.Models;
using MDL_BASE.Models.Base;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentConfig.Entities;
using MDL_iLOGIS.ComponentConfig.EntityInterfaces;
using MDL_iLOGIS.ComponentConfig.Mappers;
using MDL_iLOGIS.ComponentConfig.Repos;
using MDL_iLOGIS.ComponentConfig.ViewModels;
using MDL_iLOGIS.ComponentCore.Enums;
using MDL_iLOGIS.ComponentWMS.Enums;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using MDLX_CORE.Model;
using MDLX_CORE.Model.PrintModels;
using MDLX_MASTERDATA.Entities;
using Microsoft.AspNet.Identity;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using XLIB_COMMON.Model;
using XLIB_COMMON.Repo.Base;
using XLIB_COMMON.Repo.IDENTITY;

namespace _MPPL_WEB_START.Areas.iLOGIS.Controllers
{
    [Authorize]
    public class ConfigController : Controller
    {
        readonly IDbContextiLOGIS db;
        private UnitOfWork_iLogis uow;
        private ChangeLogRepo changeLogRepo;
        private UserRepo userRepo;
        public ConfigController(IDbContextiLOGIS db, IUserStore<User, string> userStore)
        {
            this.db = db;
            ViewBag.Skin = "nasaSkin";
            uow = new UnitOfWork_iLogis(db);
            userRepo = new UserRepo(userStore, db);
            changeLogRepo = new ChangeLogRepo(db);
        }

        // GET: iLOGIS/Config
        public ActionResult Index()
        {
            return View();
        }

        // WORKSTATION
        public ActionResult WorkstationItem()
        {
            WorkstationItem vm = new WorkstationItem();
            return View();
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public ActionResult WorkstationItemDelete(int id)
        {
            WorkstationItem wI = uow.WorkstationItemRepo.GetById(id);
            uow.WorkstationItemRepo.Delete(wI);
            return Json("");
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult WorkstationItemUpdate(WorkstationItem item)
        {
            if (item.Id > 0)
            {
                SaveHistoryWorkstationItem(item, false);
                uow.WorkstationItemRepo.AddOrUpdate(item);
            }
            else
            {
                uow.WorkstationItemRepo.AddOrUpdate(item);
                SaveHistoryWorkstationItem(item, true);
            }

            item = uow.WorkstationItemRepo.GetById(item.Id);
            WorkstationItemViewModel vm = new WorkstationItemViewModel()
            {
                Id = item.Id,
                WorkstationId = item.Workstation.Id,
                WorkstationName = item.Workstation.Name,
                LineName = item.Workstation.Line.Name,
                ItemWMSId = item.ItemWMS.Id,
                ItemCode = item.ItemWMS.Item.Code,
                ItemName = item.ItemWMS.Item.Name,
                MaxBomQty = item.MaxBomQty,
                MaxPackages = item.MaxPackages,
                SafetyStock = item.SafetyStock,
                CheckOnly = item.CheckOnly,
                PutTo = item.PutTo
            };
            return Json(vm);
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_OPERATOR)]
        public JsonResult WorkstationItemAddOrUpdate(int itemWMSId, string lineName, string workstationName, string putTo)
        {
            Workstation workstation = uow.WorkstationRepo.GetList().Where(x => x.Line.Name == lineName && x.Name == workstationName).FirstOrDefault();

            if (workstation != null) 
            {
                WorkstationItem wi = uow.WorkstationItemRepo.GetList().Where(x => x.Workstation.Line.Name == lineName && x.ItemWMSId == itemWMSId).FirstOrDefault();

                if(wi == null)
                    wi = new WorkstationItem();

                wi.ItemWMSId = itemWMSId;
                wi.WorkstationId = workstation.Id;
                wi.PutTo = putTo;
                wi.CheckOnly = false;

                uow.WorkstationItemRepo.AddOrUpdate(wi);
                SaveHistoryWorkstationItem(wi, true);

                return Json(iLogisStatus.NoError);
            }
            else
            {
                return Json(iLogisStatus.WorkstationNotFoud);
            }
        }
        [HttpPost]
        public JsonResult WorkstationItemGetList(WorkstationItemViewModel item, int pageIndex, int pageSize)
        {
            WorkstationItem filter = new WorkstationItem()
            {
                WorkstationId = item.WorkstationId,
                ItemWMS = new ItemWMS()
                {
                    Item = new Item()
                    {
                        Code = item.ItemCode,
                        Name = item.ItemName
                    }
                },
                Workstation = new Workstation()
                {
                    Name = item.WorkstationName,
                    Line = new Resource2() { Name = item.LineName }
                },
                MaxBomQty = item.MaxBomQty,
                MaxPackages = item.MaxPackages,
                SafetyStock = item.SafetyStock,
            };

            IQueryable<WorkstationItem> query = uow.WorkstationItemRepo.GetList(filter);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();

            List<WorkstationItemViewModel> list = query.Skip(startIndex).Take(pageSize)
                .Select(x => new WorkstationItemViewModel()
                {
                    Id = x.Id,
                    WorkstationId = x.Workstation.Id,
                    WorkstationName = x.Workstation.Name,
                    LineName = x.Workstation.Line.Name,
                    ItemWMSId = x.ItemWMS.Id,
                    ItemCode = x.ItemWMS.Item.Code,
                    ItemName = x.ItemWMS.Item.Name,
                    MaxBomQty = x.MaxBomQty,
                    MaxPackages = x.MaxPackages,
                    SafetyStock = x.SafetyStock,
                    CheckOnly = x.CheckOnly,
                    PutTo = x.PutTo
                })
                .ToList();

            return Json(new { data = list, itemsCount });
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult WorkstationItemGroupUpdate(WorkstationItem item, IEnumerable<int> ids)
        {
            List<WorkstationItem> wiList = uow.WorkstationItemRepo.GetList().Where(x => ids.Contains(x.Id)).ToList();

            var changes = uow.WorkstationItemRepo.GetChanges(item);

            foreach (WorkstationItem wi in wiList)
            {
                uow.WorkstationItemRepo.SetChanges(wi, changes);
                SaveHistoryWorkstationItem(wi, false);
                uow.WorkstationItemRepo.AddOrUpdate(wi);
            }
            uow.SaveChanges();

            //uow.WorkstationItemRepo.BulkUpdate(item, ids, "[iLOGIS].[CONFIG_WorkstationItem]");

            return Json(item);
        }

        //ItemWMS
        public ActionResult ItemWMS()
        {
            ItemWMS vm = new ItemWMS();
            return View(vm);
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public ActionResult ItemWMSDelete(int id)
        {
            ItemWMS wI = uow.ItemWMSRepo.GetById(id);
            uow.ItemWMSRepo.Delete(wI);
            return Json("");
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult ItemWMSUpdate(ItemWMS item)
        {
            ItemWMS objInDB = uow.ItemWMSRepo.GetByIdAsNoTracking(item.Id);
            ObjectsComparer oc = new ObjectsComparer();
            oc.DetectChanges(objInDB, item, item.Id);
            changeLogRepo.AddChangeLogs(oc.ObjectDataChanges, User.Identity.GetUserId());

            uow.ItemWMSRepo.AddOrUpdate(item);
            return Json(item);
        }
        [HttpPost]
        public JsonResult ItemWMSGetList(ItemWMS item)
        {
            //List<WorkstationItem> WorkstationItemList = uowWMS.WorkstationItemRepo.GetList(part).ToList();
            List<ItemWMS> ItemWMSList = new List<ItemWMS>() {
                new ItemWMS(){
                    Item = new  Item() {Id = 1, Name = "Item 1" },
                    Id = 1,
                },
                new ItemWMS(){
                    Item = new  Item() {Id = 1, Name = "Item 2" },
                    Id = 2
                },
                new ItemWMS(){
                    Item = new  Item() {Id = 1, Name = "Item 3" },
                    Id = 3
                },
                new ItemWMS(){
                    Item = new  Item() {Id = 1, Name = "Item 4" },
                    Id = 4
                }
            };
            return Json(ItemWMSList);
        }
        [HttpPost]
        public JsonResult ItemWMSAutocomplete(string prefix)
        {
            List<AutocompleteViewModel> acData = uow.ItemWMSRepo.GetList()
                .Where(x => x.Item.Code.StartsWith(prefix))
                .OrderBy(x => x.Item.Code)
                .Take(10)
                .Select(x => new AutocompleteViewModel { TextField = x.Item.Code, ValueField = x.Id.ToString(), Data1 = x.Item.Name, Data6 = (int)x.Item.UnitOfMeasure })
                .ToList();

            if (acData.Count <= 0)
            {
                acData = uow.ItemWMSRepo.GetList()
                .Where(x => x.Item.Name.Contains(prefix))
                .OrderBy(x => x.Item.Code)
                .Take(10)
                .Select(x => new AutocompleteViewModel { TextField = x.Item.Code, ValueField = x.Id.ToString(), Data1 = x.Item.Name, Data6 = (int)x.Item.UnitOfMeasure })
                .ToList();
            }

            return Json(acData);
        }

        public ActionResult ItemWMSCopySettings()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ItemWMSCopySettings(string itemCode, string itemCodeNew)
        {
            ItemWMS itemWMS = uow.ItemWMSRepo.GetByCode(itemCode);
            ItemWMS itemWMSNew = uow.ItemWMSRepo.GetByCode(itemCodeNew);
            bool done = false;

            if (itemWMS != null && itemWMSNew != null)
            {
                CopyItemParameters(itemWMS, itemWMSNew);
                CopyItemWorkstations(itemWMS, itemWMSNew);
                CopyItemPackages(itemWMS, itemWMSNew);
                done = true;
            }

            return Json(done);
        }
        private void CopyItemPackages(ItemWMS itemWMS, ItemWMS itemWMSNew)
        {
            var packageItems = uow.PackageItemRepo.GetList().Where(x => x.ItemWMSId == itemWMS.Id).ToList();
            bool newItem = false;

            foreach (var packageItem in packageItems)
            {
                var packageItemOfNew = uow.PackageItemRepo.GetList()
                    .Where(x => x.ItemWMSId == itemWMSNew.Id && x.PackageId == packageItem.PackageId)
                    .FirstOrDefault();

                if (packageItemOfNew == null)
                {
                    packageItemOfNew = new PackageItem();
                    newItem = true;
                }

                packageItemOfNew.ItemWMSId = itemWMSNew.Id;
                packageItemOfNew.PackageId = packageItem.PackageId;
                packageItemOfNew.PalletD = packageItem.PalletD;
                packageItemOfNew.PalletH = packageItem.PalletH;
                packageItemOfNew.PalletW = packageItem.PalletW;
                packageItemOfNew.QtyPerPackage = packageItem.QtyPerPackage;
                packageItemOfNew.WeightGross = packageItem.WeightGross;
                packageItemOfNew.PackagesPerPallet = packageItem.PackagesPerPallet;
                packageItemOfNew.WarehouseId = packageItem.WarehouseId;
                packageItemOfNew.WarehouseLocationTypeId = packageItem.WarehouseLocationTypeId;
                packageItemOfNew.PickingStrategy = packageItem.PickingStrategy;


                if (newItem)
                {
                    uow.PackageItemRepo.AddOrUpdate(packageItemOfNew);
                    SaveHistoryPackageItem(packageItemOfNew, newItem);
                }
                else
                {
                    SaveHistoryPackageItem(packageItemOfNew, newItem);
                    uow.PackageItemRepo.AddOrUpdate(packageItemOfNew);
                }


            }
        }
        private void CopyItemWorkstations(ItemWMS itemWMS, ItemWMS itemWMSNew)
        {
            var workstationItems = uow.WorkstationItemRepo.GetList().Where(x => x.ItemWMSId == itemWMS.Id).ToList();
            bool newItem = false;

            foreach (var workstationItem in workstationItems)
            {
                var workstationItemOfNew = uow.WorkstationItemRepo.GetList()
                    .Where(x => x.ItemWMSId == itemWMSNew.Id && x.WorkstationId == workstationItem.WorkstationId)
                    .FirstOrDefault();

                if (workstationItemOfNew == null)
                {
                    workstationItemOfNew = new WorkstationItem();
                    newItem = true;
                }

                workstationItemOfNew.ItemWMSId = itemWMSNew.Id;
                workstationItemOfNew.WorkstationId = workstationItem.WorkstationId;
                workstationItemOfNew.CheckOnly = workstationItem.CheckOnly;
                workstationItemOfNew.MaxBomQty = workstationItem.MaxBomQty;
                workstationItemOfNew.MaxPackages = workstationItem.MaxPackages;
                workstationItemOfNew.SafetyStock = workstationItem.SafetyStock;

                if (newItem)
                {
                    uow.WorkstationItemRepo.AddOrUpdate(workstationItemOfNew);
                    SaveHistoryWorkstationItem(workstationItemOfNew, newItem);
                }
                else
                {
                    SaveHistoryWorkstationItem(workstationItemOfNew, newItem);
                    uow.WorkstationItemRepo.AddOrUpdate(workstationItemOfNew);
                }

            }
        }
        private void CopyItemParameters(ItemWMS itemWMS, ItemWMS itemWMSNew)
        {
            itemWMSNew.ABC = itemWMS.ABC;
            itemWMSNew.XYZ = itemWMS.XYZ;
            itemWMSNew.TrainNo = itemWMS.TrainNo;
            itemWMSNew.Weight = itemWMS.Weight;
            itemWMSNew.PickerNo = itemWMS.PickerNo;

            itemWMSNew.Item.Color = itemWMS.Item.Color;
            itemWMSNew.Item.Color1 = itemWMS.Item.Color1;
            itemWMSNew.Item.Color2 = itemWMS.Item.Color2;
            itemWMSNew.Item.Height = itemWMS.Item.Height;
            itemWMSNew.Item.IsCommon = itemWMS.Item.IsCommon;
            itemWMSNew.Item.ItemGroupId = itemWMS.Item.ItemGroupId;
            itemWMSNew.Item.Lenght = itemWMS.Item.Lenght;
            itemWMSNew.Item.ProcessId = itemWMS.Item.ProcessId;
            itemWMSNew.Item.ResourceGroupId = itemWMS.Item.ResourceGroupId;
            itemWMSNew.Item.Specific1 = itemWMS.Item.Specific1;
            itemWMSNew.Item.Specific2 = itemWMS.Item.Specific2;
            itemWMSNew.Item.Specific3 = itemWMS.Item.Specific3;
            itemWMSNew.Item.Specific4 = itemWMS.Item.Specific4;
            itemWMSNew.Item.Width = itemWMS.Item.Width;
            itemWMSNew.Item.Type = itemWMS.Item.Type;

            //itemWMSNew.BC = itemWMS.BC;
            //itemWMSNew.DEF = itemWMS.DEF;
            //itemWMSNew.T = itemWMS.T;
            //itemWMSNew.V = itemWMS.V;
            //itemWMSNew.W = itemWMS.W;

            uow.ItemWMSRepo.AddOrUpdate(itemWMSNew);
        }

        private void SaveHistoryPackageItem(PackageItem updatedItem, bool newItem)
        {
            PackageItem objInDB = uow.PackageItemRepo.GetByIdAsNoTracking(newItem ? 0 : updatedItem.Id);
            string itemCode = objInDB != null ? objInDB.ItemWMS.Item.Code : uow.ItemWMSRepo.GetById(updatedItem.ItemWMSId).Item.Code;

            ObjectsComparer oc = new ObjectsComparer();
            oc.Config.AddLogDependentField("PackageId", "Package.Name");
            oc.DetectChanges(objInDB, updatedItem, updatedItem.Id, updatedItem.ItemWMSId, "", itemCode);
            changeLogRepo.AddChangeLogs(oc.ObjectDataChanges, User.Identity.GetUserId());
        }
        private void SaveHistoryWorkstationItem(WorkstationItem updatedItem, bool newItem)
        {
            WorkstationItem objInDB = uow.WorkstationItemRepo.GetByIdAsNoTracking(newItem ? 0 : updatedItem.Id);
            string itemCode = objInDB != null ? objInDB.ItemWMS.Item.Code : uow.ItemWMSRepo.GetById(updatedItem.ItemWMSId).Item.Code;

            ObjectsComparer oc = new ObjectsComparer();
            oc.Config.AddLogDependentField("WorkstationId", "Workstation.Name");
            oc.DetectChanges(objInDB, updatedItem, updatedItem.Id, updatedItem.ItemWMSId, "", itemCode);
            changeLogRepo.AddChangeLogs(oc.ObjectDataChanges, User.Identity.GetUserId());
        }
        private void SaveHistoryItem(Item updatedItem)
        {
            Item objInDB = uow.ItemRepo.GetByIdAsNoTracking(updatedItem.Id);
            ObjectsComparer oc = new ObjectsComparer();
            oc.Config.ExcludedFields = new string[] { "CreatedDate", "New", "StartDate", "Old_Id", "OriginalName" };
            oc.DetectChanges((Item)objInDB, updatedItem, updatedItem.Id, updatedItem.Id, updatedItem.Code, updatedItem.Code);
            changeLogRepo.AddChangeLogs(oc.ObjectDataChanges, User.Identity.GetUserId());
        }
        private void SaveHistoryItemWMS(ItemWMS updatedItem)
        {
            ItemWMS objInDB = uow.ItemWMSRepo.GetByIdAsNoTracking(updatedItem.Id);
            ObjectsComparer oc = new ObjectsComparer();
            oc.Config.ExcludedFields = new string[] { "ItemId" };
            oc.DetectChanges(objInDB, updatedItem, updatedItem.Id, updatedItem.Id, updatedItem.Item.Code, updatedItem.Item.Code);
            changeLogRepo.AddChangeLogs(oc.ObjectDataChanges, User.Identity.GetUserId());
        }

        //ItemWMS
        public ActionResult Item()
        {
            ItemWMS vm = new ItemWMS();
            return View(vm);
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public ActionResult Item2Delete(int id)
        {
            ItemWMS item = uow.ItemWMSRepo.GetById(id);
            uow.ItemWMSRepo.Delete(item);
            return Json("");
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult Item2Update(ItemWMSGridViewModel item)
        {

            if (item.Id > 0)
            {
                ItemWMS itemWMS_temp1 = uow.ItemWMSRepo.GetById(item.Id);

                //if (itemWMS_temp1 == null)
                //{
                //    Item itemMasterData = uow.ItemRepo.GetByCode(item.Code);
                //    if (itemMasterData != null)
                //    {
                //        itemWMS_temp1 = new ItemWMS() { Item = itemMasterData, ItemId = itemMasterData.Id };
                //    }
                //}

                ItemWMS itemWMS = new ItemWMS();
                itemWMS.Item = new Item();

                ReflectionHelper.CopyProperties(item, itemWMS);
                ReflectionHelper.CopyProperties(item, itemWMS.Item);
                itemWMS.Item.Id = itemWMS_temp1.ItemId;

                SaveHistoryItem(itemWMS.Item);
                SaveHistoryItemWMS(itemWMS);

                itemWMS_temp1.Item.Code = item.Code;
                itemWMS_temp1.Item.Name = item.Name;
                itemWMS_temp1.Item.ItemGroupId = item.ItemGroupId > 0 ? (int?)item.ItemGroupId : null;
                itemWMS_temp1.PickerNo = item.PickerNo;
                itemWMS_temp1.TrainNo = item.TrainNo;
                itemWMS_temp1.Item.Type = item.Type;
                itemWMS_temp1.Weight = item.Weight;
                uow.ItemWMSRepo.AddOrUpdate(itemWMS_temp1);
            }

            return Json(item);
        }

        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult Item2GroupUpdate(ItemWMS item, IEnumerable<int> ids)
        {
            uow.ItemWMSRepo.BulkUpdate(item, ids, "[iLOGIS].[CONFIG_Item]");
            return Json(item);
        }
        [HttpPost]
        public JsonResult Item2GetList(ItemWMSGridViewModel filter, int pageIndex, int pageSize)
        {
            ItemWMS filter2 = new ItemWMS()
            {
                Item = new Item()
                {
                    Name = filter.Name,
                    Code = filter.Code,
                    DEF = filter.DEF,
                    BC = filter.BC,
                    PREFIX = filter.PREFIX,
                    Type = filter.Type,
                    UnitOfMeasure = filter.UnitOfMeasure,
                    StartDate = filter.StartDate != null ? Convert.ToDateTime(filter.StartDate) : new DateTime(),
                    ItemGroupId = filter.ItemGroupId > 0 ? (int?)filter.ItemGroupId : null
                },
                H = filter.H,
                PickerNo = filter.PickerNo,
                TrainNo = filter.TrainNo,
            };

            IQueryable<ItemWMS> query = uow.ItemWMSRepo.GetList(filter2);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();
            List<ItemWMSGridViewModel> items = query.Skip(startIndex).Take(pageSize).ToList<ItemWMSGridViewModel>();

            items.ForEach(x => x.StartDate = x.StartDateTmp.ToString("yyyy-MM-dd"));

            return Json(new { data = items, itemsCount });
        }

        //AutomaticRules
        public ActionResult AutomaticRule()
        {
            return View();
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public ActionResult AutomaticRuleDelete(int id)
        {
            AutomaticRule item = uow.AutomaticRuleRepo.GetById(id);
            uow.ItemWMSRepo.Delete(item);
            return Json("");
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult AutomaticRuleUpdate(AutomaticRule rule)
        {
            AutomaticRule objInDB = uow.AutomaticRuleRepo.GetByIdAsNoTracking(rule.Id);
            ObjectsComparer oc = new ObjectsComparer();
            oc.DetectChanges(objInDB, rule, rule.Id, rule.Id, rule.PREFIX, rule.PREFIX);
            changeLogRepo.AddChangeLogs(oc.ObjectDataChanges, User.Identity.GetUserId());

            rule.LastChange = DateTime.Now;
            rule.UserName = User.Identity.Name;
            uow.ItemWMSRepo.AddOrUpdate(rule);
            return Json(rule);
        }
        [HttpPost]
        public JsonResult AutomaticRuleGetList(AutomaticRule filter, int pageIndex, int pageSize)
        {
            filter.IsPackageType = false;
            IQueryable<AutomaticRule> query = uow.AutomaticRuleRepo.GetList(filter);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();
            List<AutomaticRule> items = query.Skip(startIndex).Take(pageSize).ToList();

            return Json(new { data = items, itemsCount });
        }
        [HttpPost]
        public JsonResult AutomaticRuleApply()
        {
            bool result = uow.AutomaticRuleRepo.Apply();
            return Json(result);
        }
        [HttpPost]
        public JsonResult AutomaticRuleApplyById(int id)
        {
            bool result = uow.AutomaticRuleRepo.ApplyById(id);
            return Json(result);
        }

        //AutomaticRulesPackage
        public ActionResult AutomaticRulePackage()
        {
            return View();
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public ActionResult AutomaticRulePackageDelete(int id)
        {
            AutomaticRule item = uow.AutomaticRuleRepo.GetById(id);
            uow.ItemWMSRepo.Delete(item);
            return Json("");
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult AutomaticRulePackageUpdate(AutomaticRule rule)
        {
            AutomaticRule objInDB = uow.AutomaticRuleRepo.GetByIdAsNoTracking(rule.Id);
            ObjectsComparer oc = new ObjectsComparer();
            oc.DetectChanges(objInDB, rule, rule.Id, rule.Id, rule.PREFIX, rule.PREFIX);
            changeLogRepo.AddChangeLogs(oc.ObjectDataChanges, User.Identity.GetUserId());

            rule.IsPackageType = true;
            rule.LastChange = DateTime.Now;
            rule.UserName = User.Identity.Name;
            uow.ItemWMSRepo.AddOrUpdate(rule);
            return Json(rule);
        }
        [HttpPost]
        public JsonResult AutomaticRulePackageGetList(AutomaticRule filter, int pageIndex, int pageSize)
        {
            filter.IsPackageType = true;
            IQueryable<AutomaticRule> query = uow.AutomaticRuleRepo.GetList(filter);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();
            List<AutomaticRule> items = query.Skip(startIndex).Take(pageSize).ToList();

            List<int?> packagesIds = items.Where(x => x.PackageId != null).Select(x => x.PackageId).ToList();
            List<Package> packages = uow.PackageRepo.GetList().Where(x => packagesIds.Contains(x.Id)).ToList();

            foreach (AutomaticRule item in items)
            {
                item.PackageName = packages.FirstOrDefault(x => x.Id == item.PackageId).Name;
            }

            return Json(new { data = items, itemsCount });
        }
        [HttpPost]
        public JsonResult AutomaticRulePackageApplyById(int id)
        {
            bool result = uow.AutomaticRuleRepo.ApplyPackageById(id);
            return Json(result);
        }

        //Package
        public ActionResult Package()
        {
            ViewBag.ShowPriceColumn = AppClient.appClient.SettingsILOGIS.ShowPackagePriceColumn;
            Package vm = new Package();
            return View();
        }
        [HttpPost]
        public ActionResult PackageDelete(int id)
        {
            Package wI = uow.PackageRepo.GetById(id);
            uow.PackageRepo.Delete(wI);
            return Json("");
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult PackageUpdate(Package item)
        {
            Package oldPart = uow.PackageRepo.GetByIdAsNoTracking(item.Id);
            ObjectsComparer oc = new ObjectsComparer();
            oc.DetectChanges(oldPart, item, item.Id, item.Id, item.Name, item.Name);
            changeLogRepo.AddChangeLogs(oc.ObjectDataChanges, User.Identity.GetUserId());

            uow.PackageRepo.AddOrUpdate(item);
            return Json(item);
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult PackageGroupUpdate(Package item, IEnumerable<int> ids)
        {
            uow.PackageRepo.BulkUpdate(item, ids, "[iLOGIS].[CONFIG_Package]");
            return Json(item);
        }
        [HttpPost]
        public JsonResult PackageGetList(Package filter, int pageIndex, int pageSize)
        {
            IQueryable<Package> query = uow.PackageRepo.GetList(filter);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();
            List<Package> items = query.Skip(startIndex).Take(pageSize).ToList();

            return Json(new { data = items, itemsCount });
        }
        [HttpPost, AllowAnonymous]
        public JsonResult PackageAutocomplete(string p, int? w, int? h, int? d)
        {
            List<AutocompleteViewModel> acData = uow.PackageRepo.GetList()
                .Where(x =>
                    (x.Deleted == false) &&
                    (p == null || x.Code.StartsWith(p) || x.Name.StartsWith(p)) &&
                    (w == null || x.Width == w) &&
                    (h == null || x.Height == h) &&
                    (d == null || x.Depth == d))
                .OrderBy(x => x.Code)
                .ThenBy(x => x.Width)
                .ThenBy(x => x.Height)
                .ThenBy(x => x.Depth)
                .Take(10)
                .Select(x => new AutocompleteViewModel
                {
                    TextField = x.Code,
                    ValueField = x.Id.ToString(),
                    Data1 = x.Name,
                    Data2 = x.Depth + "x" + x.Width + "x" + x.Height,
                    Data3 = x.FullPalletHeight.ToString(),
                    Data4 = x.PackagesPerPallet.ToString()
                })
                .ToList();

            return Json(acData);
        }

        //PackageItem
        public ActionResult PackageItem()
        {
            //PackageItem vm = new PackageItem();
            return View();
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public ActionResult PackageItemDelete(int id)
        {
            PackageItem wI = uow.PackageItemRepo.GetById(id);
            uow.PackageItemRepo.Delete(wI);
            return Json("");
        }
        //[HttpPost, AuthorizeRoles(DefRoles.ILOGIS_ADMIN, DefRoles.ILOGIS_OPERATOR)]
        [HttpPost, AllowAnonymous]
        public JsonResult PackageItemUpdate(PackageItem item)
        {
            if (item.Id > 0)
            {
                SaveHistoryPackageItem(item, false);
                uow.PackageItemRepo.AddOrUpdate(item);
            }
            else
            {
                uow.PackageItemRepo.AddOrUpdate(item);
                SaveHistoryPackageItem(item, true);
            }

            PackageItem pckgItm = uow.PackageItemRepo.GetById(item.Id);
            PackageItemViewModel vm = pckgItm.CastToViewModel<PackageItemViewModel>();
            return Json(vm);
        }

        [HttpPost, AllowAnonymous]
        public JsonResult PackageItemGetList(PackageItemViewModel filter, int pageIndex, int pageSize)
        {
            if (filter.PackageName != null)
            {
                if (filter.PackageName.StartsWith("p:"))
                {
                    filter.PackageName = filter.PackageName.Replace("p:", "");
                }
                else if (filter.PackageName.StartsWith("c:"))
                {
                    filter.PackageName = filter.PackageName.Replace("c:", "");
                    filter.PackageCode = filter.PackageName;
                    filter.PackageName = null;
                }
            }

            IQueryable<PackageItem> query = uow.PackageItemRepo.GetList(filter);
            int itemsCount = query.Count();
            int startIndex = (pageIndex - 1) * pageSize;

            List<PackageItemViewModel> items = query.Skip(startIndex).Take(pageSize).ToList<PackageItemViewModel>();

            return Json(new { data = items, itemsCount });
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult PackageItemGroupUpdate(PackageItem item, IEnumerable<int> ids)
        {
            List<PackageItem> packageItems = uow.PackageItemRepo.GetList().Where(x => ids.Contains(x.Id)).ToList();

            foreach (PackageItem pi in packageItems)
            {

            }

            uow.PackageItemRepo.BulkUpdate(item, ids, "[iLOGIS].[CONFIG_PackageItem]");
            return Json(item);
        }

        //Warehouse
        public ActionResult Warehouse()
        {
            Warehouse vm = new Warehouse();
            return View();
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public ActionResult WarehouseDelete(int id)
        {
            Warehouse wI = uow.WarehouseRepo.GetById(id);
            uow.WarehouseRepo.Delete(wI);
            return Json("");
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult WarehouseUpdate(Warehouse item)
        {
            Warehouse wh = uow.WarehouseRepo.GetById(item.Id);

            if (wh == null)
            {
                wh = uow.WarehouseRepo.CreateNew();
            }

            wh.Code = item.Code;
            wh.Name = item.Name;
            wh.ParentWarehouseId = item.ParentWarehouseId > 0 ? item.ParentWarehouseId : null;
            wh.LabelLayoutFileName = item.LabelLayoutFileName;
            wh.AccountingWarehouseId = item.AccountingWarehouseId > 0 && item.AccountingWarehouseId != item.Id ? item.AccountingWarehouseId : null;
            wh.isOutOfScore = item.isOutOfScore;
            wh.isMRP = item.isMRP;
            wh.WarehouseType = WarehouseTypeEnum.SubWarehouse;
            uow.WarehouseRepo.AddOrUpdate(wh);

            return Json(item);
        }
        [HttpPost, AllowAnonymous]
        public JsonResult WarehouseGetList(Warehouse filter, int pageIndex, int pageSize)
        {
            IQueryable<Warehouse> query = uow.WarehouseRepo.GetList(filter);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();
            List<Warehouse> items = query.Skip(startIndex).Take(pageSize).ToList();
            

            return Json(new { data = items, itemsCount });
        }

        //WarehouseItem
        public ActionResult WarehouseItem()
        {
            WarehouseItem vm = new WarehouseItem();
            return View();
        }
        //[HttpPost, Authorize(Roles = DefRoles.iLogisAdmin)]
        //public ActionResult WarehouseItemDelete(int id)
        //{
        //    WarehouseItem wI = uow.WarehouseItemRepo.GetById(id);
        //    uow.WarehouseItemRepo.Delete(wI);
        //    return Json("");
        //}
        //[HttpPost, Authorize(Roles = DefRoles.iLogisAdmin)]
        //public JsonResult WarehouseItemUpdate(WarehouseItem item)
        //{
        //    uow.WarehouseItemRepo.AddOrUpdate(item);
        //    return Json(item);
        //}
        //[HttpPost]
        //public JsonResult WarehouseItemGetList(WarehouseItem part, int pageIndex, int pageSize)
        //{
        //    IQueryable<WarehouseItem> query = uow.WarehouseItemRepo.GetList();
        //    int startIndex = (pageIndex - 1) * pageSize;
        //    int itemsCount = query.Count();
        //    List<WarehouseItem> items = query.Skip(startIndex).Take(pageSize).ToList();
        //    return Json(new { data = items, itemsCount });
        //}

        //WarehouseLocation
        public ActionResult WarehouseLocation()
        {
            WarehouseLocation vm = new WarehouseLocation();
            return View();
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public ActionResult WarehouseLocationDelete(int id)
        {
            WarehouseLocation wI = uow.WarehouseLocationRepo.GetById(id);
            uow.WarehouseLocationRepo.Delete(wI);
            return Json("");
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult WarehouseLocationUpdate(WarehouseLocation item)
        {
            WarehouseLocation wl = uow.WarehouseLocationRepo.GetById(item.Id);

            if (wl == null)
            {
                wl = uow.WarehouseLocationRepo.CreateNew();
            }

            wl.Name = item.Name;
            wl.ParentWarehouseLocationId = item.ParentWarehouseLocationId > 0 ? item.ParentWarehouseLocationId : null;
            wl.WarehouseId = item.WarehouseId;
            wl.TypeId = item.TypeId > 0 ? item.TypeId : null;
            wl.AvailableForPicker = item.AvailableForPicker;
            uow.WarehouseLocationRepo.AddOrUpdate(wl);

            return Json(item);
        }
        [HttpPost, AllowAnonymous]
        public JsonResult WarehouseLocationGetList(WarehouseLocation filter, int pageIndex, int pageSize)
        {
            IQueryable<WarehouseLocation> query = uow.WarehouseLocationRepo.GetList(filter);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();
            List<WarehouseLocation> items = query.Skip(startIndex).Take(pageSize).ToList();

            return Json(new { data = items, itemsCount });
        }

        //Transporter
        public ActionResult Transporter()
        {
            Transporter vm = new Transporter();
            return View();
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public ActionResult TransporterDelete(int id)
        {
            Transporter wI = uow.TransporterRepo.GetById(id);
            uow.TransporterRepo.Delete(wI);
            return Json("");
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult TransporterUpdate(Transporter item)
        {
            Transporter objInDB = uow.TransporterRepo.GetByIdAsNoTracking(item.Id);
            ObjectsComparer oc = new ObjectsComparer();
            oc.DetectChanges(objInDB, item, item.Id, 0, item.Name, "");
            changeLogRepo.AddChangeLogs(oc.ObjectDataChanges, User.Identity.GetUserId());

            uow.TransporterRepo.AddOrUpdate(item);
            return Json(item);
        }
        [HttpPost, AllowAnonymous]
        public JsonResult TransporterGetList(Transporter filter, int pageIndex = 1, int pageSize = 100)
        {
            IQueryable<Transporter> TransporterList = uow.TransporterRepo.GetList().OrderBy(x => x.Name);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = TransporterList.Count();
            List<Transporter> items = TransporterList.Skip(startIndex).Take(pageSize).ToList();
            //items.ForEach(x => { x.LoopQty = 100; });

            return Json(new { data = items, itemsCount });
        }

        //WarehouseLocationSort
        public ActionResult WarehouseLocationSort()
        {
            WarehouseLocationSort vm = new WarehouseLocationSort();
            return View();
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public ActionResult WarehouseLocationSortDelete(int id)
        {
            WarehouseLocationSort wLs = uow.WarehouseLocationSortRepo.GetById(id);
            uow.WarehouseLocationSortRepo.Delete(wLs);
            return Json("");
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult WarehouseLocationSortUpdate(WarehouseLocationSort item)
        {
            uow.WarehouseLocationSortRepo.AddOrUpdate(item);
            return Json(item);
        }
        [HttpPost]
        public JsonResult WarehouseLocationSortGetList(WarehouseLocationSort filter, int pageIndex, int pageSize)
        {
            IQueryable<WarehouseLocationSort> WarehouseLocationSortList = uow.WarehouseLocationSortRepo.GetList().OrderBy(x => x.RegalNumber);
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = WarehouseLocationSortList.Count();
            List<WarehouseLocationSort> items = WarehouseLocationSortList.Skip(startIndex).Take(pageSize).ToList();
            //items.ForEach(x => { x.LoopQty = 100; });

            return Json(new { data = items, itemsCount });
        }

        //WarehouseLocationType
        public ActionResult WarehouseLocationType()
        {
            WarehouseLocationType vm = new WarehouseLocationType();
            return View();
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public ActionResult WarehouseLocationTypeDelete(int id)
        {
            WarehouseLocationType whlocType = uow.WarehouseLocationTypeRepo.GetById(id);
            uow.WarehouseLocationTypeRepo.Delete(whlocType);
            return Json("");
        }
        [HttpPost, Authorize(Roles = DefRoles.ILOGIS_ADMIN)]
        public JsonResult WarehouseLocationTypeUpdate(WarehouseLocationType item)
        {
            uow.WarehouseLocationTypeRepo.AddOrUpdate(item);
            return Json(item);
        }
        [HttpPost, AllowAnonymous]
        public JsonResult WarehouseLocationTypeGetList(WarehouseLocationType filter, int pageIndex, int pageSize)
        {
            IQueryable<WarehouseLocationType> query = uow.WarehouseLocationTypeRepo.GetList();
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();
            List<WarehouseLocationType> items = query.Skip(startIndex).Take(pageSize).ToList();

            return Json(new { data = items, itemsCount });
        }

        [HttpPost, AllowAnonymous]
        public JsonResult PickingStrategyGetList()
        {
            List<SelectListItem> pickingStrategies = new List<SelectListItem>();
            //pickingStrategies.Add(new SelectListItem()
            //{
            //    Value = Convert.ToString((int)PickingStrategyEnum.Unknow),
            //    Text = "Niezdefiniowany"
            //});
            pickingStrategies.Add(new SelectListItem()
            {
                Value = Convert.ToString((int)PickingStrategyEnum.FullPackage),
                Text = "Całe opakowanie"
            });
            pickingStrategies.Add(new SelectListItem()
            {
                Value = Convert.ToString((int)PickingStrategyEnum.UpToOrderQty),
                Text = "Wyliczany"
            });

            return Json(pickingStrategies);
        }
        // LISTS FOR SELECTS
        [HttpPost, AllowAnonymous]
        public JsonResult ResourceGetList()
        {
            //List<Resource2> resList = uow.ResourceRepo.GetList().ToList();
            List<Resource2> resList = new List<Resource2>() {
                new Resource2(){
                    Name = "Res 1",
                    Id = 1
                },
                new Resource2(){
                    Name = "Res 2",
                    Id = 2,
                },
                new Resource2(){
                    Name = "Res 3",
                    Id = 3,
                }
            };
            return Json(resList);
        }

        [HttpPost, AllowAnonymous]
        public JsonResult ItemGetList()
        {
            //List<Item> itemList = uow.ItemRepo.GetList().ToList();
            List<Item> itemList = new List<Item>() {
                new Item(){
                    Name = "Item 1",
                    Id = 1
                },
                new Item(){
                    Name = "Item 2",
                    Id = 2
                }
            };
            return Json(itemList);
        }
        public ActionResult SaveExcel(int id)
        {
            ActionAsPdf pdfResult = null;
            DateTime dt1 = DateTime.Now;
            string dt = dt1.Year.ToString() + dt1.Month.ToString() + dt1.Day.ToString() + dt1.Hour.ToString() + dt1.Minute.ToString() + dt1.Second.ToString();

            pdfResult = new ActionAsPdf("Print", new { id, authorize = 1 })
            {
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = new Rotativa.Options.Margins(3, 3, 3, 3),
                FileName = "Data" + id + "_" + dt + ".xlsx"
            };
            return pdfResult;
        }

        public ActionResult PrintLabels()
        {
            
            return View();
        }
        [HttpPost]
        public JsonResult PrintLabels(string printerIp, string prefix, int startNumber, int endNumber, int step)
        {
            string labelDefinition = "^XA^FWN^FO50,80^BQN,2,10,270^FD000{{Code}}^FS^CF0,200^FO330,125^FD{{Code}}^FS^XZ";
            PrintLabelModelZEBRA printModel = new PrintLabelModelZEBRA(printerIp);
            step = step <= 0 ? 1 : step;
            for (int st = startNumber; st <= endNumber; st += step)
            {
                string startNumberString = st.ToString("D3");
                string labelText = prefix + startNumberString;
                printModel.PrepareLabel(labelDefinition, new LabelData() { Code = labelText });
                printModel.Print();
            }           
            return Json(0);
        }

        [HttpPost]
        public JsonResult PrintLabelsWh(string printerIp, int startNumber, int endNumber, int step)
        {
            string labelDefinition = "^XA^FWN^FO70,055^BQN,2,10,270^FDPPP{{Code}}^FS^CF0,60^FO85,300^FD{{Code}}^FS^FO470,055^BQN,2,410,270^FDPPP{{Barcode}}^FS^CF0,60^FO485,300^FD{{Barcode}}^FS^XZ";
            PrintLabelModelZEBRA printModel = new PrintLabelModelZEBRA(printerIp);
            step = step <= 0 ? 1 : step;
            for (int st = startNumber; st < endNumber; st += (step + 1))
            {
                string labelText = st.ToString("D4");
                string labelTextSec = (st + 1).ToString("D4");
                labelText = labelText.Substring(0, 2) + "-" + labelText.Substring(2, 2);
                labelTextSec = labelTextSec.Substring(0, 2) + "-" + labelTextSec.Substring(2, 2);
                printModel.PrepareLabel(labelDefinition, new LabelData() { Code = labelText, Barcode = labelTextSec });
                printModel.Print();
            }
            return Json(0);
        }
        [HttpPost]
        public JsonResult PrintLabelsWorkstation(string printerIp, string lineName, int startNumber, int endNumber, string specialChar, string flowrack)
        {
            string labelDefinition = 
                    "^XA^FWN^FO50,80^BQN,2,10,270^FD000{{Code}}" +
                    "^FS^CF0,90^FO320,105^FD{{MachineNumber}}" +
                    "^FS^CF0,90^FO320,230^FD{{SerialNumber}}" +
                    "^FS^CF0,90^FO530,230^FD{{Location}}" +
                    "^FS^XZ";

            PrintLabelModelZEBRA printModel = new PrintLabelModelZEBRA(printerIp);
            for (int i = startNumber; i <= endNumber; i ++)
            {
                string startNumberString = i.ToString();
                string labelText = lineName + "." + startNumberString + specialChar + (flowrack != null && flowrack.Length > 0? "." + flowrack : "");
                printModel.PrepareLabel(labelDefinition, new LabelData() { Code = labelText, MachineNumber = lineName, SerialNumber = i.ToString() + specialChar, Location = flowrack });
                printModel.Print();
            }
            
            return Json(0);
        }
       

    }
}