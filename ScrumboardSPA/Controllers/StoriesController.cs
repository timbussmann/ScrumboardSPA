namespace ScrumboardSPA.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Data.Story;
    using Data.Story.State;

    public class StoriesController : ApiController
    {
        private readonly IStoryRepository storyRepository;
        private readonly IStateDetailRepository stateDetailRepository;

        public StoriesController() : this(new StoryRepository(), new StateDetailRepository())
        {
            //TODO use ninject and remove this constructor!
        }

        public StoriesController(IStoryRepository storyRepository, IStateDetailRepository stateDetailRepository)
        {
            this.storyRepository = storyRepository;
            this.stateDetailRepository = stateDetailRepository;
        }

        [ActionName("Default")]
        public IEnumerable<UserStory> GetStories()
        {
            return this.storyRepository.GetAllStories();
        }

        [ActionName("States")]
        public IEnumerable<StateDetail> GetStates()
        {
            return this.stateDetailRepository.GetStateDetails();
        } 
    }
}
