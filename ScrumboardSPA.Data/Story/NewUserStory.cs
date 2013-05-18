namespace ScrumboardSPA.Data.Story
{
    using State;

    public class NewUserStory
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public StoryState State { get; set; }
        public int StackRank { get; set; }
        public int StoryPoints { get; set; }
    }
}