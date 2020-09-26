using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XLIB_COMMON.Enums;

namespace MDL_BASE.ViewModel
{
    public class PrinterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string IpAdress { get; set; }

        public string Model { get; set; }
        public string SerialNumber { get; set; }

        public PrinterType PrinterType { get; set; }

    }
}