namespace ScrumboardSPA.Sockets
{
    using Microsoft.AspNet.SignalR;

    using ScrumboardSPA.Data.Story;

    class StoryHubService : IStoryHubService
    {
        public void UpdateStory(UserStory updatedStory)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<StoryHub>();
            context.Clients.All.updateStory(updatedStory);
        }
    }
}