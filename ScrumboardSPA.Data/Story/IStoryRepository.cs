namespace ScrumboardSPA.Data.Story
{
    using System.Collections.Generic;

    public interface IStoryRepository
    {
        IEnumerable<Story> GetAllStories();
    }
}