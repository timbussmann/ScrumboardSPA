namespace ScrumboardSPA.Data.Conflicts
{
    using System;
    using System.Collections.Generic;
    using ScrumboardSPA.Data.Story;

    public class ConflictRepository : IConflictRepository
    {
        private readonly Dictionary<string, StoryUpdateConflict> conflicts = new Dictionary<string, StoryUpdateConflict>();  
        
        public string AddConflict(UserStory original, UserStory requested)
        {
            string id = Guid.NewGuid().ToString();
            var conflict = new StoryUpdateConflict()
                {
                    ConflictId = id,
                    Original = original,
                    Requested = requested
                };
            this.conflicts.Add(id, conflict);
            return id;
        }
    }
}