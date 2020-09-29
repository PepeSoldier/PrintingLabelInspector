using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace _MPPL_WEB_START.Areas.ONEPROD.Models
{
    [Microsoft.AspNet.SignalR.Hubs.HubName("jobLabelCheckHub")]
    public class JobLabelCheckHub : Hub
    {
        public async Task SendBarcode(string barcode)
        {
            await Clients.Others.broadcastMessage(barcode);
        }
    }
}