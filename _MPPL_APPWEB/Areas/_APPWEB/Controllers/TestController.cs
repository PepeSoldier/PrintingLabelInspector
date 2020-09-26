using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _MPPL_WEB_START.Areas._APPWEB.Controllers
{
    public class TestController : BaseController
    {
        public ActionResult AppTest()
        {
            ViewBag.Message = "Tests";
            return View();
        }
        public JsonResult AlertTest()
        {
            int type = new Random().Next(1, 6);
            XLIB_COMMON.Model.AlertManager.Instance.AddAlert((XLIB_COMMON.Interface.AlertMessageType)type, DateTime.Now.ToString(), "K");
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GridTest()
        {
            return View();
        }

      
        public JsonResult GritTestJsonData()
        {
            TestJsonResult json0 = new TestJsonResult { Id = 1, Name = "Włochy", Age = 12, DataUrodzin = new DateTime(2010, 10, 05) };
            TestJsonResult json1 = new TestJsonResult { Id = 2, Name = "Czechy", Age = 14, DataUrodzin = new DateTime(2009, 10, 06) };
            TestJsonResult json2 = new TestJsonResult { Id = 3, Name = "Włochy", Age = 15, DataUrodzin = new DateTime(2005, 10, 06) };
            TestJsonResult json3 = new TestJsonResult { Id = 4, Name = "Jamaica", Age = 18, DataUrodzin = new DateTime(2000, 01, 01) };
            TestJsonResult json4 = new TestJsonResult { Id = 1, Name = "Włochy", Age = 12, DataUrodzin = new DateTime(2010, 10, 05) };
            TestJsonResult json5 = new TestJsonResult { Id = 1, Name = "Włochy", Age = 12, DataUrodzin = new DateTime(2010, 10, 05) };
            TestJsonResult json6 = new TestJsonResult { Id = 1, Name = "Włochy", Age = 12, DataUrodzin = new DateTime(2010, 10, 05) };
            TestJsonResult json7 = new TestJsonResult { Id = 1, Name = "Włochy", Age = 12, DataUrodzin = new DateTime(2010, 10, 05) };
            TestJsonResult json8 = new TestJsonResult { Id = 1, Name = "Włochy", Age = 12, DataUrodzin = new DateTime(2010, 10, 05) };
            TestJsonResult json9 = new TestJsonResult { Id = 1, Name = "Włochy", Age = 12, DataUrodzin = new DateTime(2010, 10, 05) };


            return Json(new List<TestJsonResult> { json0, json1, json2, json3, json4, json5, json6, json7, json8, json9 }, JsonRequestBehavior.AllowGet);
        }

        public class TestJsonResult
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public DateTime DataUrodzin { get; set; }
        }

        [HttpGet]
        public JsonResult DeleteItem(TestJsonResult temp)
        {

            return Json(temp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewTest()
        {
            return View();
        }

    }


}