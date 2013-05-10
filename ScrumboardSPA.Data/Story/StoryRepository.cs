namespace ScrumboardSPA.Data.Story
{
    using System.Collections.Generic;
    using System.Linq;
    using ScrumboardSPA.Data.Story.State;

    public class StoryRepository : IStoryRepository
    {
        private readonly UserStory[] userStories = new[]
            {
                new UserStory
                    {
                        Id = 1, 
                        Title = "REST API: Stories anzeigen", 
                        Description = "Als Entwickler möchte ich die Stories des aktuellen Sprints sehen.", 
                        StoryPoints = 2, 
                        StackRank = 999, 
                        State = StoryState.Done
                    }, 
                new UserStory
                    {
                        Id = 2, 
                        Title = "REST API: Story laden", 
                        Description = "Als Entwickler möchte ich eine einzelne Story laden.", 
                        StoryPoints = 1, 
                        StackRank = 990, 
                        State = StoryState.Done
                    }, 
                new UserStory
                    {
                        Id = 4, 
                        Title = "Story Detail View", 
                        Description = "Als Benutzer möchte ich detailliertere Informationen zu einer Story erhalten.", 
                        StoryPoints = 2, 
                        StackRank = 980, 
                        State = StoryState.Done
                    }, 
                new UserStory
                    {
                        Id = 5, 
                        Title = "Story mit drag & drop verschieben", 
                        Description =
                            "Als Benutzer möchte ich eine Story auf dem Scrumboard auf einen anderen Status verschieben können.", 
                        StackRank = 950, 
                        State = StoryState.WorkInProgress, 
                        StoryPoints = 5
                    }, 
                new UserStory
                    {
                        Id = 6, 
                        Title = "Ladescreen anzeigen", 
                        Description =
                            "Als Benuter möchte ich einen Ladebalken sehen solange die Appliaktion noch nicht bereit ist.", 
                        StackRank = 600, 
                        State = StoryState.SprintBacklog
                    }, 
                new UserStory
                    {
                        Id = 3, 
                        Title = "REST API: Progress links auf Story", 
                        Description =
                            "Als API Benutzer möchte ich die möglichen Statusänderungen auf einer Story sehen", 
                        StoryPoints = 3, 
                        StackRank = 500, 
                        State = StoryState.SprintBacklog
                    }, 
                new UserStory
                    {
                        Id = 7, 
                        Title = "Storypoints auf Storykarte anzeigen", 
                        Description = "Als Benutzer möchte ich die Storypoints einer Story am Scrumboard sehen.", 
                        StackRank = 499, 
                        State = StoryState.SprintBacklog, 
                        StoryPoints = 1
                    }
            };

        public IEnumerable<UserStory> GetAllStories()
        {
            return this.userStories.OrderByDescending(s => s.StackRank);
        }
    }
}