namespace ScrumboardSPA.Data.Story
{
    using State;

    public class UserStory
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public int Id { get;  set; }
        public StoryState State { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StoryPoints { get; set; }
        public int StackRank { get; set; }
    }
}
