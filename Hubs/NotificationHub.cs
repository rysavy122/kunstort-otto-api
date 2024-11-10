using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace App.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendBackgroundColorUpdate(string color)
        {
            await Clients.All.SendAsync("ReceiveBackgroundColorUpdate", color);
        }
    }
}