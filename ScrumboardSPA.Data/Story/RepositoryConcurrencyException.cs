namespace ScrumboardSPA.Data.Model
{
    using System;
    using Story;

    public class RepositoryConcurrencyException : Exception
    {
        public RepositoryConcurrencyException(UserStory original, UserStory requested)
        {
            this.Original = original;
            this.Requested = requested;
        }

        public UserStory Requested { get; set; }
        public UserStory Original { get; set; }
    }
}