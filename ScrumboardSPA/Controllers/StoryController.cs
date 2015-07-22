namespace ScrumboardSPA.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using Data.Model;
    using Data.Story;
    using Data.Story.State;
    using Sockets;

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
        public IHttpActionResult SetState(int id, SetStoryStateCommand updateCommand)
        {
            if (updateCommand == null || updateCommand.Etag == Guid.Empty)
            {
                return this.BadRequest();
            }

            UserStory story = this.storyRepository.GetStory(id);
            if (story == null)
            {
                return this.NotFound();
            }

            try
            {
                story.State = updateCommand.State;
                story.Etag = updateCommand.Etag;
                UserStory updatedStory = this.storyRepository.UpdateStory(story);
                this.storyHubService.UpdateStory(updatedStory);
                return this.Ok();
            }
            catch (RepositoryConcurrencyException)
            {
                return this.Conflict();
            }
        }

        [HttpPost]
        public IHttpActionResult CreateStory(CreateUserStoryModel newStory)
        {
            
            if (newStory == null || string.IsNullOrWhiteSpace(newStory.Title))
            {
                return this.BadRequest("Story title is required");
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
            return this.Created(string.Empty, createdStory);
        }

        /// <summary>
        /// Deletes the story.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("delete")]
        public IHttpActionResult DeleteStory(int id)
        {
            if (this.storyRepository.DeleteStory(id))
            {
                this.storyHubService.DeleteStory(id);
                return this.StatusCode(HttpStatusCode.NoContent);
            }

            return this.NotFound();
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
