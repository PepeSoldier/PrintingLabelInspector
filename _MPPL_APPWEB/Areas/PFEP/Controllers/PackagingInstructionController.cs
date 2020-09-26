using _MPPL_WEB_START.Areas.PFEP.Models;
using _MPPL_WEB_START.Models;
using MDL_BASE.Enums;
using MDL_BASE.Models;
using MDL_BASE.Models.Base;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;
using XLIB_COMMON.Repo.Base;

using XLIB_COMMON.Repo.IDENTITY;
using MDL_PFEP.Interface;
using MDL_PFEP.Model.ELDISY_PFEP;
using MDL_PFEP.Repo.ELDISY_PFEP;
using MDL_PFEP.Repo.DEF;
using MDL_PFEP.ViewModel;
using MDL_PFEP.ComponentPackingIntruction.Models;
using XLIB_COMMON.Interface;
using XLIB_COMMON.Model;
using Microsoft.AspNet.Identity;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using MDLX_MASTERDATA.Repos;
using XLIB_COMMON.Enums;

namespace _MPPL_WEB_START.Areas.PFEP.Controllers
{
    public class PackingInstructionController : Controller
    {
        private readonly IDbContextPFEP_Eldisy db;
        private PackingInstructionPackageRepo packingInstructionPackageRepo;
        private PackingInstructionRepo packingInstructionRepo;
        private PackageRepo packageRepo;
        private ChangeLogRepo changeLogRepo;
        private CorrectionRepo correctionRepo;
        private PackingInstructionPhotoRepo packingInstrPhotoRepo;
        private RepoArea repoArea;
        private UserRepo userRepo;
        private Mailer_PFEP mailer_PFEP;
        
        public PackingInstructionController(IDbContextPFEP_Eldisy db, IUserStore<User, string> userStore)
        {
            this.db = db;
            packingInstructionPackageRepo = new PackingInstructionPackageRepo(db);
            packingInstructionRepo = new PackingInstructionRepo(db);
            packageRepo = new PackageRepo(db);
            packingInstrPhotoRepo = new PackingInstructionPhotoRepo(db);

            repoArea = new RepoArea(db);
            userRepo = new UserRepo(userStore, db);
            correctionRepo = new CorrectionRepo(db);
            changeLogRepo = new ChangeLogRepo(db);
            mailer_PFEP = new Mailer_PFEP(Mailer.Create());
        }

