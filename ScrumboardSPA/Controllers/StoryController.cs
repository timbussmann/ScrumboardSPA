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

    using ScrumboardSPA.Sockets;

    public class StoryController : ApiController
    {
        private readonly IStoryRepository storyRepository;

        private readonly IStoryHubContextWrapper storyHubService;

        public StoryController(IStoryRepository storyRepository, IStoryHubContextWrapper storyHubService)
        {
            this.storyRepository = storyRepository;
            this.storyHubService = storyHubService;
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
        public HttpResponseMessage SetState(int id, SetStoryStateCommand updateCommand)
        {
            if (updateCommand == null || updateCommand.Etag == Guid.Empty)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            UserStory story = this.storyRepository.GetStory(id);
            if (story == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            try
            {
                story.State = updateCommand.State;
                story.Etag = updateCommand.Etag;
                UserStory updatedStory = this.storyRepository.UpdateStory(story);
                this.storyHubService.UpdateStory(updatedStory);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (RepositoryConcurrencyException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict,
                                              new ConcurrencyErrorModel() { Original = ex.Original, Requested = ex.Requested});
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
            this.storyHubService.CreateStory(createdStory);
            return Request.CreateResponse(HttpStatusCode.Created);
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

    public class SetStoryStateCommand
    {
        public StoryState State { get; set; }
        public Guid Etag { get; set; }
    }
}
