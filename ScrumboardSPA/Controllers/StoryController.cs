namespace ScrumboardSPA.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Data.Model;
    using Data.Story;
    using Data.Story.State;

    using Microsoft.AspNet.SignalR;

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
        public HttpResponseMessage SetState(int id, StoryState state, [FromBody]Guid etag)
        {
            if (etag == Guid.Empty)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "provide a valid etag (GUID) for the story");
            }

            UserStory story = this.storyRepository.GetStory(id);
            if (story == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            try
            {
                story.State = state;
                story.Etag = etag;
                UserStory updatedStory = this.storyRepository.UpdateStory(story);
                return this.Request.CreateResponse(HttpStatusCode.OK, updatedStory);
            }
            catch (RepositoryConcurrencyException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict,
                                              new ConcurrencyErrorModel(){Original = ex.Original, Requested = ex.Requested});
            }
        }

        [HttpPost]
        public HttpResponseMessage CreateStory(CreateUserStoryModel newStory)
        {
            if (newStory == null || string.IsNullOrWhiteSpace(newStory.Title))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Story title is required");
            }

            var story = new NewUserStory
                                  {
                                      Title = newStory.Title,
                                      Description = newStory.Description,
                                      StackRank = newStory.StackRank,
                                      StoryPoints = newStory.StoryPoints,
                                      State = newStory.State ?? StoryState.SprintBacklog
                                  };

            UserStory createdStory = this.storyRepository.AddNewStory(story);
            return Request.CreateResponse(HttpStatusCode.Created, createdStory);
        }
    }

    public class CreateUserStoryModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int StackRank { get; set; }
        public int StoryPoints { get; set; }
        public StoryState? State { get; set; }
    }

    public class ConcurrencyErrorModel
    {
        public UserStory Original { get; set; }
        public UserStory Requested { get; set; }
    }
}
