using MDL_BASE.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MDL_CORE.ComponentCore.Entities;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using MDLX_CORE.ComponentCore.Entities;
using WebPush;
using System.Web.Script.Serialization;
using System;
using MDL_BASE.Models.IDENTITY;
using XLIB_COMMON.Repo.IDENTITY;
using MDL_CORE.ViewModels;
using MDLX_CORE.Mappers;
using MDL_CORE.ComponentCore.Models;
using _LABELINSP_APPWEB.Areas._APPWEB.ViewModels;
using System.Threading.Tasks;



namespace _LABELINSP_APPWEB.Areas.CORE.Controllers
{
    [Authorize]
    public class NotificationDeviceController : Controller
    {
        private readonly IDbContextCore db;
        private readonly UnitOfWorkCore uow;
        private readonly UserRepo UserManager;

        public NotificationDeviceController()
        {
                
        }
        public NotificationDeviceController(IUserStore<User, string> userStore, IDbContextCore db)
        {
            this.db = db;
            ViewBag.Skin = "nasaSkin";
            uow = new UnitOfWorkCore(db);
            UserManager = new UserRepo(userStore, db);
        }

        // GET: NotificationDevices
        //NotificationDevice
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PublicKey = NotificationManager.PublicKey;
            return View();
        }

        [HttpPost]
        public JsonResult NotificationDeviceDelete(int id = 0)
        {
            NotificationDevice device = uow.NotificationDeviceRepo.GetById(id);
            device.Deleted = true;
            uow.NotificationDeviceRepo.AddOrUpdate(device);
            JsonModel jsonModel = new JsonModel();
            jsonModel.MessageType = JsonMessageType.success;
            jsonModel.Message = "Usunięto urządzenie";
            jsonModel.Data = device;
            return Json(jsonModel);
        }

        [HttpPost]
        public JsonResult NotificationDeviceCreateNew(NotificationDevice device)
        {
            JsonModel jsonModel = new JsonModel();
            device.UserId = User.Identity.GetUserId();
            if(device.UserId != null)
            {
                device.RegistrationDate = DateTime.Now;
                uow.NotificationDeviceRepo.AddOrUpdate(device);
                jsonModel.MessageType = JsonMessageType.success;
                jsonModel.Message = "Twoje urządzenie zostało dodane";
                jsonModel.Data = device;
            }
            else
            {
                jsonModel.MessageType = JsonMessageType.danger;
                jsonModel.Message = "Błąd. Nie jesteś zalogowany";
            }
            return Json(jsonModel);
        }

        [HttpPost]
        public JsonResult TestNotification(string userId, string pushEndpoint)
        {
            JsonModel jsonModel = new JsonModel();
            string CurrentUser = User.Identity.GetUserId();
            if(userId == CurrentUser)
            {
                NotificationDevice device = null; // uow.NotificationDeviceRepo.GetByUserIdAndPushEndpoint(userId, pushEndpoint);

                var pushSubscription = new PushSubscription(device.PushEndpoint, device.PushP256DH, device.PushAuth);
                var vapidDetails = new VapidDetails(NotificationManager.VapidSubject, NotificationManager.PublicKey, NotificationManager.PrivateKey);

                var payload = new
                {
                    notification = new
                    {
                        title = "Test",
                        body = "",
                        badge = "iLOGIS",
                    }
                };
                var json = new JavaScriptSerializer().Serialize(payload);

                var webPushClient = new WebPushClient();
                try
                {
                    webPushClient.SendNotification(pushSubscription, json, vapidDetails);
                }
                catch (System.Exception e)
                {
                    jsonModel.MessageType = JsonMessageType.danger;
                    jsonModel.Message = "Problem z wysłaniem powiadomienia";
                    jsonModel.Data = e.Message;
                }
            }
            else
            {
                jsonModel.MessageType = JsonMessageType.danger;
                jsonModel.Message = "Urządzenie nie należy do Ciebie";
                jsonModel.Data = "Urządzenie nie należy do Usera";
            }
            
            return Json(jsonModel);
        }
       

        [HttpPost]
        public JsonResult NotificationDeviceGetList(NotificationDeviceViewModel filter, int pageIndex, int pageSize)
        {
            IQueryable<NotificationDevice> query = uow.NotificationDeviceRepo.GetList();
            List<NotificationDeviceViewModel> notifList = new List<NotificationDeviceViewModel>();
            try
            {
               notifList = query.ToList<NotificationDeviceViewModel>();
            }
            catch
            {

            }
            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = notifList.Count();
            return Json(new { data = notifList, itemsCount });
        }
    }
}