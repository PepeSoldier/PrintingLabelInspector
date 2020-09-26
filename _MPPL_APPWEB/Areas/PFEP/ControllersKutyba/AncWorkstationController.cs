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
using Newtonsoft.Json;
using System.Globalization;

using MDLX_BASE.Repo;
using MDL_BASE.Models.MasterData;
using MDLX_MASTERDATA.Entities;
using MDL_iLOGIS.ComponentConfig.Entities;

namespace _MPPL_WEB_START.Areas.PFEP.Controllers
{
    public class AncWorkstationController : Controller
    {
        private DbContextAPP_ElectroluxPLV db;
        private GenericRepository<WorkstationItem> repository;
        private GenericRepository<Item> repositoryAnc;
        private GenericRepository<Workstation> repositoryWorkstation;
        //private GenericRepository<MontageType> repositoryMontageType;
        //private GenericRepository<FeederType> repositoryFeederType;
        //private GenericRepository<BufferType> repositoryBufferType;

        public AncWorkstationController()
        {
            db = DbContextAPP_ElectroluxPLV.Create();
            repository = new GenericRepository<WorkstationItem>(db);
            repositoryAnc = new GenericRepository<Item>(db);
            repositoryWorkstation = new GenericRepository<Workstation>(db);
            //repositoryMontageType = new GenericRepository<MontageType>(db);
            //repositoryFeederType = new GenericRepository<FeederType>(db);
            //repositoryBufferType = new GenericRepository<BufferType>(db);
        }

        // GET: AncWorkstation
        public async Task<ActionResult> Index()
        {
            var resultsAnc = await repositoryAnc.Get();
            var resultsSetAnc = resultsAnc as IEnumerable<Item>;
            Dictionary<string, string> listAnc = resultsSetAnc.ToDictionary(x => x.Id.ToString(), x => x.Code);
            ViewBag.ListAnc = JsonConvert.SerializeObject(listAnc);

            var resultsWorkstation = await repositoryWorkstation.Get();
            var resultsSetWorkstation = resultsWorkstation as IEnumerable<Workstation>;
            Dictionary<string, string> listWorkstation = resultsSetWorkstation.ToDictionary(x => x.Id.ToString(), x => x.Name);
            ViewBag.ListWorkstation = JsonConvert.SerializeObject(listWorkstation);

            //var resultsMontageType = await repositoryMontageType.Get();
            //var resultsSetMontageType = resultsMontageType as IEnumerable<MontageType>;
            //Dictionary<string, string> listMontageType = resultsSetMontageType.ToDictionary(x => x.Id.ToString(), x => x.Name);
            //ViewBag.ListMontageType = JsonConvert.SerializeObject(listMontageType);

            //var resultsFeederType = await repositoryFeederType.Get();
            //var resultsSetFeederType = resultsFeederType as IEnumerable<FeederType>;
            //Dictionary<string, string> listFeederType = resultsSetFeederType.ToDictionary(x => x.Id.ToString(), x => x.Name);
            //ViewBag.ListFeederType = JsonConvert.SerializeObject(listFeederType);

            //var resultsBufferType = await repositoryBufferType.Get();
            //var resultsSetBufferType = resultsBufferType as IEnumerable<BufferType>;
            //Dictionary<string, string> listBufferType = resultsSetBufferType.ToDictionary(x => x.Id.ToString(), x => x.Name);
            //ViewBag.ListBufferType = JsonConvert.SerializeObject(listBufferType);

            return View(await repository.Get());
        }

        // POST: AncWorkstation/Edit
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,AncId,WorkstationId,BomQuantity,Capacity,MontageTypeId,FeederTypeId,BufferTypeId")] WorkstationItem ancWorkstation)
        {
            if (ModelState.IsValid)
            {
                int success = 0;
                if (ancWorkstation.Id != 0)
                {
                    success = await repository.Update(ancWorkstation);
                }
                else
                {
                    success = await repository.Insert(ancWorkstation);
                }
                if (success == 1)
                {
                    return Json(new { result = "success", id = ancWorkstation.Id });
                }
            }
            return Json(new { result = "error" });
        }

        // POST: AncWorkstation/Editdata
        [HttpPost]
        public async Task<ActionResult> Editdata(string data)
        {
            string[] lines = data.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            int success = 0;
            foreach (string line in lines)
            {
                WorkstationItem ancWorkstation = new WorkstationItem();
                if (line == "")
                {
                    continue;
                }
                string[] elements = line.Split('\t');

                if (elements.Count() < 7)
                {
                    continue;
                }

                string tmp1 = elements[0];
                IEnumerable<Item> findAnc = repositoryAnc.GetSynchronous(o => o.Code == tmp1);
                if (findAnc.Count() == 0)
                {
                    continue;
                }
                ancWorkstation.ItemWMS = null; //findAnc.First();

                string tmp2 = elements[1];
                IEnumerable<Workstation> findWorkstation = repositoryWorkstation.GetSynchronous(o => o.Name == tmp2);
                if (findWorkstation.Count() == 0)
                {
                    continue;
                }
                ancWorkstation.Workstation = findWorkstation.First();

                ancWorkstation.MaxBomQty = Int32.Parse(elements[2]);
                ancWorkstation.MaxPackages = Int32.Parse(elements[3]);

                //string tmp3 = elements[4];
                //IEnumerable<MontageType> findMontageType = repositoryMontageType.GetSynchronous(o => o.Name == tmp3);
                //if (findMontageType.Count() == 0)
                //{
                //    continue;
                //}
                ////ancWorkstation.MontageType = findMontageType.First();

                //string tmp4 = elements[5];
                //IEnumerable<FeederType> findFeederType = repositoryFeederType.GetSynchronous(o => o.Name == tmp4);
                //if (findFeederType.Count() == 0)
                //{
                //    continue;
                //}
                ////ancWorkstation.FeederType = findFeederType.First();

                //string tmp5 = elements[6];
                //IEnumerable<BufferType> findBufferType = repositoryBufferType.GetSynchronous(o => o.Name == tmp5);
                //if (findBufferType.Count() == 0)
                //{
                //    continue;
                //}
                ////ancWorkstation.BufferType = findBufferType.First();

                success = await repository.Insert(ancWorkstation);
            }

            if (success > 0)
            {
                return Json(new { result = "success" });
            }

            return Json(new { result = "error" });
        }

        // POST: AncWorkstation/Delete/5
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
