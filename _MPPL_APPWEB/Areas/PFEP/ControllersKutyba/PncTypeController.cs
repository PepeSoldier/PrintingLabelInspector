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
    public class PncTypeController : Controller
    {
    //    private DbContextAPP_Electrolux db;
    //    private GenericRepository<PncType> repository;

    //    public PncTypeController()
    //    {
    //        db = new DbContextAPP_Electrolux();
    //        repository = new GenericRepository<PncType>(db);
    //    }

    //    // GET: PncType
    //    public async Task<ActionResult> Index()
    //    {
    //        return View(await repository.Get());
    //    }

    //    // POST: PncType/Edit
    //    [HttpPost]
    //    public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] PncType pncType)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            int success = 0;
    //            if (pncType.Id != 0)
    //            {
    //                success = await repository.Update(pncType);
    //            }
    //            else
    //            {
    //                success = await repository.Insert(pncType);
    //            }
    //            if (success == 1)
    //            {
    //                return Json(new { result = "success", id = pncType.Id });
    //            }
    //        }
    //        return Json(new { result = "error" });
    //    }

    //    // POST: PncType/Editdata
    //    [HttpPost]
    //    public async Task<ActionResult> Editdata(string data)
    //    {
    //        string[] lines = data.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
    //        int success = 0;
    //        foreach (string line in lines)
    //        {
    //            PncType pncType = new PncType();
    //            if (line == "")
    //            {
    //                continue;
    //            }
    //            string[] elements = line.Split('\t');

    //            if (elements.Count() < 1)
    //            {
    //                continue;
    //            }

    //            pncType.Name = elements[0];

    //            success = await repository.Insert(pncType);
    //        }

    //        if (success > 0)
    //        {
    //            return Json(new { result = "success" });
    //        }

    //        return Json(new { result = "error" });
    //    }

    //    // POST: PncType/Delete/5
    //    [HttpPost]
    //    public async Task<ActionResult> Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        await repository.Delete(id);
    //        return Json(new { result = "success" });
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            db.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }
    }
}
