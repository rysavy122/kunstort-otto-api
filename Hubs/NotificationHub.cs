using App.Models;
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
    public async Task SendForschungsfrageUpdate(Forschungsfrage forschungsfrage)
    {
      await Clients.All.SendAsync("ReceiveForschungsfrageUpdate", forschungsfrage);
    }
    public async Task SendCommentUpdate(string action, Kommentar comment)
    {
      await Clients.All.SendAsync("ReceiveCommentUpdate", action, comment);
    }
    public async Task SendCommentPositionUpdate(CommentPosition position)
    {
      await Clients.All.SendAsync("ReceiveCommentPositionUpdate", position);
    }
    public async Task SendMediaUpdate(object mediaData)
    {
      await Clients.All.SendAsync("ReceiveMediaUpdate", mediaData);
    }
    public async Task SendMediaDeleteUpdate(string fileName)
    {
      await Clients.All.SendAsync("ReceiveMediaDeleteUpdate", fileName);
    }

    public async Task SendCommentDeleteUpdate(int commentId)
    {
      await Clients.All.SendAsync("ReceiveCommentDeleteUpdate", commentId);
    }

    public async Task SendMediaPositionUpdate(MediaPosition position)
    {
      await Clients.All.SendAsync("ReceiveMediaPositionUpdate", position);
    }

  }
}
