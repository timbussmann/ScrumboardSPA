namespace ScrumboardSPA.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Data.Story;
    using Data.Story.State;

    public class StoryController : ApiController
    {
        private readonly IStoryRepository storyRepository;

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

        [HttpPost]
        public HttpResponseMessage CreateStory(NewUserStory newStory)
        {
            var story = new UserStory
                                  {
                                      Title = newStory.Title,
                                      Description = newStory.Description,
                                      StackRank = newStory.StackRank ?? 0,
                                      StoryPoints = newStory.StoryPoints,
                                      State = newStory.State ?? StoryState.SprintBacklog
                                  };

            this.storyRepository.AddNewStory(story);
            return Request.CreateResponse(HttpStatusCode.Created, story);
        }
    }

    public class NewUserStory
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? StackRank { get; set; }
        public int StoryPoints { get; set; }
        public StoryState? State { get; set; }
    }
}
