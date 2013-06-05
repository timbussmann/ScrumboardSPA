namespace ScrumboardSPA
{
    using Microsoft.AspNet.SignalR;

    public class StoryHub : Hub
    {
        private static int clientCounter = 0;

        public override System.Threading.Tasks.Task OnConnected()
        {
            clientCounter++;
            this.Clients.All.updateOnlineUsers(clientCounter);
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected()
        {
            clientCounter--;
            this.Clients.All.updateOnlineUsers(clientCounter);
            return base.OnDisconnected();
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            return base.OnReconnected();
        }
    }
}