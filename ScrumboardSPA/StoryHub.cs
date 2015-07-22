namespace ScrumboardSPA
{
    using System.Threading.Tasks;
    using Microsoft.AspNet.SignalR;

    public class StoryHub : Hub
    {
        private static int clientCounter = 0;

        public override Task OnConnected()
        {
            clientCounter++;
            this.Clients.All.updateOnlineUsers(clientCounter);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            clientCounter--;
            this.Clients.All.updateOnlineUsers(clientCounter);
            return base.OnDisconnected(stopCalled);
        }
    }
}