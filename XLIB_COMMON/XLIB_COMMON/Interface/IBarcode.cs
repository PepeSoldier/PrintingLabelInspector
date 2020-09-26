using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLIB_COMMON.Interface
{
    public interface IBarcodeParser
    {
        int StockUnitId { get; set; }
        decimal Qty { get; set; }
        string ItemCode { get; set; }
        string Location { get; set; }
        DateTime DateTime { get; set; }
        string SerialNumber { get; }
        string Barcode { get; }

        void Parse(string barcode, string template);
        int GetQty(string barcode);
        string GetSerialNumber(string barcode);
        string GetName(string barcode);
        string GetCode(string barcode);
    }
}
