namespace ScrumboardSPA.Data.Story
{
    public class SprintBacklogStory : Story
    {
        public SprintBacklogStory(int id) : base(id)
        {
        }

        public override string State { get { return "Sprint Backlog"; }}
    }
}