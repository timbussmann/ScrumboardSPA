namespace ScrumboardSPA.Test.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Model;
    using Data.Story;
    using Data.Story.State;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class StoryRepositoryTest
    {
        private StoryRepository testee;

        [SetUp]
        public void SetUp()
        {
            this.testee = new StoryRepository();
        }

        [Test]
        public void Add_ShouldReturnAddedStory()
        {
            NewUserStory newUserStory = new NewUserStory()
                                         {
                                             Description = "Description",
                                             State = StoryState.ToVerify,
                                             StackRank = 999,
                                             StoryPoints = 3,
                                             Title = "A test story"
                                         };

            UserStory result = this.testee.AddNewStory(newUserStory);

            result.ShouldHave().SharedProperties().EqualTo(newUserStory);
        }

        [Test]
        public void Add_ShouldSetNewEtagOnStory()
        {
            var newUserStory = new NewUserStory();

            UserStory result = this.testee.AddNewStory(newUserStory);

            result.Etag.Should().NotBeEmpty();
        }

        [Test]
        public void Add_ShouldAssignUniqueIdToStory()
        {
            var newStory1 = new NewUserStory();
            var newStory2 = new NewUserStory();

            UserStory result1 = this.testee.AddNewStory(newStory1);
            UserStory result2 = this.testee.AddNewStory(newStory2);

            result1.Id.Should().NotBe(result2.Id);
        }

        [Test]
        public void AddStory_ShouldNotReturnReferenceToStoredStory()
        {
            const StoryState expectedState = StoryState.WorkInProgress;
            var createdStory = this.CreateNewUserStories(1, expectedState).Single();

            createdStory.State = StoryState.Done;
            UserStory result = this.testee.GetAllStories().Single();

            result.State.Should().Be(expectedState);
        }

        [Test]
        public void GetAllStories_ShouldReturnAllAddedStories()
        {
            const int numberOfStories = 100;
            IEnumerable<UserStory> newStories = this.CreateNewUserStories(numberOfStories);

            IEnumerable<UserStory> result = this.testee.GetAllStories();

            result.Should().HaveCount(numberOfStories);
            result.Select(s => s.Id).Should().Contain(newStories.Select(s => s.Id));
        }

        [Test]
        public void GetAllStories_ShouldNotReturnReferencesToStoredStories()
        {
            const StoryState expectedState = StoryState.WorkInProgress;
            this.CreateNewUserStories(1, expectedState).Single();

            UserStory result = this.testee.GetAllStories().Single();
            result.State = StoryState.Done;

            UserStory reloadedResult = this.testee.GetAllStories().Single();
            reloadedResult.State.Should().Be(expectedState);
        }

        [Test]
        public void UpdateStory_WhenEtagIsEqual_ThenUpdateStory()
        {
            const StoryState expectedStoryState = StoryState.Done;
            var createdStory = this.CreateNewUserStories(1, StoryState.WorkInProgress).Single();
            createdStory.State = expectedStoryState;

            UserStory result = this.testee.UpdateStory(createdStory);

            result.State.Should().Be(expectedStoryState);
            result.Etag.Should().NotBe(createdStory.Etag);
        }

        [Test]
        public void UpdateStory_WhenEtagIsNotEqual_ThenThrowException()
        {
            var createdStory = this.CreateNewUserStories(1, StoryState.WorkInProgress).Single();
            createdStory.State = StoryState.WorkInProgress;
            var updatedStory = this.testee.UpdateStory(createdStory);

            createdStory.State = StoryState.Done;
            Action action = () => this.testee.UpdateStory(createdStory);

            var expection = action.ShouldThrow<RepositoryConcurrencyException>();
            expection.And.Requested.ShouldHave().AllProperties().EqualTo(createdStory);
            expection.And.Original.ShouldHave().AllProperties().EqualTo(updatedStory);
        }

        [Test]
        public void UpdateStory_ShouldNotReturnReferenceToStoredStory()
        {
            UserStory createdStory = this.CreateNewUserStories(1).Single();
            createdStory.State = StoryState.WorkInProgress;
            UserStory updatedStory = this.testee.UpdateStory(createdStory);
            updatedStory.State = StoryState.ToVerify;

            var reloadedStory = this.testee.GetAllStories().Single();

            reloadedStory.State.Should().Be(StoryState.WorkInProgress);
        }

        [Test]
        public void GetStory_WhenStoryExsits_ThenReturnStory()
        {
            UserStory createdStory = this.CreateNewUserStories(1).Single();

            UserStory result = this.testee.GetStory(createdStory.Id);

            result.ShouldHave().AllProperties().EqualTo(createdStory);
        }

        [Test]
        public void GetStory_WhenStoryDoesNotExsit_ThenReturnNull()
        {
            UserStory createdStory = this.CreateNewUserStories(1).Single();

            UserStory result = this.testee.GetStory(5);

            result.Should().BeNull();
        }

        [Test]
        public void GetStory_ShouldNotReturnReferenceToStoredStory()
        {
            const StoryState expectedState = StoryState.WorkInProgress;
            UserStory createdStory = this.CreateNewUserStories(1, expectedState).Single();

            UserStory result = this.testee.GetStory(createdStory.Id);
            result.State = StoryState.Done;

            UserStory reloadedResult = this.testee.GetAllStories().Single();
            reloadedResult.State.Should().Be(expectedState);
        }

        //not exsits
        // reftest

        private IEnumerable<UserStory> CreateNewUserStories(int numberOfStories, StoryState state = StoryState.SprintBacklog)
        {
            var stories = new List<UserStory>();
            for (int i = 0; i < numberOfStories; i++)
            {
                stories.Add(this.testee.AddNewStory(new NewUserStory()
                                                        {
                                                            Title = "Story" + i,
                                                            State = state
                                                        }));
            }

            return stories;
        }
    }
}