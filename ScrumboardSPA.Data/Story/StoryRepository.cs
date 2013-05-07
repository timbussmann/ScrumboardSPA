using System;
using System.Collections.Generic;

namespace ScrumboardSPA.Data.Story
{
    public class StoryRepository : IStoryRepository
    {
        public IEnumerable<Story> GetAllStories()
        {
            return new Story[]
                       {
                           new WorkInProgressStory(1)
                               {
                                   Title = "REST API: Stories anzeigen",
                                   Description = "Als Entwickler möchte ich die Stories des aktuellen Sprints sehen.",
                                   StoryPoints = 2,
                                   StackRank = 999
                               },
                           new SprintBacklogStory(2)
                               {
                                   Title = "REST API: Story laden",
                                   Description = "Als Entwickler möchte ich eine einzelne Story laden.",
                                   StoryPoints = 1,
                                   StackRank = 990
                               },
                           new SprintBacklogStory(3)
                               {
                                   Title = "REST API: Progress links auf Story",
                                   Description =
                                       "Als API Benutzer möchte ich die möglichen Statusänderungen auf einer Story sehen",
                                   StoryPoints = 3,
                                   StackRank = 900
                               },
                       };
        }
    }
}
