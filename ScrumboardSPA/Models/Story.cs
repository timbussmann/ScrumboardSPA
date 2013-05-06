namespace ScrumboardSPA.Models
{
    public class Story
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public StoryState State { get; set; }
        public int StoryPoints { get; set; }
        public int StackRank { get; set; }
    }
}