namespace ScrumboardSPA.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Web.Http;
    using Data.Story;

    public class ScrumboardController : ApiController
    {
        private IStoryRepository storyRepository;

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
