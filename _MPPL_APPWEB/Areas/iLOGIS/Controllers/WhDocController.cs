using _MPPL_WEB_START.Areas._APPWEB.ViewModels;
using _MPPL_WEB_START.Models;
using MDL_BASE.Models.IDENTITY;
using MDL_BASE.Models.MasterData;

using MDL_CORE.ComponentCore.Models;
using MDL_iLOGIS.ComponentConfig._Interfaces;
using MDL_iLOGIS.ComponentCore.Models;
using MDL_iLOGIS.ComponentCore.Enums;
using MDL_iLOGIS.ComponentWHDOC.Entities;
using MDL_iLOGIS.ComponentWHDOC.Enums;
using MDL_iLOGIS.ComponentWHDOC.Models;
using MDL_iLOGIS.ComponentWHDOC.ViewModels;
using MDL_iLOGIS.ComponentWMS.Mappers;
using MDL_WMS.ComponentConfig.UnitOfWorks;
using MDLX_CORE.ComponentCore.UnitOfWorks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PdfSharp;
using PdfSharp.Pdf;
using PDFtoPrinter;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using XLIB_COMMON.Model;
using XLIB_COMMON.Repo.IDENTITY;
using _MPPL_WEB_START.Areas._APPWEB.Models;
using MDL_CORE.ComponentCore.Entities;

namespace _MPPL_WEB_START.Areas.iLOGIS.Controllers
{
    [AuthorizeRoles(DefRoles.ILOGIS_WHDOC_VIEWER)]
    public class WhDocController : Controller
    {
        private UnitOfWork_iLogis uow;
        private UnitOfWorkCore uowCore;
        private UserRepo UserManager;
        private readonly IDbContextiLOGIS db;

