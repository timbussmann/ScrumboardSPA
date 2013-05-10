namespace ScrumboardSPA.Data.Story
{
    using System.Collections.Generic;
    using ScrumboardSPA.Data.Story.State;

    public class StoryRepository : IStoryRepository
    {
        public IEnumerable<UserStory> GetAllStories()
        {
            return new[]
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
                            Description = "Als Benutzer möchte ich detailliertere Informationen zu einer Story erhalten", 
                            StoryPoints = 2, 
                            StackRank = 980, 
                            State = StoryState.WorkInProgress
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
                        }
                };
        }
    }
}