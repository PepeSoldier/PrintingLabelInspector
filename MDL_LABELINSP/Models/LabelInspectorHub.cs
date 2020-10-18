using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace MDL_LABELINSP.Models
{
    [Microsoft.AspNet.SignalR.Hubs.HubName("labelInspectorHub")]
    public class LabelInspectorHub : Hub
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