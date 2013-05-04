namespace ScrumboardSPA.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Models;

    public class ScrumboardController : ApiController
    {
        public IEnumerable<StoryState> GetStates()
        {
            return new[]
                {
                    new StoryState("Sprint Backlog"),
                    new StoryState("WIP"), 
                    new StoryState("To Verify"), 
                    new StoryState("Done")
                };
        }
    }
}
