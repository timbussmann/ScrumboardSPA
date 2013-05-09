namespace ScrumboardSPA.Data.Story.State
{
    using System.Collections.Generic;

    public interface IStateDetailRepository
    {
        IEnumerable<StateDetail> GetStateDetails();
    }
}