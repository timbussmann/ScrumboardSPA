namespace ScrumboardSPA.Sockets
{
    using Data.Story;
    using Microsoft.AspNet.SignalR;

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

        public void DeleteStory(int deletedStoryId)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<StoryHub>();
            context.Clients.All.deleteStory(deletedStoryId);
        }
    }
}