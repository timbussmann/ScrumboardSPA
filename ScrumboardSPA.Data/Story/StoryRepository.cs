namespace ScrumboardSPA.Data.Story
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;
    using State;

    public class StoryRepository : IStoryRepository
    {
        private readonly object lockObject = new object();
        private readonly List<UserStory> userStories = new List<UserStory>();

        public IEnumerable<UserStory> GetAllStories()
        {
            return this.userStories.Select(s => (UserStory)s.Clone());
        }

        public UserStory AddNewStory(NewUserStory newStory)
        {
            int id = this.GetNewId();
            var story = new UserStory(id)
                                  {
                                      Title = newStory.Title,
                                      Description = newStory.Description,
                                      State = newStory.State,
                                      StackRank = newStory.StackRank,
                                      StoryPoints = newStory.StoryPoints
                                  };
            
            this.userStories.Add(story);
            return (UserStory)story.Clone();
        }

        public UserStory UpdateStory(UserStory story)
        {
            var originalStory = this.userStories.Find(s => s.Id == story.Id);

            if (originalStory.Etag != story.Etag)
            {
                throw new RepositoryConcurrencyException(originalStory, story);
            }

            int index = this.userStories.IndexOf(originalStory);
            UserStory newStory = new UserStory(story);
            this.userStories[index] = newStory;

            return (UserStory)newStory.Clone();
        }

        public UserStory GetStory(int id)
        {
            var story = this.userStories.SingleOrDefault(s => s.Id == id);

            if (story != null)
            {
                return (UserStory)story.Clone();
            }

            return null;
        }

        public bool DeleteStory(int id)
        {
            var story = this.userStories.SingleOrDefault(s => s.Id == id);
            return this.userStories.Remove(story);
        }

        private int GetNewId()
        {
            lock (this.lockObject)
            {
                return userStories.Any() ? userStories.Max(s => s.Id) + 1 : 1;
            }
        }

        public void Initialize()
        {
            this.AddNewStory(new NewUserStory()
            {
                Title = "Drag and drop",
                Description =
                                         "As a user I want to be able to drag and drop user stories on the board.",
                StackRank = 990,
                State = StoryState.Done,
                StoryPoints = 5
            });
            this.AddNewStory(new NewUserStory()
            {
                Title = "SignalR support",
                Description =
                                         "As a user I want to be notified when another team member changes the state of a story on the board.",
                StackRank = 950,
                State = StoryState.WorkInProgress,
                StoryPoints = 8
            });
            this.AddNewStory(new NewUserStory()
            {
                Title = "Show Scrum board in offline mode",
                Description =
                                         "As a user I want to see the scrumboard even when I am offline",
                StackRank = 940,
                State = StoryState.SprintBacklog,
                StoryPoints = 3
            });
            this.AddNewStory(new NewUserStory()
            {
                Title = "Move Stories in offline mode",
                Description =
                                         "As a user I want to move stories when I am offline and get them synced when connection is reestablished.",
                StackRank = 920,
                State = StoryState.SprintBacklog,
                StoryPoints = 8
            });
        }
    }
}