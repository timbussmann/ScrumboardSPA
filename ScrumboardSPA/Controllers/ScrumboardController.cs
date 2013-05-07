namespace ScrumboardSPA.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Data.Story;

    public class ScrumboardController : ApiController
    {
        private readonly IStoryRepository storyRepository;

        public ScrumboardController() : this(new StoryRepository())
        {
            //TODO use ninject and remove this constructor!
        }

        public ScrumboardController(IStoryRepository storyRepository)
        {
            this.storyRepository = storyRepository;
        }

        public IEnumerable<Story> GetStories()
        {
            return this.storyRepository.GetAllStories();
        }
    }
}
