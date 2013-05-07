using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScrumboardSPA.Controllers
{
    using Data.Story;

    public class StoryController : ApiController
    {
        private readonly IStoryRepository storyRepository;

        public StoryController() : this(new StoryRepository())
        {
            //TODO use ninject and remove this constructor!
        }

        public StoryController(IStoryRepository storyRepository)
        {
            this.storyRepository = storyRepository;
        }

        public Story GetStory(int id)
        {
            Story story = this.storyRepository.GetAllStories().SingleOrDefault(s => s.Id == id);

            if (story == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return story;
        }
    }
}
