namespace ScrumboardSPA.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;
    using Data.Story;

    public class ScrumboardController : ApiController
    {
        private readonly IStoryRepository storyRepository;

        public ScrumboardController(IStoryRepository storyRepository)
        {
            this.storyRepository = storyRepository;
        }

        public IEnumerable<Story> GetStories()
        {
            return this.storyRepository.GetAllStories();
        }

        public Story GetStory(int storyId)
        {
            return this.storyRepository.GetAllStories().Single(s => s.Id == storyId);
        }
    }
}
