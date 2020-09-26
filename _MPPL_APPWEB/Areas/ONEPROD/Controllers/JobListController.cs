using _MPPL_WEB_START.Areas.ONEPROD.Models;
using MDLX_CORE.ComponentCore.Entities;
using MDLX_CORE.ComponentCore.Repos;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_PFEP.Interface;
using MDL_PFEP.Repo.PFEP;
using MDL_PRD.Interface;
using MDL_WMS.ComponentConfig.Repos;
using MDLX_MASTERDATA.Repos;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MDLX_MASTERDATA.Entities;
using MDL_ONEPROD.ComponentMes.ViewModels;
using MDL_ONEPROD.ComponentCORE.ViewModels;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    public class JobListController : Controller
    {
        ProductionOrderRepo repo;
        IDbContextiLOGIS dbiLOGIS;
        public JobListController(IDbContextPRD db, IDbContextiLOGIS dbiLOGIS)
        {
            repo = new ProductionOrderRepo(db);
            this.dbiLOGIS = dbiLOGIS;
        }

        // GET: ONEPROD/JobList
        public ActionResult JobList(int port, string workstationIds, string templateName = "10x.49", int version = 1, bool dropLayout = false)
        {
            ViewBag.Port = port;
            ViewBag.WorkstationIds = workstationIds;
            ViewBag.TemplateName = templateName;
            ViewBag.DropLayout = dropLayout;
            
            if(version > 1)
            {
                return View("JobList" + version.ToString());
            }
            else
            {
                return View();
            }
        }

        public JsonResult GetItems(string workstationIds, string barcode = "")
        {
            //barcode = "01539210000830014795";
            if(barcode.Length < 19) { return Json(-1); }

            string serialNo  = barcode.Substring(11, 8);
            ProductionOrder po = repo.GetBySerialNo(Convert.ToInt32(serialNo));

            JobListViewModel vm = new JobListViewModel();
            vm.ItemVersion = barcode.Substring(1, 9);
            vm.ItemSerialNo = serialNo;

            if (po != null)
            {
                vm.ItemName = po.Pnc.Name;
                vm.ItemId = po.Pnc.Id;
                vm.ItemCode = po.Pnc.Code;
                vm.WorkorderNo = po.OrderNumber;
                vm.WorkorderQtyPlanned = po.QtyPlanned;
                vm.JobListItems = new List<JobListItem>();
                vm.JobListDataFound = true;

                WorkstationItemRepo awRepo = new WorkstationItemRepo(dbiLOGIS);
                BomWorkorderRepo bwRepo = new BomWorkorderRepo(dbiLOGIS);
                
                int[] wIds = Array.ConvertAll(workstationIds.Split(','), s => int.Parse(s));
                List<int> workstItemIds = awRepo.GetItemIdsByWorkstationIds(wIds).OrderBy(x=>x).ToList();
                vm.JobListItems = bwRepo.GetItemsForWorkorderAndWorkstation(po, workstItemIds);
                
                if(vm.JobListItems == null || vm.JobListItems.Count < 1)
                {
                    BomRepo bomRepo = new BomRepo(dbiLOGIS);
                    vm.JobListDataFound = false;
                    vm.JobListItems = bomRepo.GetItemsForPncAndWorkstation(po.Pnc, workstItemIds);
                }

                return Json(new { data = vm, error = false, message = vm.JobListDataFound? string.Empty : "BOM data" });
            }
            else
            {
                return Json(new { data = string.Empty, error = true, message = "Zlecenie nie znalezione" });
            }

        }

        public JsonResult GetItemsByPrefixes(string[] prefixes, string barcode = "")
        {
            //barcode = "01539210000830014795";
            if (barcode.Length < 19) { return Json(-1); }

            string serialNo = barcode.Substring(11, 8);
            ProductionOrder po = repo.GetBySerialNo(Convert.ToInt32(serialNo));

            JobListViewModel vm = new JobListViewModel();
            vm.ItemVersion = barcode.Substring(1, 9);
            vm.ItemSerialNo = serialNo;

            if (po != null)
            {
                vm.ItemName = po.Pnc.Name;
                vm.ItemId = po.Pnc.Id;
                vm.ItemCode = po.Pnc.Code;
                vm.WorkorderNo = po.OrderNumber;
                vm.WorkorderQtyPlanned = po.QtyPlanned;
                vm.JobListItems = new List<JobListItem>();
                vm.JobListDataFound = true;

                WorkstationItemRepo awRepo = new WorkstationItemRepo(dbiLOGIS);
                BomWorkorderRepo bwRepo = new BomWorkorderRepo(dbiLOGIS);

                vm.JobListItems = bwRepo.GetItemsForWorkorderAndPrefixes(po, prefixes.ToList());
                if (vm.JobListItems == null || vm.JobListItems.Count < 1)
                {
                    BomRepo bomRepo = new BomRepo(dbiLOGIS);
                    vm.JobListDataFound = false;
                    vm.JobListItems = bomRepo.GetItemsForWorkorderAndPrefixes(po.Pnc, prefixes.ToList());
                }

                return Json(new { data = vm, error = false, message = vm.JobListDataFound ? string.Empty : "BOM data" });
            }
            else
            {
                return Json(new { data = string.Empty, error = true, message = "Zlecenie nie znalezione" });
            }

        }

        public JsonResult GetKitItems(string barcode = "", string prefix = "")
        {
            //barcode = "01539210000830014795";
            
            
            
            if(barcode.Length < 19) { return Json(-1); }

            string serialNo  = barcode.Substring(11, 8);
            ProductionOrder po = repo.GetBySerialNo(Convert.ToInt32(serialNo));

            JobListViewModel vm = new JobListViewModel();
            vm.ItemVersion = barcode.Substring(1, 9);
            vm.ItemSerialNo = serialNo;

            if (po != null)
            {
                BomRepo bomRepo = new BomRepo(dbiLOGIS);
                Item item = bomRepo.GetItemsForPncAndPrefix(po.Pnc, prefix).Take(1).FirstOrDefault();              

                vm.ItemName = item.Name;
                vm.ItemId = item.Id;
                vm.ItemCode = item.Code;
                vm.WorkorderNo = po.OrderNumber;
                vm.WorkorderQtyPlanned = po.QtyPlanned;
                vm.JobListItems = new List<JobListItem>();
                vm.JobListDataFound = true;
                vm.JobListItems = bomRepo.GetChildsOfItem(item.Id).Select(x => new JobListItem() { 
                    ItemId = x.Anc.Id,
                    ItemCode = x.Anc.Code,
                    ItemName = x.Anc.Name,
                    Prefix = x.Prefix,
                    Qty = x.PCS,
                    PhotoPosition = 1,
                    ParentItemId = item.Id
                }).ToList();

                return Json(new { data = vm, error = false, message = vm.JobListDataFound? string.Empty : "BOM data" });
            }
            else
            {
                return Json(new { data = string.Empty, error = true, message = "Zlecenie nie znalezione" });
            }

        }

        public ActionResult LoadTemplate(string templateName)
        {
            return View("JobListTemplate_" + templateName);
        }

        public ActionResult JobListMultiPage(int port, string workstationIds, int version = 1)
        {
            ViewBag.Port = port;
            ViewBag.WorkstationIds = workstationIds;
            ViewBag.Version = version;

            return View();
        }

        [HttpPost]
        public JsonResult GetSelectedWorkstationsName(string workstationIds)
        {
            RepoWorkstation wrkstRepo = new RepoWorkstation(dbiLOGIS);
            int[] wIds = Array.ConvertAll(workstationIds.Split(','), s => int.Parse(s));
            string wrkstNames = wrkstRepo.GetWorkstationsNames(wIds);

            return Json(wrkstNames);
        }
        [HttpGet]
        public JsonResult GetBarcode(string barcode)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<JobListHub>();
            //context.Clients.All.broadcastMessage(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            context.Clients.All.broadcastMessage(barcode);
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult TCPBarcodeReceived(string barcode, string workstationName)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<JobListHub>();
            //context.Clients.All.broadcastMessage(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //context.Clients.All.broadcastMessage(barcode);
            context.Clients.Group(workstationName).broadcastMessage(barcode);
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ChangePage(int val, string workstationName)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<JobListMultiPageHub>();
            context.Clients.Group(workstationName).broadcastMessage(val.ToString());
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MobileBarcode(string port)
        {
            ViewBag.port = port;
            return View();
        }
    }
}