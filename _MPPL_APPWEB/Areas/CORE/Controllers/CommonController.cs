using _MPPL_WEB_START.Areas.CORE.ViewModels;
using System.Web.Mvc;
using XLIB_COMMON.Model;

namespace _MPPL_WEB_START.Areas.CORE.Controllers
{
    public class CommonController : Controller
    {
        public CommonController()
        {
            ViewBag.Skin = "nasaSkin";
        }

        public JsonResult ParseBarcode(string barcode, string template)
        {
            BarcodeParsedViewModel vm = new BarcodeParsedViewModel();

            BarcodeManager barcodeParser = new BarcodeManager();
            barcodeParser.Parse(barcode, template);

            vm.ItemCode = barcodeParser.ItemCode;
            vm.Location = barcodeParser.Location;
            vm.Qty = barcodeParser.Qty;
            vm.SerialNumber = barcodeParser.SerialNumber;
            vm.StockUnitId = barcodeParser.StockUnitId;
            vm.ErrorText = barcodeParser.Error ? "Wykryto błąd." : "";
            vm.ErrorText += barcodeParser.ErrorUnexpectedQtyChar ? "Wykryto nieprawidłowy znak w obszarze ilości." : "";
            vm.ErrorText += barcodeParser.ErrorWrongLength ? "Wykryto nieoczekiwaną długość kodu." : "";

            return Json(vm);
        }
    }
}