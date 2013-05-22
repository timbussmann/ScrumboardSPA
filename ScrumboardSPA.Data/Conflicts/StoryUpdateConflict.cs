namespace ScrumboardSPA.Data.Conflicts
{
    using ScrumboardSPA.Data.Story;

    class StoryUpdateConflict
    {
        public string ConflictId { get; set; }

        public UserStory Original { get; set; }

        public UserStory Requested { get; set; }
    }
}
