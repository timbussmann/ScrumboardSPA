namespace ScrumboardSPA.Test.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Data.Story;
    using Data.Story.State;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using ScrumboardSPA.Controllers;

    [TestFixture]
    class StoriesControllerTest
    {
        private IStoryRepository storyRepository;
        private IStateDetailRepository stateDetailRepository;

        private StoriesController testee;

        [SetUp]
        public void SetUp()
        {
            this.storyRepository = A.Fake<IStoryRepository>();
            this.stateDetailRepository = A.Fake<IStateDetailRepository>();

            this.testee = new StoriesController(this.storyRepository, this.stateDetailRepository);
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

        [Test]
        public void GetStates_ShouldReturnStateDetailsForState()
        {
            var stateDetail = new StateDetail();
            A.CallTo(() => this.stateDetailRepository.GetStateDetails()).Returns(new[]
                                                                                     {
                                                                                         stateDetail
                                                                                     });

            var result = this.testee.GetStates();

            result.Single().Should().Be(stateDetail);
        }

        private IEnumerable<UserStory> SetupStories(IEnumerable<UserStory> stories)
        {
            A.CallTo(() => this.storyRepository.GetAllStories()).Returns(stories);
            return stories;
        }
    }
}