        //--------------------SHOW---------------------------
        [Authorize]
        public ActionResult Show(int id = 0)
        {
            DateTime temp = new DateTime(1, 1, 1);
            User user = userRepo.FindById(User.Identity.GetUserId());
            //TODO: wydaje mi się, że trzeba pomyslec nad krótszymi nazwami zmiennych
            PackingInstructionViewModel vm = new PackingInstructionViewModel();
            vm.PackinInstructionObject = packingInstructionRepo.GetById(id);
            if(vm.PackinInstructionObject == null)
            {
                return RedirectToAction("Browse");
            }
            vm.PackingInstructionPackageList = packingInstructionPackageRepo.GetPackagesForInstruction(id);
            
            vm.PackingInstructionPhotos = packingInstrPhotoRepo.GetByPackingInstruction(vm.PackinInstructionObject);
            vm.ChangeLogs = new ChangeLogRepo(db).GetListByObjectIdAndName(id, "PackingInstruction").ToList();
            vm.PrevIntrId = packingInstructionRepo.GetPrevId(id);
            vm.NextIntrId = packingInstructionRepo.GetNextId(id);
            if (vm.PackinInstructionObject.ConfirmId != null)
            {
                vm.IsUserAllowedToConfirm = user.Id == vm.PackinInstructionObject.ConfirmId ? true : false;
            }
            else
            {
                vm.IsUserAllowedToConfirm = false;
            }
            if (vm.PackinInstructionObject.ExaminerId != null)
            {
                vm.IsUserAllowedToExamine = user.Id == vm.PackinInstructionObject.ExaminerId ? true : false;
            }
            else
            {
                vm.IsUserAllowedToExamine = false;
            }
            
            return View(vm);
        }
        [HttpPost, AuthorizeRoles(DefRoles.PFEP_PACKINGINSTR_EXAMINER)]
        public JsonResult ExamineInstruction(PackingInstructionViewModel vm)
        {
            PackingInstruction pi = packingInstructionRepo.GetById(vm.PackinInstructionObject.Id);

            ObjectsComparer oc = new ObjectsComparer();
            oc.Config.IncludedFields = new string[] { "Examined", "ExamineComment" };
            oc.DetectChanges(pi, vm.PackinInstructionObject, pi.Id);
            if(oc.ObjectDataChanges.Count() == 1)
            {
                oc.ObjectDataChanges.Add(new ObjectDataChange
                {
                    fieldDisplayName = "Sprawdził",
                    fieldName = "Examined",
                    newValue = "False",
                    oldValue = "False",
                    objClassName = "PackingInstruction",
                    objectId = pi.Id
                });
            };
            
            changeLogRepo.AddChangeLogs(oc.ObjectDataChanges, User.Identity.GetUserId());

            pi.ExamineComment = vm.PackinInstructionObject.ExamineComment;
            pi.Examined = vm.PackinInstructionObject.Examined;
            pi.ExaminedDate = DateTime.Now;
            pi.Examiner = userRepo.FindById(User.Identity.GetUserId());

            packingInstructionRepo.AddOrUpdate(pi);
            if(pi.Examined == false)
            {
                ApplicationRole identityRole = userRepo.FindByRoleName(DefRoles.ADMIN);
                mailer_PFEP.RejectedPackingInstruction(vm.PackinInstructionObject,userRepo.GetUsersByRole(identityRole));
                AlertManager.Instance.AddAlert(AlertMessageType.danger, "Odrzuciłeś Instrukcję " + DateTime.Now.ToString(), User.Identity.Name);
            }
            else
            {
                if (pi.Confirm != null)
                {
                    mailer_PFEP.NewPackingInstruction(vm.PackinInstructionObject, pi.Confirm);
                    AlertManager.Instance.AddAlert(AlertMessageType.success, "Zaakceptowałeś Instrukcję " + DateTime.Now.ToString(),User.Identity.Name);
                }
            }
            
            return Json(pi, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, AuthorizeRoles(DefRoles.PFEP_PACKINGINSTR_CONFIRMER)]
        public JsonResult ConfirmInstruction(PackingInstructionViewModel vm)
        {
            var pi = packingInstructionRepo.GetById(vm.PackinInstructionObject.Id);

            ObjectsComparer oc = new ObjectsComparer();
            oc.Config.IncludedFields = new string[] { "Confirmed", "ConfirmComment" };
            oc.DetectChanges(pi, vm.PackinInstructionObject, pi.Id);

            if (oc.ObjectDataChanges.Count() == 1)
            {
                oc.ObjectDataChanges.Add(new ObjectDataChange
                {
                    fieldDisplayName = "Zatwierdził",
                    fieldName = "Confirmed",
                    newValue = "False",
                    oldValue = "False",
                    objClassName = "PackingInstruction",
                    objectId = pi.Id
                });
            };
            changeLogRepo.AddChangeLogs(oc.ObjectDataChanges, User.Identity.GetUserId());
            
            pi.ConfirmComment = vm.PackinInstructionObject.ConfirmComment;
            pi.Confirmed = vm.PackinInstructionObject.Confirmed;
            pi.ConfirmedDate = DateTime.Now;
            pi.Confirm = userRepo.FindById(User.Identity.GetUserId());

            if (pi.Confirmed == false)
            {
                ApplicationRole identityRole = userRepo.FindByRoleName(DefRoles.ADMIN);
                mailer_PFEP.RejectedPackingInstruction(vm.PackinInstructionObject, userRepo.GetUsersByRole(identityRole));
                AlertManager.Instance.AddAlert(AlertMessageType.danger, "Odrzuciłeś Instrukcję " + DateTime.Now.ToString(), User.Identity.Name);
            }
            else
            {
                if (pi.Creator != null)
                {
                    mailer_PFEP.ConfirmedPackingInstruction(vm.PackinInstructionObject, pi.Creator);
                    AlertManager.Instance.AddAlert(AlertMessageType.success, "Zaakceptowałeś Instrukcję " + DateTime.Now.ToString(), User.Identity.Name);
                }
            }
            packingInstructionRepo.AddOrUpdate(pi);

            return Json(pi, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPackagesForInstruction(string InstructionID)
        {
            List<PackingInstructionPackage> Packages = packingInstructionPackageRepo.GetPackagesForInstruction(Int32.Parse(InstructionID));
            PackingInstruction pi = packingInstructionRepo.GetById(Int32.Parse(InstructionID));
            Packages = packingInstructionPackageRepo.CalculateForOneHundred(Packages, pi);
            return Json(Packages, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Print(int id = 0)
        {
            //Widok wydruku intrukcji. Powinno być to samo co "Show" ale z wyłączonym layoutem;
            PackingInstructionViewModel vm = new PackingInstructionViewModel();
            vm.PackinInstructionObject = packingInstructionRepo.GetById(id);
            vm.PackingInstructionPackageList = packingInstructionPackageRepo.GetPackagesForInstruction(id);
            vm.PackingInstructionPhotos = packingInstrPhotoRepo.GetByPackingInstruction(vm.PackinInstructionObject);
           
            return View(vm);
        }
        public ActionResult PrintOnShow(int id = 0)
        {
            //Widok wydruku intrukcji. Powinno być to samo co "Show" ale z wyłączonym layoutem;
            PackingInstructionViewModel vm = new PackingInstructionViewModel();
            vm.PackinInstructionObject = packingInstructionRepo.GetById(id);
            vm.PackingInstructionPackageList = packingInstructionPackageRepo.GetPackagesForInstruction(id);
            vm.PackingInstructionPackageList = packingInstructionPackageRepo.CalculateForOneHundred(vm.PackingInstructionPackageList, vm.PackinInstructionObject);
            vm.PackingInstructionPhotos = packingInstrPhotoRepo.GetByPackingInstruction(vm.PackinInstructionObject);
            vm.StepTwo = true;
            return View("Print", vm);
        }
        public ActionResult SavePDF(int id)
        {
            ActionAsPdf pdfResult = null;
            DateTime dt1 = DateTime.Now;
            string dt = dt1.Year.ToString() + dt1.Month.ToString() + dt1.Day.ToString() + dt1.Hour.ToString() + dt1.Minute.ToString() + dt1.Second.ToString();

            pdfResult = new ActionAsPdf("Print", new { id = id, authorize = 1  })
            {
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = new Rotativa.Options.Margins(3, 3, 3, 3),
                FileName = "InstrukcjaPakowania_nr" + id + "_" + dt + ".pdf"
            };
            return pdfResult;
        }

        //--------------------BROWSE---------------------------
        public ActionResult Browse()
        {
            //Przeglądanie listy instrukcji

            return View();
        }
        [HttpGet]
        public JsonResult GetInstructions(PackingInstruction filter)
        {
            List<PackingInstructionFilterObject> instructionsList = new List<PackingInstructionFilterObject>();
            int startIndex = (filter.pageIndex - 1) * filter.pageSize;
            var instructions = packingInstructionRepo.GetListWithCorrections(filter);
            var inctuctionsCount = instructions.Count();
            if(filter.sortField == null)
            {
                instructionsList = instructions.OrderByDescending(x => x.Id).Skip(startIndex).Take(filter.pageSize).ToList();
            }
            else
            {
                instructionsList = instructions.Skip(startIndex).Take(filter.pageSize).ToList();
            }            
            return Json(new { data = instructionsList, itemsCount = inctuctionsCount }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetInstructionsBySap(string packageSapNumber)
        {
            var instructions = packingInstructionRepo.GetListWithCorrectionsBySap(packageSapNumber);
            var instrList = instructions.ToList();
            var instrCount = instructions.Count();
            return Json(new { data = instrList, itemsCount = instrCount }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, AuthorizeRoles(DefRoles.ADMIN)]
        public JsonResult DeleteInstruction(PackingInstruction item)
        {
            PackingInstruction temp = packingInstructionRepo.GetById(item.Id);
            temp.Deleted = true;
            packingInstructionRepo.AddOrUpdate(temp);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        //---------------------EDIT-----------------------------
        [HttpGet,AuthorizeRoles(DefRoles.ADMIN)]
        public ActionResult Edit(int? Id)
        {
            PackingInstructionViewModel vm = new PackingInstructionViewModel();

            List<User> engineerList = userRepo.GetQualityEngineers();
            vm.EngineersSelectList = engineerList.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FullName }).ToList();

            List<User> managerList = userRepo.GetInstructionConfirmers();
            vm.ManagersSelectList = managerList.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FullName }).ToList();

            List<Area> areaList = repoArea.GetList().ToList();
            vm.AreasSelectList = areaList.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

            if (Id > 0)
            {
                vm.PackinInstructionObject = packingInstructionRepo.GetById((int)Id);
                vm.PackingInstructionPackageList = packingInstructionPackageRepo.GetPackagesForInstruction((int)Id);
                vm.PackinInstructionObject.Area = repoArea.GetById(vm.PackinInstructionObject.AreaId);
                vm.PackingInstructionPhotos = new PackingInstructionPhotoRepo(db).GetByPackingInstruction(vm.PackinInstructionObject);
                vm.Corrections = packingInstructionRepo.GetCorrectionsByInstructionId((int)Id);
                vm.StepTwo = true;
            }
            else
            {
                vm.PackinInstructionObject = new PackingInstruction();
                vm.PackingInstructionPackageList = new List<PackingInstructionPackage>();
                vm.PackingInstructionPhotos = new List<PackingInstructionPhoto>();
                vm.StepTwo = false;
            }
            
            //Tworzenie edycja instrukcji
            return View(vm);
        }
        [HttpPost,AuthorizeRoles(DefRoles.ADMIN)]
        public ActionResult Edit(PackingInstructionViewModel vm)
        {
            List<User> engineerList = userRepo.GetQualityEngineers();
            List<User> managerList = userRepo.GetManagers();
            List<Area> areaList = repoArea.GetList().ToList();
            
            User creator = userRepo.FindById(User.Identity.GetUserId());
            //vm.PackinInstructionObject.Area.Id = vm.PackinInstructionObject.AreaId;
            vm.EngineersSelectList = engineerList.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FullName }).ToList();
            vm.ManagersSelectList = managerList.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.FullName }).ToList();
            vm.AreasSelectList = areaList.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            vm.PackinInstructionObject.AreaId = Convert.ToInt32(vm.AreaName);
            if (vm.StepTwo == false || vm.PackinInstructionObject.Id == 0)
            {
                vm.PackinInstructionObject = packingInstructionRepo.Create(vm.PackinInstructionObject, creator);
                vm.StepTwo = true;
                
                ObjectsComparer oc = new ObjectsComparer();
                oc.Config.IncludedFields = new string[] { "Creator"};
                oc.DetectChanges(new PackingInstruction(), vm.PackinInstructionObject, vm.PackinInstructionObject.Id);
                changeLogRepo.AddChangeLogs(oc.ObjectDataChanges, creator.Id);
            }
            else
            {
                PackingInstruction pi = packingInstructionRepo.GetById(vm.PackinInstructionObject.Id);
                vm.PackinInstructionObject.CreatorId = User.Identity.GetUserId();
                vm.PackinInstructionObject.Creator = userRepo.FindById(vm.PackinInstructionObject.CreatorId);
                vm.PackinInstructionObject.Examiner = userRepo.FindById(vm.PackinInstructionObject.ExaminerId);
                vm.PackinInstructionObject.Confirm = userRepo.FindById(vm.PackinInstructionObject.ConfirmId);

                ObjectsComparer ocTrue = new ObjectsComparer(true);

                if (ocTrue.DetectChanges(pi, vm.PackinInstructionObject, pi.Id) == true)
                {
                    User user = userRepo.FindById(vm.PackinInstructionObject.ExaminerId);
                    packingInstructionRepo.AssignEditted(pi,vm.PackinInstructionObject,creator);
                    mailer_PFEP.NewPackingInstruction(vm.PackinInstructionObject, user);
                };
                
                changeLogRepo.AddChangeLogs(ocTrue.ObjectDataChanges, User.Identity.GetUserId());


                return RedirectToAction("Show", new { id = vm.PackinInstructionObject.Id });
            }
            return View(vm);
        }
       
        
        [HttpPost, AuthorizeRoles(DefRoles.ADMIN)]
        public JsonResult AddPackageToInstruction(PackingInstructionPackage item, string InstructionId)
        {
            PackingInstruction pi = packingInstructionRepo.GetById(Int32.Parse(InstructionId));
            item.Package = packageRepo.GetBySapNumber(item.Package.Code);
            item.PackingInstructionId = pi.Id;
            packingInstructionPackageRepo.Add(item);
            item = packingInstructionPackageRepo.CalculateForOneHundred(item, pi);
            return Json(item,JsonRequestBehavior.AllowGet);
        }
        [HttpPost, AuthorizeRoles(DefRoles.ADMIN)]
        public JsonResult RemovePackageFromInstruction(PackingInstructionPackage item)
        {
            PackingInstructionPackage temp = packingInstructionPackageRepo.GetById(item.Id);
            packingInstructionPackageRepo.Delete(temp);

            return Json(item, JsonRequestBehavior.AllowGet);
        }

        
        //--------------------CORRECTION---------------------------
        public ActionResult AddCorrection(int id = 0)
        {
            PackingInstructionViewModel vm = new PackingInstructionViewModel();
            vm.Correction.PackingInstructionId = id;
            vm.PackinInstructionObject = packingInstructionRepo.GetById(id);
            vm.PackingInstructionPackageList = packingInstructionPackageRepo.GetPackagesForInstruction(id);
            vm.PackingInstructionPhotos = packingInstrPhotoRepo.GetByPackingInstruction(vm.PackinInstructionObject);

            return View(vm);
        }
        [HttpPost]
        public JsonResult AddCorrection(PackingInstructionViewModel cvm)
        {
            PackingInstruction pi = packingInstructionRepo.GetById(cvm.PackinInstructionObject.Id);
            cvm.Correction.Applicant = userRepo.FindById(User.Identity.GetUserId());
            cvm.Correction.PackingInstructionId = cvm.PackinInstructionObject.Id;
            cvm.Correction.ApplicationStart = DateTime.Now;
            cvm.Correction.CorrectionOpen = true;
            packingInstructionRepo.AddCorrection(cvm.Correction);
            ApplicationRole role = userRepo.FindByRoleName("Admin");
            List<User> UserList = userRepo.GetUsersByRole(role);
            mailer_PFEP.NewCorrection(pi, UserList);
            
            AlertManager.Instance.AddAlert(AlertMessageType.info, "Zgłoszona Poprawka", cvm.Correction.Applicant.UserName);
            return Json(cvm,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RemoveCorrection(int? PackingInstructionId)
        {
            List<Correction> correctionList = packingInstructionRepo.GetCorrectionsByInstructionId((int)PackingInstructionId);
            foreach(var item in correctionList)
            {
                item.Deleted = true;
                correctionRepo.AddOrUpdate(item);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        //-----------------------PHOTO-----------------------------
        [HttpPost]
        public JsonResult UploadPhoto(int parentObjectId)
        {
            HttpPostedFileBase file;
            List<PackingInstructionPhoto> attachmentList = new List<PackingInstructionPhoto>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                file = Request.Files[i];

                if (AttachmentHelper.IsExtensionPhotoAllowed(AttachmentHelper.GetPostedFileName(file, Request.Browser.Browser)))
                {
                    //UploadAttachmentPhoto(file, parentObjectId, parentType1);
                    PackingInstructionPhotoRepo repo = new PackingInstructionPhotoRepo(db);
                    PackingInstructionPhoto attachmentPhoto = repo.Add(parentObjectId, file.FileName);

                    ImageManager imageManager = new ImageManager();
                    imageManager.SaveBigPhoto(Image.FromStream(file.InputStream), attachmentPhoto);
                    imageManager.SaveMidPhoto(Image.FromStream(file.InputStream), attachmentPhoto);
                    imageManager.SaveMiniPhoto(Image.FromStream(file.InputStream), attachmentPhoto);
                    attachmentList.Add(attachmentPhoto);
                }
                else
                {
                    attachmentList.Add(new PackingInstructionPhoto
                    {
                        Id = 0,
                        Extension = AttachmentHelper.GetFileExtension(AttachmentHelper.GetPostedFileName(file, Request.Browser.Browser)),
                        UploadErrorMessage = "Niedozwolony format pliku. Załaduj jeden z formatów: " + String.Join(", ", AttachmentHelper.GetAllowedPhotoExtensions().ToArray()) + "."
                    });
                }
            }

            if (!(Request.Files.Count > 0))
                return Json("No files selected.");
            else
                return Json(attachmentList);
        }
        [HttpPost]
        public JsonResult DeletePhoto(int photoId)
        {
            Attachment photo = packingInstrPhotoRepo.GetById(photoId);
            ImageManager imageManager = new ImageManager();
            imageManager.DeleteBigPhoto(photo);
            imageManager.DeleteMidPhoto(photo);
            imageManager.DeleteMiniPhoto(photo);

            packingInstrPhotoRepo.Delete(photo);
            return Json(0);
        }
        //TODO: skasować po wdrożeniu. Funkcja tymczasowa.
        public JsonResult CreatePhotosAttachment()
        {
            List<PackingInstruction> list = packingInstructionRepo.GetList().Where(d=>d.Id >= 1170).OrderBy(x=>x.Id).ToList();
            PackingInstructionPhotoRepo repo = new PackingInstructionPhotoRepo(db);
            PackingInstructionPhoto attachmentPhoto = null;
            ImageManager imageManager = new ImageManager();
            Image img = null;

            List<string> lipa = new List<string>();
            string photoName = string.Empty;
            string appPath = AppDomain.CurrentDomain.BaseDirectory;

            foreach (PackingInstruction pi in list)
            {
                for(int i = 1; i <= 4; i++)
                {
                    if (i == 1) photoName = pi.TempPhoto1;
                    if (i == 2) photoName = pi.TempPhoto2;
                    if (i == 3) photoName = pi.TempPhoto3;
                    if (i == 4) photoName = pi.TempPhoto4;

                    if (photoName != null && photoName.Length > 3)
                    {
                        try
                        {
                            img = Image.FromFile(appPath + @"Uploads\zz_photos\" + photoName, true);

                            if (img != null)
                            {
                                attachmentPhoto = repo.Add(pi.Id, photoName);
                            }
                            else
                            {
                                lipa.Add(pi.Id.ToString() + "##" + photoName);
                            }

                            imageManager.SaveBigPhoto(img, attachmentPhoto);
                            img = Image.FromFile(appPath + @"Uploads\zz_photos\" + photoName, true);
                            imageManager.SaveMidPhoto(img, attachmentPhoto);
                            img = Image.FromFile(appPath + @"Uploads\zz_photos\" + photoName, true);
                            imageManager.SaveMiniPhoto(img, attachmentPhoto);
                        }
                        catch
                        {
                            lipa.Add(pi.Id.ToString() + "##" + photoName);
                        }
                    }
                }
            }
            return Json(lipa, JsonRequestBehavior.AllowGet);
        }

        //--------------------AUTOCOMPLETES AND OTHER---------------
        [HttpGet]
        public JsonResult GetUnitOfMeasures()
        {
            var sList = Enum.GetValues(typeof(UnitOfMeasure)).Cast<UnitOfMeasure>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            return Json(sList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetVersionTypes()
        {
            var versionTypes = Enum.GetValues(typeof(InstructionVersion)).Cast<InstructionVersion>().Select(v => new SelectListItem
            {
                Text = v.ToString().Substring(0, 4) + ".",
                Value = ((int)v).ToString()
            }).ToList();
            versionTypes.Insert(0, new SelectListItem { Text = "", Value = "-1" });
            return Json(versionTypes, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ClientNameAutoComplete(string Prefix)
        {
            List<string> ClientNameAutoComplete = packingInstructionRepo.GetClientNameList(Prefix);
            return Json(ClientNameAutoComplete, JsonRequestBehavior.AllowGet);
        }

    }
}
