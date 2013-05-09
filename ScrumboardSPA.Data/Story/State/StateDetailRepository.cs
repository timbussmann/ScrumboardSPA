namespace ScrumboardSPA.Data.Story.State
{
    using System.Collections.Generic;

    public class StateDetailRepository : IStateDetailRepository
    {
        public IEnumerable<StateDetail> GetStateDetails()
        {
            return new List<StateDetail>()
            {
                new StateDetail {State = StoryState.SprintBacklog, Name = "Sprint Backlog"},
                new StateDetail {State =  StoryState.WorkInProgress, Name = "Work in progress"},
                new StateDetail {State = StoryState.ToVerify, Name = "To verify"},
                new StateDetail {State = StoryState.Done, Name = "Done"}
            };
        } 
    }
}
