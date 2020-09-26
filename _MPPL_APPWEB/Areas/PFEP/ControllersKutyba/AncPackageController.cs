using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using _MPPL_WEB_START.Migrations;
using MDL_PFEP.Repo;
//using MDL_PFEP.Models.PFEP;
using MDL_PFEP.Models.DEF;
using MDLX_BASE.Repo;
using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA.Entities;
using MDL_iLOGIS.ComponentConfig.Entities;

namespace _MPPL_WEB_START.Areas.PFEP.Controllers
{
    public class AncPackageController : Controller
    {
        private DbContextAPP_ElectroluxPLV db;
        private GenericRepository<PackageItem> repository;
        private GenericRepository<Item> repositoryAnc;
        private GenericRepository<Package> repositoryPackage;

        public AncPackageController()
        {
            db = DbContextAPP_ElectroluxPLV.Create();
            repository = new GenericRepository<PackageItem>(db);
            repositoryAnc = new GenericRepository<Item>(db);
            repositoryPackage = new GenericRepository<Package>(db);
        }

        // GET: AncPackage
        public async Task<ActionResult> Index()
        {
            var resultsAnc = await repositoryAnc.Get();
            var resultsSetAnc = resultsAnc as IEnumerable<MDLX_MASTERDATA.Entities.Item>;
            Dictionary<string, string> listAnc = resultsSetAnc.ToDictionary(x => x.Id.ToString(), x => x.Code);
            ViewBag.ListAnc = JsonConvert.SerializeObject(listAnc);

            var resultsPackage = await repositoryPackage.Get();
            var resultsSetPackage = resultsPackage as IEnumerable<Package>;
            Dictionary<string, string> listPackage = resultsSetPackage.ToDictionary(x => x.Id.ToString(), x => x.Name);
            ViewBag.ListPackage = JsonConvert.SerializeObject(listPackage);

            return View(await repository.Get());
        }

        // POST: AncPackage/Edit
        [HttpPost]
        public async Task<ActionResult> Edit(
            [Bind(Include = "Id,AncId,PackageId,Returnable,Quantity,Width,Depth,Height,Weight,NetWeight,NumberOfBoxesOnPallet,Stackable")]
            PackageItem ancPackage)
        {
            if (ModelState.IsValid)
            {
                int success = 0;
                if (ancPackage.Id != 0)
                {
                    success = await repository.Update(ancPackage);
                }
                else
                {
                    success = await repository.Insert(ancPackage);
                }
                if (success == 1)
                {
                    return Json(new { result = "success", id = ancPackage.Id });
                }
            }
            return Json(new { result = "error" });
        }

        // POST: AncPackage/Editdata
        [HttpPost]
        public async Task<ActionResult> Editdata(string data)
        {
            string[] lines = data.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            int success = 0;
            foreach (string line in lines)
            {
                PackageItem ancPackage = new PackageItem();
                if (line == string.Empty)
                {
                    continue;
                }
                string[] elements = line.Split('\t');

                if (elements.Count() < 11)
                {
                    continue;
                }

                string tmp1 = elements[0];
                IEnumerable<Item> findAnc = repositoryAnc.GetSynchronous(o => o.Code == tmp1);
                if (findAnc.Count() == 0)
                {
                    continue;
                }
                ancPackage.ItemWMS = null;//findAnc.First();

                string tmp2 = elements[1];
                IEnumerable<Package> findPackage = repositoryPackage.GetSynchronous(o => o.Name == tmp2);
                if (findPackage.Count() == 0)
                {
                    continue;
                }
                ancPackage.Package = findPackage.First();

                bool returnable;
                elements[2] = elements[2].ToLower();
                returnable = (elements[2] == "tak" || elements[2] == "1" || elements[2] == "true");
                //ancPackage.Returnable = returnable;

                CultureInfo culture = new CultureInfo("pl");
                ancPackage.QtyPerPackage = 1; //Decimal.Parse(elements[3].Replace(".", ","), culture.NumberFormat);

                //ancPackage.Width = Int32.Parse(elements[4]);
                //ancPackage.Depth = Int32.Parse(elements[5]);
                //ancPackage.Height = Int32.Parse(elements[6]);
                //ancPackage.Weight = Decimal.Parse(elements[7].Replace(".", ","), culture.NumberFormat);
                //ancPackage.NetWeight = Decimal.Parse(elements[8].Replace(".", ","), culture.NumberFormat);
                ancPackage.PackagesPerPallet = Int32.Parse(elements[9]);

                bool stackable;
                elements[10] = elements[10].ToLower();
                stackable = (elements[10] == "tak" || elements[10] == "1" || elements[10] == "true");
                //ancPackage.Stackable = stackable;

                success = await repository.Insert(ancPackage);
            }

            if (success > 0)
            {
                return Json(new { result = "success" });
            }

            return Json(new { result = "error" });
        }

        // POST: AncPackage/Delete/5
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
