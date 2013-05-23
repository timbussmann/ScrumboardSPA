namespace ScrumboardSPA.Sockets
{
    using ScrumboardSPA.Data.Story;

    /// <summary>
    /// The StoryHubService interface.
    /// </summary>
    public interface IStoryHubContextWrapper
    {
        /// <summary>
        /// Updates the story.
        /// </summary>
        /// <param name="updatedStory">The user story.</param>
        void UpdateStory(UserStory updatedStory);
    }
}