namespace ScrumboardSPA.Data.Story
{
    using System;
    using State;

    public sealed class UserStory : ICloneable
    {
        public UserStory(int id)
        {
            this.Id = id;
            this.Etag = Guid.NewGuid();
        }

        public UserStory(UserStory userStory) : this(userStory.Id)
        {
            this.Title = userStory.Title;
            this.Description = userStory.Description;
            this.StackRank = userStory.StackRank;
            this.State = userStory.State;
            this.StoryPoints = userStory.StoryPoints;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public StoryState State { get; set; }
        public int StoryPoints { get; set; }
        public int StackRank { get; set; }

        public Guid Etag { get; set; }
        public int Id { get;  private set; }

        public object Clone()
        {
            UserStory clone = new UserStory(this) {Etag = this.Etag};
            return clone;
        }
    }
}
