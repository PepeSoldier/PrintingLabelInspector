using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using _MPPL_WEB_START.Migrations;
using MDLX_BASE.Repo;
using MDL_PFEP.Models.DEF;

namespace _MPPL_WEB_START.Areas.PFEP.Controllers
{
    public class AncTypeController : Controller
    {
        private DbContextAPP_ElectroluxPLV db;
        //private GenericRepository<AncType> repository;

        public AncTypeController()
        {
            db = DbContextAPP_ElectroluxPLV.Create();
            //repository = new GenericRepository<AncType>(db);
        }

        //// GET: AncType
        //public async Task<ActionResult> Index()
        //{
        //    return View(await repository.Get());
        //}

        //// POST: AncType/Edit
        //[HttpPost]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] AncType ancType)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        int success = 0;
        //        if (ancType.Id != 0)
        //        {
        //            success = await repository.Update(ancType);
        //        }
        //        else
        //        {
        //            success = await repository.Insert(ancType);
        //        }
        //        if (success == 1)
        //        {
        //            return Json(new { result = "success", id = ancType.Id });
        //        }
        //    }
        //    return Json(new { result = "error" });
        //}

        // POST: AncType/Editdata
        //[HttpPost]
        //public async Task<ActionResult> Editdata(string data)
        //{
        //    string[] lines = data.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        //    int success = 0;
        //    foreach (string line in lines)
        //    {
        //        AncType ancType = new AncType();
        //        if (line == string.Empty)
        //        {
        //            continue;
        //        }
        //        string[] elements = line.Split('\t');

        //        if (elements.Count() < 1)
        //        {
        //            continue;
        //        }

        //        ancType.Name = elements[0];

        //        success = await repository.Insert(ancType);
        //    }

        //    if (success > 0)
        //    {
        //        return Json(new { result = "success" });
        //    }

        //    return Json(new { result = "error" });
        //}

        //// POST: AncType/Delete/5
        //[HttpPost]
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    await repository.Delete(id);
        //    return Json(new { result = "success" });
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
