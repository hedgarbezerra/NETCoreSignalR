using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NETCoreSignalR.Services.External
{
    //[Authorize]
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message, string room, bool join)
        {
            if (join)
            {
                await AddToGroup(room).ConfigureAwait(false);
                await Clients.Group(room).SendAsync("ReceiveMessage", new
                {
                    Sender = Context.User.Identity.Name,
                    Message = message
                }).ConfigureAwait(true);

            }
            else
            {
                await Clients.Group(room).SendAsync("ReceiveMessage", user, message).ConfigureAwait(true);

            }
        }
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId}/{Context.User.Identity.Name} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
