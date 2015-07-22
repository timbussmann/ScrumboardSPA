namespace ScrumboardSPA.Sockets
{
    using Data.Story;

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

        /// <summary>
        /// Creates the story.
        /// </summary>
        /// <param name="createdStory">The created story.</param>
        void CreateStory(UserStory createdStory);

        void DeleteStory(int deletedStoryId);
    }
}