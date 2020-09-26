using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _MPPL_WEB_START.Migrations;
using MDL_PFEP.Repo;
//using MDL_PFEP.Models.PFEP;
using MDL_PFEP.Models.DEF;
using MDLX_BASE.Repo;
using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA.Entities;

namespace _MPPL_WEB_START.Areas.PFEP.Controllers
{
    public class LineController : Controller
    {
        private DbContextAPP_ElectroluxPLV db;
        private GenericRepository<Resource2> repository;

        public LineController()
        {
            db = DbContextAPP_ElectroluxPLV.Create();
            repository = new GenericRepository<Resource2>(db);
        }

        // GET: Line
        public async Task<ActionResult> Index()
        {
            return View(await repository.Get());
        }

        // POST: Line/Edit
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Resource2 line)
        {
            if (ModelState.IsValid)
            {
                int success = 0;
                if (line.Id != 0)
                {
                    success = await repository.Update(line);
                }
                else
                {
                    success = await repository.Insert(line);
                }
                if (success == 1)
                {
                    return Json(new { result = "success", id = line.Id });
                }
            }
            return Json(new { result = "error" });
        }

        // POST: Line/Editdata
        [HttpPost]
        public async Task<ActionResult> Editdata(string data)
        {
            string[] lines = data.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            int success = 0;
            foreach (string line in lines)
            {
                Resource2 lineEntity = new Resource2();
                if (line == "")
                {
                    continue;
                }
                string[] elements = line.Split('\t');

                if (elements.Count() < 1)
                {
                    continue;
                }

                lineEntity.Name = elements[0];

                success = await repository.Insert(lineEntity);
            }

            if (success > 0)
            {
                return Json(new { result = "success" });
            }

            return Json(new { result = "error" });
        }

        // POST: Line/Delete/5
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
