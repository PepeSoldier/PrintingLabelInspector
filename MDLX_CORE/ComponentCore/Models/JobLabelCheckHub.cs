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

        public async Task JoinWorkstation(string workstationName)
        {
            await Groups.Add(Context.ConnectionId, workstationName);
            var c = Clients.Group(workstationName);
            c.broadcastMessage("99999999999999999999");
        }

        public Task LeaveWorkstation(string workstationName)
        {
            return Groups.Remove(Context.ConnectionId, workstationName);
        }
    }
}