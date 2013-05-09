using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScrumboardSPA.Controllers
{
    using Data.Story;
    using Data.Story.State;

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

        [HttpGet]
        public UserStory GetStory(int id)
        {
            UserStory story = this.storyRepository.GetAllStories().SingleOrDefault(s => s.Id == id);

            if (story == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return story;
        }

        [HttpPut]
        [ActionName("state")]
        public HttpResponseMessage SetState(int id, StoryState state)
        {
            // Aufruf via /api/story/{id}/state/{state}, konfiguriert via WebApiConfig.
            // Alternative ist /api/story/{id}/state mit {state} im Http body -> benötigt keine eigene Route-config, daüfr [FromBody] Attribut beim Paramter

            UserStory story = this.storyRepository.GetAllStories().SingleOrDefault(s => s.Id == id);

            if (story == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            story.State = state;
            return Request.CreateResponse(HttpStatusCode.OK, story);
        }
    }
}
