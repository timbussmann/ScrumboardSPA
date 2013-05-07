namespace ScrumboardSPA.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Data.Story;

    public class StoriesController : ApiController
    {
        private readonly IStoryRepository storyRepository;

        public StoriesController() : this(new StoryRepository())
        {
            //TODO use ninject and remove this constructor!
        }

        public StoriesController(IStoryRepository storyRepository)
        {
            this.storyRepository = storyRepository;
        }

        public IEnumerable<Story> GetStories()
        {
            return this.storyRepository.GetAllStories();
        }
    }
}
