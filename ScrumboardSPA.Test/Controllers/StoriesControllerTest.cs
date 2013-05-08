namespace ScrumboardSPA.Test.Controllers
{
    using System.Collections.Generic;
    using Data.Story;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using ScrumboardSPA.Controllers;

    [TestFixture]
    class StoriesControllerTest
    {
        private IStoryRepository storyRepository;

        private StoriesController testee;

        [SetUp]
        public void SetUp()
        {
            this.storyRepository = A.Fake<IStoryRepository>();

            this.testee = new StoriesController(this.storyRepository);
        }

        [Test]
        public void GetStories_ShouldReturnAllAvailableStories()
        {
            var stories = this.SetupStories(new UserStory[]
                                           {
                                               new UserStory{Id = 33}
                                           });

            IEnumerable<UserStory> result = this.testee.GetStories();

            result.ShouldAllBeEquivalentTo(stories);
        }

        private IEnumerable<UserStory> SetupStories(IEnumerable<UserStory> stories)
        {
            A.CallTo(() => this.storyRepository.GetAllStories()).Returns(stories);
            return stories;
        }
    }
}
