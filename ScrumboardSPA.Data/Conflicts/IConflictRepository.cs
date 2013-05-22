namespace ScrumboardSPA.Data.Conflicts
{
    using ScrumboardSPA.Data.Story;

    public interface IConflictRepository
    {
        string AddConflict(UserStory original, UserStory requested);


    }
}