        public WhDocController(IDbContextiLOGIS db, IUserStore<User, string> userStore)
        {
            this.db = db;
            uow = new UnitOfWork_iLogis(db);
            uowCore = new UnitOfWorkCore(db);
            UserManager = new UserRepo(userStore, db);
            ViewBag.Skin = "nasaSkin";
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Index(int id = 0)
        {
            WhDocumentAbstractViewModel whDocumentAbstractViewModel = new WhDocumentAbstractViewModel();
            ApplicationRole identityRole = UserManager.FindByRoleName(DefRoles.ILOGIS_WHDOC_APPROVER);
            List<User> personList = UserManager.GetUsersByRole(identityRole);

            if (id != 0)
            {
                whDocumentAbstractViewModel = uow.WhDocumentWZRepo.GetById(id).FirstOrDefault<WhDocumentAbstractViewModel>();
                whDocumentAbstractViewModel.WhDocumentItems = uow.WhDocumentItemRepo.GetByDocumentId(id).ToList<WhDocumentItemViewModel>();
            }
            else
            {
                WhDocumentAbstract whDocument = new WhDocumentWZ();
                whDocument.WhDocumentItems = new List<WhDocumentItem>();
                whDocument.Status = EnumWhDocumentStatus.init;
                whDocument.CreatorId = User.Identity.GetUserId();
                whDocument.DocumentDate = DateTime.Now;
                whDocument.StampTime = DateTime.Now;
                uow.WhDocumentWZRepo.Add(whDocument);
                //whDocument.DocumentNumber = whDocument.Id.ToString();
                whDocument.DocumentNumber = whDocument.Id.ToString() + "/" + DateTime.Now.Year.ToString();
                uow.WhDocumentWZRepo.AddOrUpdate(whDocument);
                //whDocumentAbstractViewModel.Id = whDocument.Id;
                //whDocumentAbstractViewModel.CreatorName = UserManager.FindById(whDocument.CreatorId).FullName;
                //whDocumentAbstractViewModel.DocumentDate = whDocument.DocumentDate;
                //whDocumentAbstractViewModel.IssueDate = whDocument.IssueDate??DateTime.Now;
                //whDocumentAbstractViewModel.StampTime = whDocument.StampTime;
                return RedirectToAction("Index", new { id = whDocument.Id });
            }
            whDocumentAbstractViewModel.QrCode = QrCodeGenerator.GenerateSVG(PrepareQRCodeContent(whDocumentAbstractViewModel));
            whDocumentAbstractViewModel.ApproverList = new SelectList(personList, "Id", "FullName");
            SetUserPermissionToEdit(whDocumentAbstractViewModel);

            return View(whDocumentAbstractViewModel);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult IndexSignMobile(int id = 0)
        {
            return View(id);
        }

        public void SetUserPermissionToEdit(WhDocumentAbstractViewModel vm)
        {
            string currentUserId = User.Identity.GetUserId();
            if (UserManager.IsInRole(currentUserId, DefRoles.ILOGIS_WHDOC_EDITOR))
            {
                vm.EnableEditing = true;
            }
            else
            {
                vm.EnableEditing = false;
            }
        }

        [HttpGet, AuthorizeRoles(DefRoles.ILOGIS_WHDOC_APPROVER)]
        public ActionResult IndexMobile(int id = 0)
        {
            WhDocumentAbstractViewModel whDocumentAbstractViewModel = uow.WhDocumentWZRepo.GetById(id).FirstOrDefault<WhDocumentAbstractViewModel>();
            whDocumentAbstractViewModel.WhDocumentItems = uow.WhDocumentItemRepo.GetByDocumentId(id).ToList<WhDocumentItemViewModel>();

            return View(whDocumentAbstractViewModel);
        }

        [HttpGet, AuthorizeRoles(DefRoles.ILOGIS_WHDOC_SECURITY)]
        public ActionResult IndexMobileSecurity()
        {
            return View();
        }

        [HttpPost, AuthorizeRoles(DefRoles.ILOGIS_WHDOC_SECURITY)]
        //Ta funkcja powinna się nazywać SeccurityApproveGetDocument. "Barcode" nie mmówi nic.
        public JsonResult IndexMobileSecurityGetData(string barcode = "")
        {
            //QRCODE: "WHDOC-WZ.[" + whDocumentAbstractViewModel.Id.ToString() + "].Number.[" + whDocumentAbstractViewModel.DocumentNumber + "].C.[1]"
            JsonModel jsonModel = new JsonModel();
            int documentId = 0;

            if (barcode != "" && barcode.StartsWith("WHDOC-WZ."))
            {
                try
                {
                    string documentIdStr = barcode.Split('.')[1].Replace('[', ' ').Replace(']', ' ').Trim();
                    documentId = Int32.Parse(documentIdStr);
                    WhDocumentAbstractViewModel whDocumentAbstractViewModel = uow.WhDocumentWZRepo.GetById(documentId).FirstOrDefault<WhDocumentAbstractViewModel>();
                    jsonModel.Data = whDocumentAbstractViewModel;
                    whDocumentAbstractViewModel.WhDocumentItems = uow.WhDocumentItemRepo.GetByDocumentId(documentId).ToList<WhDocumentItemViewModel>();
                    jsonModel.MessageType = JsonMessageType.success;
                    jsonModel.Message = "Wczytano dokument";
                }
                catch (Exception ex)
                {
                    jsonModel.MessageType = JsonMessageType.danger;
                    jsonModel.Message = "Parsowanie nie powiodło się";
                }
            }
            else
            {
                jsonModel.MessageType = JsonMessageType.danger;
                jsonModel.Message = "Nieznany ciąg znaków";
            }

            return Json(jsonModel);
        }

        [HttpPost, AuthorizeRoles(DefRoles.ILOGIS_WHDOC_SECURITY)]
        public JsonResult IndexMobileSecurityConfirmDocument(int documentId)
        {
            WhDocumentWZ whDocumentWZ = uow.WhDocumentWZRepo.GetById(documentId);
            string userId = User.Identity.GetUserId();
            whDocumentWZ.SecurityApproverId = userId;
            uow.WhDocumentWZRepo.AddOrUpdate(whDocumentWZ);
            JsonModel jsonModel = new JsonModel();
            jsonModel.MessageType = JsonMessageType.success;
            jsonModel.Message = "Zatwierdzono";
            return Json(jsonModel);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Print(int id = 0)
        {
            if (id == 0)
            {
                return RedirectToAction("Browse");
            }
            WhDocumentAbstractViewModel whDocumentAbstractViewModel = uow.WhDocumentWZRepo.GetById(id).FirstOrDefault<WhDocumentAbstractViewModel>();
            whDocumentAbstractViewModel.WhDocumentItems = uow.WhDocumentItemRepo.GetByDocumentId(id).ToList<WhDocumentItemViewModel>();
            whDocumentAbstractViewModel.QrCode = QrCodeGenerator.GenerateSVG(PrepareQRCodeContent(whDocumentAbstractViewModel));
            return View(whDocumentAbstractViewModel);
        }

        private static string PrepareQRCodeContent(WhDocumentAbstractViewModel whDocumentAbstractViewModel)
        {
            return "WHDOC-WZ.[" + whDocumentAbstractViewModel.Id.ToString() + "].Number.[" + whDocumentAbstractViewModel.DocumentNumber + "].C.[1]";
        }

        [HttpGet, AllowAnonymous]
        public ActionResult PrintHTML(int id)
        {
            //var htmlDict = this.ControllerContext.Controller.TempData.FirstOrDefault(x => x.Key == "PrintHTML");
            //var html = htmlDict.Value;
            object html = System.IO.File.ReadAllText(Path.Combine(Server.MapPath("~/Uploads/WhDocuments"), "WZ_" + id + "_text.txt"));
            object cc = this.ControllerContext;
            return View("PrintHTML", html);
        }

        [HttpGet]
        public ActionResult Browse()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BrowseMobile()
        {
            return View();
        }

        [HttpPost, AuthorizeRoles(DefRoles.ILOGIS_WHDOC_APPROVER)]
        public JsonResult Approve(int id, bool isApproved = false)
        {
            WhDocumentWZ whDocumentWZ = uow.WhDocumentWZRepo.GetById(id);
            string userId = User.Identity.GetUserId();
            JsonModel jsonModel = new JsonModel();
            iLogisMailer mailerIlogis = new iLogisMailer(Mailer.Create());

            if (whDocumentWZ != null && whDocumentWZ.Status < EnumWhDocumentStatus.signed)
            {
                if (userId == whDocumentWZ.ApproverId)
                {
                    whDocumentWZ.Status = isApproved ? EnumWhDocumentStatus.approved : EnumWhDocumentStatus.rejected;
                    whDocumentWZ.ApproveDate = DateTime.Now;
                    uow.WhDocumentWZRepo.AddOrUpdate(whDocumentWZ);
                    mailerIlogis.WzDoc(whDocumentWZ, whDocumentWZ.Creator.Email);
                    if (isApproved == true)
                    {
                        jsonModel.Message = "Dokument zaakceptowany";
                        jsonModel.MessageType = JsonMessageType.success;
                        jsonModel.Data = "Zaakceptowano";
                    }
                    else
                    {
                        jsonModel.Message = "Dokument odrzucony";
                        jsonModel.MessageType = JsonMessageType.warning;
                        jsonModel.Data = "Odrzucono";
                    }
                }
                else
                {
                    jsonModel.Message = "Nie masz uprawnień do akceptacji";
                    jsonModel.MessageType = JsonMessageType.danger;
                }
            }
            else
            {
                jsonModel.Message = "Nie odnaleziono dokumentu";
                jsonModel.MessageType = JsonMessageType.danger;
            }
            return Json(jsonModel);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult SignAndPrint(int id, string html)
        {
            JsonModel jsonModel = new JsonModel();

            WhDocumentWZ whDocumentWZ = uow.WhDocumentWZRepo.GetById(id);
            whDocumentWZ.Status = EnumWhDocumentStatus.signed;
            //whDocumentWZ.IssueDate = DateTime.Now;
            //whDocumentWZ.IssuerId = User.Identity.GetUserId();
            uow.WhDocumentWZRepo.AddOrUpdate(whDocumentWZ);


            string fileName = "WZ_" + id + ".pdf";
            //ViewBag.PrintHTML = html;
            //this.ControllerContext.Controller.TempData.Add("PrintHTML", html);
            var post = new Dictionary<string, string>();
            post.Add("PrintHTML", html);

            System.IO.File.WriteAllText(Path.Combine(Server.MapPath("~/Uploads/WhDocuments"), "WZ_" + id + "_text.txt"), html);

            ActionAsPdf pdfResult = null;
            pdfResult = new ActionAsPdf("PrintHTML", new { id })
            {
                PageSize = Rotativa.Options.Size.A5,
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0),
                FileName = fileName,
                CustomSwitches = "--debug-javascript --no-stop-slow-scripts --javascript-delay 2000 ",
            };

            byte[] fileBytes = pdfResult.BuildFile(this.ControllerContext);
            string filePath = Path.Combine(Server.MapPath("~/Uploads/WhDocuments"), fileName);

            System.IO.File.WriteAllBytes(filePath, fileBytes);

            //string htmlPage = RenderViewToString(this.ControllerContext, "PrintHTML", html, false);
            //var cfg = new PdfGenerateConfig { PageSize = PageSize.A5, PageOrientation = PageOrientation.Landscape };
            //PdfDocument pdf = PdfGenerator.GeneratePdf(htmlPage, cfg);
            //pdf.Save(Path.Combine(Server.MapPath("~/Uploads/WhDocuments"), "WZ_" + id + ".pdf"));

            iLogisStatus status = iLogisStatus.WHDocPDFCreatedSuccessfull;
            string statusInfo = string.Empty;

            try
            {
                Printer printer = uow.PrinterRepo.GetByName("WZPrinter");
                //var pdfToPrinter = new PDFtoPrinterPrinter();
                //PLWP1023, 10.26.33.172
                //pdfToPrinter.Print(new PrintingOptions(printer.IpAdress, filePath)).Wait();
                //new Printer().PrintRawStream(printerName, new MemoryStream(binary), "Print.pdf", false);
                new XLIB_RawPrint.Printer().PrintRawFile(printer.IpAdress, filePath, false);
                statusInfo = printer.IpAdress;
            }
            catch (Exception ex)
            {
                status = iLogisStatus.PrintingProblem;
                statusInfo = filePath + ". " + ex.Message + ". " + (ex.InnerException != null ? ex.InnerException.Message : "");
                //throw ex;
            }

            return Json(new JsonModel() { Status = (int)status, Message = statusInfo, MessageType = JsonMessageType.info });
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult PrintCopy(int id)
        {
            string fileName = "WZ_" + id + ".pdf";
            string filePath = Path.Combine(Server.MapPath("~/Uploads/WhDocuments"), fileName);

            iLogisStatus status = iLogisStatus.WHDocPDFCreatedSuccessfull;
            string statusInfo = string.Empty;

            try
            {
                Printer printer = uow.PrinterRepo.GetByName("WZPrinter");
                //var pdfToPrinter = new PDFtoPrinterPrinter();
                //PLWP1023, 10.26.33.172
                //pdfToPrinter.Print(new PrintingOptions(printer.IpAdress, filePath)).Wait();
                //new XLIB_RawPrint.Printer().PrintRawStream(printerName, new MemoryStream(binary), "Print.pdf", false);
                new XLIB_RawPrint.Printer().PrintRawFile(printer.IpAdress, filePath, false);
                statusInfo = printer.IpAdress;
            }
            catch (Exception ex)
            {
                status = iLogisStatus.PrintingProblem;
                statusInfo = filePath + ". " + ex.Message + ". " + (ex.InnerException != null ? ex.InnerException.Message : "");
                //throw ex;
            }

            return Json(new { status, statusInfo });
        }

        [HttpPost]
        public JsonResult Issue(int id)
        {
            JsonModel jsonModel = new JsonModel();

            WhDocumentWZ whDoc = uow.WhDocumentWZRepo.GetById(id);

            if (whDoc.Status >= EnumWhDocumentStatus.approved)
            {
                whDoc.IssueDate = DateTime.Now;
                whDoc.IssuerId = whDoc.CreatorId; //User.Identity.GetUserId();
                uow.WhDocumentWZRepo.AddOrUpdate(whDoc);


                WhDocumentAbstractViewModel whDocumentAbstractViewModel = new WhDocumentAbstractViewModel();
                whDocumentAbstractViewModel = whDoc.FirstOrDefault<WhDocumentAbstractViewModel>();
                whDocumentAbstractViewModel.WhDocumentItems = uow.WhDocumentItemRepo.GetByDocumentId(id).ToList<WhDocumentItemViewModel>();

                jsonModel.Data = whDocumentAbstractViewModel;
            }
            else
            {
                jsonModel.Message = "Dokument nie jest jeszcze zatwierdzony";
                jsonModel.MessageType = JsonMessageType.danger;
                jsonModel.Status = (int)iLogisStatus.WHDocNotSignedYet;
            }
            return Json(jsonModel);
        }
        
        [HttpPost, AuthorizeRoles(DefRoles.ILOGIS_WHDOC_EDITOR)]
        public JsonResult Complete(int documentId)
        {
            WhDocumentWZ whDocumentWZ = uow.WhDocumentWZRepo.GetById(documentId);
            if (whDocumentWZ != null && whDocumentWZ.Status < EnumWhDocumentStatus.completed)
            {
                //whDocumentWZ.ApproverId = User.Identity.GetUserId();
                whDocumentWZ.Status = EnumWhDocumentStatus.completed;
                uow.WhDocumentWZRepo.AddOrUpdate(whDocumentWZ);
            }
            return Json(0);
        }

        [HttpPost]
        public JsonResult WhDocumentGetList(WhDocumentAbstractViewModel filter, int pageIndex, int pageSize)
        {
            //IQueryable<int> deliveryListId = uow.WhDocumentItemRepo.GetUniqueWhDocumentId(filter.ItemCode);
            IQueryable<WhDocumentWZ> query = uow.WhDocumentWZRepo.GetList(filter) //(filter)
                .Include(c => c.WhDocumentItems);
            //.Where(x => x.WhDocumentItems.Where(y => y.ItemCode == filter.ItemCode).Any());

            int startIndex = (pageIndex - 1) * pageSize;
            int itemsCount = query.Count();

            List<WhDocumentAbstractViewModel> deliveries = query
                .Skip(startIndex).Take(pageSize)
                .ToList<WhDocumentAbstractViewModel>();

            return Json(new { data = deliveries, itemsCount });
        }

        [HttpPost]
        public JsonResult Update(WhDocumentAbstractViewModel item)
        {
            JsonModel jsonModel = new JsonModel();
            Contractor contr = uow.RepoContractor.GetbyName(item.ContractorName);

            if(contr == null)
            {
                uow.RepoContractor.AddOrUpdate(new Contractor()
                {
                    Code = "",
                    ContactAdress = item.ContractorAdress,
                    Name = item.ContractorName,
                    Deleted = false
                });
                contr = uow.RepoContractor.GetbyName(item.ContractorName);
            }
            else
            {
                contr.ContactAdress = item.ContractorAdress;
                uow.RepoContractor.AddOrUpdate(contr);
            }

            if (ValidateForm(item, contr, jsonModel))
            {
                WhDocumentWZ whDocumentWZ = uow.WhDocumentWZRepo.GetById(item.Id);
                whDocumentWZ.StampTime = DateTime.Now;
                whDocumentWZ.ApproverId = item.ApproverId;
                whDocumentWZ.Contractor = contr;
                whDocumentWZ.CostCenter = item.CostCenter;
                whDocumentWZ.CostPayer = item.CostPayer;
                whDocumentWZ.CreatorId = User.Identity.GetUserId();
                whDocumentWZ.DocumentDate = item.DocumentDate;
                whDocumentWZ.StampTime = DateTime.Now;
                whDocumentWZ.Notice = item.Notice;
                whDocumentWZ.TrailerPlateNumbers = item.TrailerPlateNumbers;
                whDocumentWZ.TruckPlateNumbers = item.TruckPlateNumbers;
                whDocumentWZ.Reason = item.Reason;
                whDocumentWZ.ApproveDate = new DateTime(2000,1,1);
                whDocumentWZ.SecurityApproverId = null;
                //whDocumentWZ.DocumentNumber = item.DocumentNumber;
                //whDocumentWZ.DocumentNumber = whDocumentWZ.Id.ToString() + "/" + DateTime.Now.Year.ToString();
                whDocumentWZ.ReferrenceDocument = item.ReferrenceDocument;
                whDocumentWZ.Reason = item.Reason;
                whDocumentWZ.Status = EnumWhDocumentStatus.ready;
                uow.WhDocumentWZRepo.AddOrUpdate(whDocumentWZ);
                List<WhDocumentItem> itemList = item.WhDocumentItems.FindAll(x => x.ItemCode != null && x.WhDocumentId == whDocumentWZ.Id).Select(y => new WhDocumentItem()
                {
                    Id = 0,//y.Id == null ? 0 : (int)y.Id,
                    WhDocumentId = whDocumentWZ.Id,
                    ItemCode = y.ItemCode,
                    ItemName = y.ItemName,
                    DisposedQty = y.DisposedQty,
                    IssuedQty = y.IssuedQty,
                    UnitOfMeasure = y.UnitOfMeasure,
                    UnitPrice = y.UnitPrice,
                }).ToList();
                uow.WhDocumentItemRepo.DeleteByDocumentId(whDocumentWZ.Id);
                uow.WhDocumentItemRepo.AddOrUpdateRange(itemList);

                NotificationManager.SendSMS(whDocumentWZ.Approver.PhoneNumber, "http://plws1062:82/iLogis/WhDoc/IndexMobile/" + whDocumentWZ.Id);
                //NotificationManager.SendNotification(uowCore, whDocumentWZ.ApproverId, "Nowa WZ", "Podpisz lub odrzuć", "/iLogis/WhDoc/IndexMobile/" + whDocumentWZ.Id);
                jsonModel.Message = "Dokument został zapisany";
                jsonModel.MessageType = JsonMessageType.success;
                jsonModel.Data = "Ok";
            }
            else
            {
                return Json(jsonModel);
            }

            return Json(jsonModel);
        }

        [HttpPost]
        public JsonResult SetStatusToEdit(int id = 0)
        {
            JsonModel jsonModel = new JsonModel();
            WhDocumentWZ whDocumentWZ = uow.WhDocumentWZRepo.GetById(id);

            if (whDocumentWZ.Status < EnumWhDocumentStatus.signed)
            {
                whDocumentWZ.ApproverId = null;
                whDocumentWZ.IssuerId = null;
                whDocumentWZ.Status = EnumWhDocumentStatus.ready;
                whDocumentWZ.CreatorId = User.Identity.GetUserId();
                uow.WhDocumentWZRepo.AddOrUpdate(whDocumentWZ);
            }
            else
            {
                jsonModel.MessageType = JsonMessageType.danger;
                jsonModel.Message = "Nie można edytować podpisanego dokumentu";
                jsonModel.Status = (int)iLogisStatus.WHDocCantEditAlreadySigned;
            }
            return Json(jsonModel);
        }

        [HttpPost]
        public JsonResult Delete(int id = 0)
        {
            string userId = User.Identity.GetUserId();
            bool hasAccess = UserManager.IsInRole(userId, DefRoles.ILOGIS_WHDOC_EDITOR);
            JsonModel jsonModel = new JsonModel();
            if (hasAccess)
            {
                WhDocumentWZ whDocumentWZ = uow.WhDocumentWZRepo.GetById(id);

                if(whDocumentWZ.Status < EnumWhDocumentStatus.completed)
                {
                    whDocumentWZ.Deleted = true;
                    whDocumentWZ.Status = EnumWhDocumentStatus.deleted;
                    uow.WhDocumentWZRepo.AddOrUpdate(whDocumentWZ);
                    jsonModel.Data = "OK";
                    jsonModel.Message = "Dokument został usunięty";
                    jsonModel.MessageType = JsonMessageType.success;
                }
                else
                {
                    jsonModel.Message = "Nie można usunąć dokumentu, który wyjechał z zakładu";
                    jsonModel.MessageType = JsonMessageType.danger;
                }
            }
            else
            {
                jsonModel.Message = "Nie masz uprawnień do tej czynności";
                jsonModel.MessageType = JsonMessageType.danger;
            }
                        
            return Json(jsonModel);
        }

        public bool ValidateForm(WhDocumentAbstractViewModel item, Contractor contr, JsonModel jsonModel)
        {
            bool retVal = true;
            if (contr == null)
            {
                jsonModel.Message = "Brak dostawcy w bazie";
                jsonModel.MessageType = JsonMessageType.danger;
                retVal = false;
            }

            if (item.ApproverId == "")
            {
                jsonModel.Message = "Nie wybrano zatwierdzającego";
                jsonModel.MessageType = JsonMessageType.danger;
                retVal = false;
            }

            if (!item.WhDocumentItems.Any(x => x.ItemCode != ""))
            {
                jsonModel.Message = "Nie znaleziono żadnych produktów do wybrania";
                jsonModel.MessageType = JsonMessageType.danger;
                retVal = false;
            }
            return retVal;
        }

        [HttpPost]
        public JsonResult ContractorWithDeliveryItemsAutocomplete(string prefix)
        {
            List<Contractor> contractorList = uow.RepoContractor.GetContractorAutocompleteList(prefix);
            List<ContractorWithDocumentItems> autoCompleteList = new List<ContractorWithDocumentItems>();
            foreach (var contractor in contractorList)
            {
                ContractorWithDocumentItems contractorWithDocumentItems = new ContractorWithDocumentItems();
                contractorWithDocumentItems.Contractor = contractor;
                contractorWithDocumentItems.WhDocumentItemLightList = uow.WhDocumentItemRepo.GetLastDocumentItemsForContractor(contractor.Id);
                autoCompleteList.Add(contractorWithDocumentItems);
            }
            return Json(autoCompleteList);
        }

        [HttpPost]
        public JsonResult ItemWZAutocomplete(string prefix)
        {
            //uow = new UnitOfWorkMasterData(db);
            List<AutocompleteViewModel> acData = uow.ItemRepo.GetList()
                .Where(x => x.Code.StartsWith(prefix))
                .OrderBy(x => x.Code)
                .Take(10)
                .Select(x => new AutocompleteViewModel { TextField = x.Code, ValueField = x.Id.ToString(), Data1 = x.Name, Data6 = (int)x.UnitOfMeasure })
                .ToList();

            acData.AddRange(uow.PackageRepo.GetList()
                .Where(x => x.Code.StartsWith(prefix))
                .OrderBy(x => x.Code)
                .Take(10)
                .Select(x => new AutocompleteViewModel { TextField = x.Code, ValueField = x.Id.ToString(), Data1 = x.Name, Data6 = (int)x.UnitOfMeasure })
                .ToList()
            );

            if (acData.Count <= 0)
            {
                acData = uow.ItemRepo.GetList()
                .Where(x => x.Name.Contains(prefix))
                .OrderBy(x => x.Code)
                .Take(10)
                .Select(x => new AutocompleteViewModel { TextField = x.Code, ValueField = x.Id.ToString(), Data1 = x.Name, Data6 = (int)x.UnitOfMeasure })
                .ToList();

                acData.AddRange(uow.PackageRepo.GetList()
                .Where(x => x.Name.Contains(prefix))
                .OrderBy(x => x.Code)
                .Take(10)
                .Select(x => new AutocompleteViewModel { TextField = x.Code, ValueField = x.Id.ToString(), Data1 = x.Name, Data6 = (int)x.UnitOfMeasure })
                .ToList()
                );
            }

            return Json(acData);
        }
    }
}