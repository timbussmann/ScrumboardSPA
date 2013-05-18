namespace ScrumboardSPA.Data.Story
{
    using System.Collections.Generic;

    public interface IStoryRepository
    {
        IEnumerable<UserStory> GetAllStories();

        UserStory AddNewStory(NewUserStory newStory);

        UserStory UpdateStory(UserStory story);

        UserStory GetStory(int id);
    }
}