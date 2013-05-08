namespace ScrumboardSPA.Data.Story
{
    public class UserStory
    {
        public int Id { get;  set; }
        public StoryState State { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StoryPoints { get; set; }
        public int StackRank { get; set; }
    }
}
