using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using _MPPL_WEB_START.Migrations;
using MDL_PFEP.Repo;
using MDL_PFEP.Models.DEF;
using Newtonsoft.Json;

using MDLX_BASE.Repo;
using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA.Entities;

namespace _MPPL_WEB_START.Areas.PFEP.Controllers
{
    public class WorkstationController : Controller
    {
        private DbContextAPP_ElectroluxPLV db;
        private GenericRepository<Workstation> repository;
        //private GenericRepository<PncType> repositoryPncType;
        private GenericRepository<Resource2> repositoryLine;

        public WorkstationController()
        {
            db = DbContextAPP_ElectroluxPLV.Create();
            repository = new GenericRepository<Workstation>(db);
            //repositoryPncType = new GenericRepository<PncType>(db);
            repositoryLine = new GenericRepository<Resource2>(db);
        }

        // GET: Workstation
        public async Task<ActionResult> Index()
        {
            //var resultsType = await repositoryPncType.Get();
            //var resultsSetType = resultsType as IEnumerable<PncType>;
            Dictionary<string, string> listType = new Dictionary<string, string>();//resultsSetType.ToDictionary(x => x.Id.ToString(), x => x.Name);
            ViewBag.ListType = JsonConvert.SerializeObject(listType);

            var resultsSetLine = await repositoryLine.Get() as IEnumerable<Resource2>;
            Dictionary<string, string> listLine = resultsSetLine.ToDictionary(x => x.Id.ToString(), x => x.Name);
            ViewBag.ListLine = JsonConvert.SerializeObject(listLine);

            return View(await repository.Get());
        }

        // POST: Workstation/Edit
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,SortOrder,LineId,TypeId")] Workstation workstation)
        {
            if (ModelState.IsValid)
            {
                int success = 0;
                if (workstation.Id != 0)
                {
                    success = await repository.Update(workstation);
                }
                else
                {
                    success = await repository.Insert(workstation);
                }
                if (success == 1)
                {
                    return Json(new { result = "success", id = workstation.Id });
                }
            }
            return Json(new { result = "error" });
        }

        // POST: Workstation/Editdata
        [HttpPost]
        public async Task<ActionResult> Editdata(string data)
        {
            string[] lines = data.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            int success = 0;
            foreach (string line in lines)
            {
                Workstation workstation = new Workstation();
                if (line == "")
                {
                    continue;
                }
                string[] elements = line.Split('\t');

                if (elements.Count() < 4)
                {
                    continue;
                }

                workstation.Name = elements[0];
                workstation.SortOrder = Int32.Parse(elements[1]);

                string tmp1 = elements[2];
                //IEnumerable<PncType> findType = repositoryPncType.GetSynchronous(o => o.Name == tmp1);
                //if (findType.Count() == 0)
                //{
                //    continue;
                //}
                ////workstation.Type = findType.First();

                string tmp2 = elements[3];
                IEnumerable<Resource2> findLine = repositoryLine.GetSynchronous(o => o.Name == tmp2);
                if (findLine.Count() == 0)
                {
                    continue;
                }
                workstation.Line = findLine.First();

                success = await repository.Insert(workstation);
            }

            if (success > 0)
            {
                return Json(new { result = "success" });
            }

            return Json(new { result = "error" });
        }

        // POST: Workstation/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            await repository.Delete(id);
            return Json(new { result = "success" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
