
using _MPPL_WEB_START.Areas.ONEPROD.Models;
using _MPPL_WEB_START.Areas.ONEPROD.ViewModels.APS;
using _MPPL_WEB_START.Areas.PRD.ViewModels;
using MDL_iLOGIS.ComponentConfig.Mappers;
using MDL_ONEPROD.ComponentMes.ViewModels;
using MDL_ONEPROD.Manager;
using MDL_ONEPROD.Model.Scheduling;
using MDL_PRD.Repo.Schedule;
using MDLX_CORE.ComponentCore.Entities;
using MDLX_CORE.ComponentCore.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas.ONEPROD.Controllers
{
    public partial class MESController : BaseController
    {
        public ActionResult WIP(int resourceGroupId)
        {
            ViewBag.ResourceGroupId = resourceGroupId;
            return View();
        }

        public JsonResult WIPGetData(int resourceGroupId)
        {
            DateTime dateNow = DateTime.Now.AddHours(-8); //new DateTime(2020, 4, 16, 10, 0, 0);
            List<Workorder> workorders = uowMes.WorkorderRepo.GetWorkorderOfResourceGroup(resourceGroupId, new DateTime(2000, 1, 1), dateNow.AddHours(26)).ToList();
            List<ViewModels.APS.WorkorderViewModel> workordersVM = new List<ViewModels.APS.WorkorderViewModel>();
            List<ResourceViewModel> resourcesVM = new List<ResourceViewModel>();

            foreach (Workorder wo in workorders)
            {
                if (wo.ParentWorkorder != null && wo.ParentWorkorder.Qty_Remain > 0)
                {
                    workordersVM.Add(new ViewModels.APS.WorkorderViewModel()
                    {
                        Id = wo.ParentWorkorder.Id,
                        ClientOrderId = wo.ClientOrder.Id,
                        ClientOrderNo = wo.ClientOrder.OrderNo,
                        LV = wo.ParentWorkorder.LV,
                        ItemCode = wo.ParentWorkorder.Item != null ? wo.ParentWorkorder.Item.Code : "",
                        ItemId = wo.ParentWorkorder.ItemId,
                        ProcessingTime = wo.ParentWorkorder.ProcessingTime,
                        Qty_Total = wo.ParentWorkorder.Qty_Total,
                        Qty_Produced = wo.ParentWorkorder.Qty_Produced,
                        Qty_Used = wo.ParentWorkorder.Qty_Used,
                        StartTime = wo.ParentWorkorder.StartTime,
                        EndTime = wo.ParentWorkorder.EndTime,
                        Status = wo.ParentWorkorder.Status,
                        ResourceId = wo.ParentWorkorder.ResourceId,
                        ResourceName = wo.ParentWorkorder.Resource != null ? wo.ParentWorkorder.Resource.Name : "",
                        Number = wo.ParentWorkorder.UniqueNumber,
                        ItemColor1 = wo.Item != null ? wo.Item.Color1 : "",
                        ItemColor2 = wo.Item != null ? wo.Item.Color2 : "",
                        Param1 = wo.ItemId != null ? (int)wo.ItemId : 0
                    });
                }
            }

            List<int> resourcesIds = workorders.Select(x => x.ParentWorkorder.Resource.Id).Distinct().ToList();
            foreach (int resourceId in resourcesIds)
            {
                var res = workorders.Select(x => x.ParentWorkorder.Resource).Where(x => x.Id == resourceId).FirstOrDefault();

                resourcesVM.Add(new ResourceViewModel()
                {
                    Id = res.Id,
                    Name = res.Name,
                });
            }
            resourcesVM = resourcesVM.OrderBy(x => x.Name).ToList();

            List<ItemInventory> itemInventory = new TaskStockMonitorManager(uowMes).GetCurrentStock(resourceGroupId);

            List<ItemInventoryViewModel> itemGroups = itemInventory
                .GroupBy(x => x.Item.ItemGroup)
                //.Select(x => x.Item.ItemGroup).Distinct().
                .Select(x => new ItemInventoryViewModel()
                {
                    Id = x.Key.Id,
                    Name = x.Key.Name,
                    Qty = x.Sum(y => y.Stock)
                })
                .ToList();

            List<ItemInventoryViewModel> items = itemInventory
                .Select(x => new ItemInventoryViewModel()
                {
                    Id = x.Item.Id,
                    Code = x.Item.Code,
                    Name = x.Item.Name,
                    Qty = x.Stock,
                    ItemGroupId = x.Item.ItemGroupId??0,
                    CoveredSeconds = 0,
                    Color1 = x.Item.Color1,
                    Color2 = x.Item.Color2,
                })
                .ToList();

            //var vv = hhh
            //    .Join(uowMes.WorkorderRepo.GetList(),
            //        wo => new { OrderNo = wo.ClientOrder.OrderNo, ResourceGroupId = resourceGroupId },
            //        woj => new { OrderNo = woj.ClientOrder.OrderNo, ResourceGroupId = woj.Resource.ResourceGroupId },
            //        (wo, woj) => new { WOJ = woj, WO = wo })
            //    .Where(x => x. == workplaceId &&
            //        x.PLOG.TimeStamp >= dateFrom &&
            //        x.PLOG.TimeStamp < dateTo)
            //    .Select(x => new
            //    {
            //        ClientWorkOrderNumber = x.PLOG.ClientWorkOrderNumber,
            //    });

            return Json(new { workordersVM, resourcesVM, itemGroups, items });
        }
    }

    public class ItemInventoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Qty { get; set; }
        public int ItemGroupId { get; set; }
        public int CoveredSeconds { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
    }
}