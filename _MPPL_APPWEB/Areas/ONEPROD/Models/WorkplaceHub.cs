using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace _MPPL_WEB_START.Areas.ONEPROD.Models
{
    [Microsoft.AspNet.SignalR.Hubs.HubName("workplaceHub")]
    public class WorkplaceHub : Hub
    {
        public async Task SendBarcode(string barcode)
        {
            await Clients.Others.broadcastMessage(barcode);
        }

        public async Task JoinWorkplace(string workplaceName)
        {
            await Groups.Add(Context.ConnectionId, workplaceName);
            var c = Clients.Group(workplaceName);
                c.broadcastMessage("99999999999999999999");
        }

        public Task LeaveWorkplace(string workplaceName)
        {
            return Groups.Remove(Context.ConnectionId, workplaceName);
        }
    }
}