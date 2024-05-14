using Microsoft.AspNetCore.SignalR;

namespace API.Services
{
    public class CartHub : Hub
    {
        public async void refresh()
        {
            await Clients.All.SendAsync("refresh");
        }
    }
}