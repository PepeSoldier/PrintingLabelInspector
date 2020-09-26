using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace _MPPL_WEB_START.Areas.ONEPROD.Models
{
    [Microsoft.AspNet.SignalR.Hubs.HubName("jobListMultiPageHub")]
    public class JobListMultiPageHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.Others.broadcastMessage(message);
        }

        public async Task JoinWorkstation(string workstationName)
        {
            await Groups.Add(Context.ConnectionId, workstationName);
            var c = Clients.Group(workstationName);
                c.broadcastMessage("1");
        }

        public Task LeaveWorkstation(string workstationName)
        {
            return Groups.Remove(Context.ConnectionId, workstationName);
        }
    }
}