namespace ScrumboardSPA.Data.Story
{
    public class WorkInProgressStory : Story
    {
        public WorkInProgressStory(int id) : base(id)
        {
        }

        public override string State
        {
            get { return "Work in progress"; }
        }
    }
}