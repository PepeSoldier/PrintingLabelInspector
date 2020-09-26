using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_CORE.ViewModels
{
    public class NotificationDeviceViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string RegisterDate { get; set; }
        public string UserId { get; set; }
        public string PushEndpoint { get; set; }
    }
}