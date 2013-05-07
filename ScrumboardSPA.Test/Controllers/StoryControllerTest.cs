namespace ScrumboardSPA.Test.Controllers
{
    using System.Net;
    using System.Web.Http;
    using Data.Story;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using ScrumboardSPA.Controllers;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    class StoryControllerTest
    {
        private IStoryRepository storyRepository;

        private StoryController testee;

        [SetUp]
        public void SetUp()
        {
            this.storyRepository = A.Fake<IStoryRepository>();

            this.testee = new StoryController(this.storyRepository);
        }

        [Test]
        public void GetStory_WhenStoryExists_ThenReturnStory()
        {
            const int StoryId = 42;
            this.SetupStories(new Story[]
                                  {
                                      new DoneStory(StoryId)
                                  });

            Story result = this.testee.GetStory(StoryId);

            result.Id.Should().Be(StoryId);
        }

        [Test]
        public void GetStory_WhenStoryDoesNotExists_ThenReturn404NotFound()
        {
            this.SetupStories(new Story[]
                                  {
                                      new DoneStory(42)
                                  });

            Action action = () => this.testee.GetStory(13);

            action.ShouldThrow<HttpResponseException>()
                .And.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private IEnumerable<Story> SetupStories(IEnumerable<Story> stories)
        {
            A.CallTo(() => this.storyRepository.GetAllStories()).Returns(stories);
            return stories;
        }
    }
}
