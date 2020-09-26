using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using _MPPL_WEB_START.Areas.AP.ViewModel;
using MDL_AP.Models.ActionPlan;
using MDL_AP.Repo;
using MDL_BASE.Models.IDENTITY;
using XLIB_COMMON.Repo.IDENTITY;
using _MPPL_WEB_START.Areas._APPWEB.Controllers;
using _MPPL_WEB_START.Migrations;
using MDL_AP.Interfaces;
using Microsoft.AspNet.Identity;
using XLIB_COMMON.Interface;

namespace _MPPL_WEB_START.Areas.AP.Controllers
{
    [Authorize]
    public class GanttchartController : BaseController
    {
        private IDbContextAP db;
        private UnitOfWorkActionPlan uow;
        

        public GanttchartController(IDbContextAP db)
        {
            this.db = db;
            uow = new UnitOfWorkActionPlan(db);
        }

        public ActionResult Show()
        {
            ActionBrowseViewModel vm = new ActionBrowseViewModel();
            vm.FilterObject = new MDL_AP.ComponentBase.Models.ActionFilterModel();

            vm.Workstations = new SelectList(uow.RepoWorkstation.GetList().ToList(), "Id", "Name");
            vm.Areas = new SelectList(uow.RepoArea.GetList().ToList(), "Id", "Name");
            vm.ShiftCodes = new SelectList(uow.RepoShiftCode.GetList().ToList(), "Id", "Name");
            vm.Departments = new SelectList(uow.RepoDepartment.GetList().ToList(), "Id", "Name");
            vm.Lines = new SelectList(uow.ResourceRepo.GetList().ToList(), "Id", "Name");
            vm.Categories = new SelectList(uow.RepoCategory.GetList().ToList(), "Id", "Name");
            vm.Types = new SelectList(uow.RepoType.GetList().ToList(), "Id", "Name");
            vm.FilterObject.ShowChildActions = false;

            return View(vm);
        }
        
        public JsonResult GetData()
        {
            GanntTasks ganttTasks = new GanntTasks();
            ganttTasks.data = new List<GanttData>();
            ganttTasks.links = new List<GanttDataLinks>();

            List<ActionModel> actions = uow.RepoAction.GetList().Where(x=>x.ParentActionId == 0).ToList();

            int linkId = 1;
            GetDataLoop(ganttTasks, actions, linkId);

            return Json(ganttTasks, JsonRequestBehavior.AllowGet);
        }
        private void GetDataLoop(GanntTasks ganttTasks, List<ActionModel> actions, int linkId)
        {
            List<ActionModel> subactions;
            foreach (ActionModel action in actions)
            {
                PrepareData(ganttTasks, action, linkId, action.ParentActionId);
                subactions = uow.RepoAction.GetChildrens(action.Id).ToList();

                if (subactions.Count > 0)
                {
                    GetDataLoop(ganttTasks, subactions, linkId);
                }
            }
        }
        private void PrepareData(GanntTasks ganttTasks, ActionModel action, int linkId, int parentActionId)
        {
            double status = (double)action.State;
            double progress = (double)(status / (double)4);

            ganttTasks.data.Add(new GanttData
            {
                id = action.Id,
                parent = parentActionId,
                progress = progress,
                text = action.Title,
                duration = Convert.ToInt32((action.PlannedEndDate - action.StartDate).TotalDays),
                sortorder = 10,
                start_date = action.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                open = false,
                color = uow.RepoAction.GetColor(action),
                responsibleDept = (action.Department != null) ? action.Department.Name : string.Empty,
                responsibleUser = (action.Assigned != null) ? action.Assigned.FullName : string.Empty
            });

            if(action.ParentActionId > 0)
            {
                ganttTasks.links.Add(new GanttDataLinks
                {
                    id = linkId,
                    source = parentActionId,
                    target = action.Id,
                    type = "1"
                });

                linkId++;
            }
        }

        [HttpPost]
        public JsonResult GetDataPost(ActionBrowseViewModel vm)
        {
            List<GanttData> data = new List<GanttData>();
            List<GanttDataLinks> links = new List<GanttDataLinks>();

            GanntTasks ganttTasks = new GanntTasks();
            ganttTasks.data = data;
            ganttTasks.links = links;

            List<ActionModel> actions = uow.RepoAction.GetList(vm.FilterObject, vm.FilterObject.State).ToList();

            int linkId = 1;
            GetDataLoop(ganttTasks, actions, linkId);

            return Json(ganttTasks);
        }

        public class GanntTasks
        {
            public List<GanttData> data { get; set; }
            public List<GanttDataLinks> links { get; set; }
        }

        public class GanttData
        {
            public int id { get; set; }
            public string text { get; set; }
            public string start_date { get; set; }
            public int duration { get; set; }
            public double progress { get; set; }
            public int sortorder { get; set; }
            public int parent { get; set; }
            public bool open { get; set; }
            public string color { get; set; }
            public string responsibleDept { get; set; }
            public string responsibleUser { get; set; }
            
        }

        public class GanttDataLinks
        {
            public int id { get; set; }
            public int source { get; set; }
            public int target { get; set; }
            public string type { get; set; }   
        }


    }
}
