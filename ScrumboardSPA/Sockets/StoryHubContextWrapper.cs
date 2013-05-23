namespace ScrumboardSPA.Sockets
{
    using Microsoft.AspNet.SignalR;

    using ScrumboardSPA.Data.Story;

    class StoryHubContextWrapper : IStoryHubContextWrapper
    {
        public void UpdateStory(UserStory updatedStory)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<StoryHub>();
            context.Clients.All.updateStory(updatedStory);
        }

        public void CreateStory(UserStory createdStory)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<StoryHub>();
            context.Clients.All.createStory(createdStory);
        }
    }
}