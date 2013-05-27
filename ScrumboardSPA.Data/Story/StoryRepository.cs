﻿namespace ScrumboardSPA.Data.Story
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Model;
    using ScrumboardSPA.Data.Story.State;

    public class StoryRepository : IStoryRepository
    {
        private readonly object lockObject = new object();
        private readonly List<UserStory> userStories = new List<UserStory>();

        public IEnumerable<UserStory> GetAllStories()
        {
            return this.userStories.Select(s => (UserStory)s.Clone());
        }

        public UserStory AddNewStory(NewUserStory newStory)
        {
            int id = this.GetNewId();
            var story = new UserStory(id)
                                  {
                                      Title = newStory.Title,
                                      Description = newStory.Description,
                                      State = newStory.State,
                                      StackRank = newStory.StackRank,
                                      StoryPoints = newStory.StoryPoints
                                  };
            
            this.userStories.Add(story);
            return (UserStory)story.Clone();
        }

        public UserStory UpdateStory(UserStory story)
        {
            var originalStory = this.userStories.Find(s => s.Id == story.Id);

            if (originalStory.Etag != story.Etag)
            {
                throw new RepositoryConcurrencyException(originalStory, story);
            }

            int index = this.userStories.IndexOf(originalStory);
            UserStory newStory = new UserStory(story);
            this.userStories[index] = newStory;

            return (UserStory)newStory.Clone();
        }

        public UserStory GetStory(int id)
        {
            var story = this.userStories.SingleOrDefault(s => s.Id == id);

            if (story != null)
            {
                return (UserStory)story.Clone();
            }

            return null;
        }

        public bool DeleteStory(int id)
        {
            var story = this.userStories.SingleOrDefault(s => s.Id == id);
            return this.userStories.Remove(story);
        }

        private int GetNewId()
        {
            lock (this.lockObject)
            {
                return userStories.Any() ? userStories.Max(s => s.Id) + 1 : 1;
            }
        }
    }
}