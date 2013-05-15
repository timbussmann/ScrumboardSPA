namespace ScrumboardSPA
{
    using Microsoft.AspNet.SignalR;

    public class StoryHub : Hub
    {
        public void UpdateStory(UserStoryWithString story)
        {
            Clients.Others.updateStory(story);
        }

        public class UserStoryWithString
        {
            public int Id { get; set; }
            public string State { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int StoryPoints { get; set; }
            public int StackRank { get; set; }
        }
    }
}