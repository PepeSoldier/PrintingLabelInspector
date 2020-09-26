using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.Common
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public string Receiver { get; set; }
        public int Type { get; set; } //1-block //2-log
        public DateTime DateTime { get; set; }
    }
}