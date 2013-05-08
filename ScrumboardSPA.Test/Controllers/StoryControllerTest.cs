namespace ScrumboardSPA.Test.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Hosting;
    using System.Web.Http.Routing;
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

            SetupControllerForTests(testee);
        }

        [Test]
        public void GetStory_WhenStoryExists_ThenReturnStory()
        {
            const int StoryId = 42;
            this.SetupStories(new UserStory[]
                                  {
                                      new UserStory {Id = StoryId}
                                  });

            UserStory result = this.testee.GetStory(StoryId);

            result.Id.Should().Be(StoryId);
        }

        [Test]
        public void GetStory_WhenStoryDoesNotExists_ThenReturn404NotFound()
        {
            this.SetupStories(new UserStory[]
                                  {
                                      new UserStory {Id = 31}
                                  });

            Action action = () => this.testee.GetStory(13);

            action.ShouldThrow<HttpResponseException>()
                .And.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public void SetState_WhenStoryExists_ThenSetNewStateOnStory()
        {
            const int StoryId = 42;
            var story = new UserStory {Id = StoryId, State = StoryState.SprintBacklog};
            this.SetupStories(new UserStory[]
                                  {
                                      story
                                  });

            this.testee.SetState(StoryId, StoryState.WorkInProgress);

            story.State.Should().Be(StoryState.WorkInProgress);
        }

        private IEnumerable<UserStory> SetupStories(IEnumerable<UserStory> stories)
        {
            A.CallTo(() => this.storyRepository.GetAllStories()).Returns(stories);
            return stories;
        }

        private static void SetupControllerForTests(ApiController controller)
        {
            // see: http://www.peterprovost.org/blog/2012/06/16/unit-testing-asp-dot-net-web-api/

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Put, "http://localhost/api/story");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "story" } });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }
    }
}
