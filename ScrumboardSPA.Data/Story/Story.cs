namespace ScrumboardSPA.Data.Story
{
    public abstract class Story
    {
        public abstract string State { get; }

        public int Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StoryPoints { get; set; }
        public int StackRank { get; set; }

        protected Story(int id)
        {
            Id = id;
        }
    }
}