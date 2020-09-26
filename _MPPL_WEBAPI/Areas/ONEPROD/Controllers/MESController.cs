using _MPPL_WEBAPI.Models;
using MDL_CORE.ComponentCore.Models;
using MDL_ONEPROD.ComponentMes.Models;
using MDL_ONEPROD.ComponetMes.Models;
using MDL_ONEPROD.Model.Scheduling;
using MDLX_CORE.Model.PrintModels;
using System.Linq;
using System.Web.Mvc;
using XLIB_COMMON.Enums;

namespace _MPPL_WEBAPI.Areas.ONEPROD.Controllers
{
    public class MESController : Controller
    {
        private DbContext_Elux dbContext;

        public MESController()
        {
            dbContext = new DbContext_Elux();
        }

        [HttpPost]
        public JsonResult GenerateSerialNumber(int resourceId)
        {
            string query = "";//SerialNumberManager.GenerateSerialNumberForONEPROD(resourceId);
            string serialNumber = dbContext.db.SqlQuery2<string>(query).FirstOrDefault();
            return Json(serialNumber);
        }

        [HttpPost]
        public JsonResult PrintoutLabel(int workplaceId, int workorderId, int qty, int serialNo)
        {
            Workplace wrkp; //= uowMes.WorkplaceRepo.GetById(workplaceId);
            wrkp = new Workplace()
            {
                PrinterType = PrinterType.CAB,
                PrintLabel = true,
                LabelName = "01",
                PrinterIPv4 = "10.26.11.206",
                LabelLayoutNo = 1
            };

            //Workorder wo = uowMes.WorkorderRepo.GetById(workorderId);
            string uniqueNumber = "1605554440"; // wo.UniqueNumber
            string itemCode = "111222333"; //wo.Item.Code

            bool isPrinted = false;

            if (wrkp.PrintLabel)
            {
                isPrinted = new PrintLabelManager().PrepareAndPrintLabel(wrkp, uniqueNumber, itemCode, serialNo, qty);
            }
            else
            {
                isPrinted = true;
            }

            return Json(isPrinted);
        }

        [HttpPost]
        public JsonResult PrintLabel(int workplaceId, int workorderId, int qty, int serialNumber)
        {
            Workplace wrkp = null; //uowMes.WorkplaceRepo.GetById(workplaceId);
            Workorder wo = null; //uowMes.WorkorderRepo.GetById(workorderId);

            bool printed = new PrintLabelManager().PrepareAndPrintLabel(wrkp, wo.UniqueNumber, "999999999", serialNumber, qty);

            return Json(printed);
        }
    }
}