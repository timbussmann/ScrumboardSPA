namespace ScrumboardSPA.Test.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Hosting;
    using System.Web.Http.Routing;
    using Data.Model;
    using Data.Story;
    using Data.Story.State;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using ScrumboardSPA.Controllers;
    using System;
    using System.Collections.Generic;

    using ScrumboardSPA.Sockets;

    [TestFixture]
    class StoryControllerTest
    {
        private IStoryRepository storyRepository;

        private StoryController testee;

        private IStoryHubContextWrapper storyHubService;

        [SetUp]
        public void SetUp()
        {
            this.storyRepository = A.Fake<IStoryRepository>();
            this.storyHubService = A.Fake<IStoryHubContextWrapper>();

            this.testee = new StoryController(this.storyRepository, this.storyHubService);

            SetupControllerForTests(testee);
        }

        [Test]
        public void GetStory_WhenStoryExists_ThenReturnStory()
        {
            const int StoryId = 42;
            this.SetupStories(new[]
                                  {
                                      new UserStory(StoryId)
                                  });

            UserStory result = this.testee.GetStory(StoryId);

            result.Id.Should().Be(StoryId);
        }

        [Test]
        public void GetStory_WhenStoryDoesNotExists_ThenReturn404NotFound()
        {
            this.SetupStories(new[]
                                  {
                                      new UserStory(31  )
                                  });

            Action action = () => this.testee.GetStory(13);

            action.ShouldThrow<HttpResponseException>()
                .And.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public void SetState_WhenStoryExists_ThenReturnSuccess()
        {
            const int UpdatedStoryId = 99;
            A.CallTo(() => this.storyRepository.UpdateStory(A<UserStory>._)).Returns(new UserStory(UpdatedStoryId));

            HttpResponseMessage result = this.testee.SetState(42, StoryState.WorkInProgress, Guid.NewGuid());

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void SetState_WhenStoryExists_ThenCallUpdateStoryOnService()
        {
            const int UpdatedStoryId = 99;
            A.CallTo(() => this.storyRepository.UpdateStory(A<UserStory>._)).Returns(new UserStory(UpdatedStoryId));

            HttpResponseMessage result = this.testee.SetState(42, StoryState.WorkInProgress, Guid.NewGuid());

            A.CallTo(() => this.storyHubService.UpdateStory(A<UserStory>.That.Matches(us => us.Id == UpdatedStoryId))).MustHaveHappened(); 
        }

        [Test]
        public void SetState_WhenStoryDoesNotExists_ThenReturnHttpStatusCodeNotFound()
        {
            A.CallTo(() => this.storyRepository.GetStory(A<int>._)).Returns(null);

            HttpResponseMessage result = this.testee.SetState(22, StoryState.WorkInProgress, Guid.NewGuid());

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public void SetState_WhenConcurrencyException_ThenReturnHttpStatusCodeConflictWithBothStoriesAttached()
        {
            UserStory original = new UserStory(44);
            UserStory requested = new UserStory(55);
            A.CallTo(() => this.storyRepository.UpdateStory(A<UserStory>._)).Throws(
                new RepositoryConcurrencyException(original, requested));

            HttpResponseMessage result = this.testee.SetState(22, StoryState.WorkInProgress, Guid.NewGuid());

            result.StatusCode.Should().Be(HttpStatusCode.Conflict);
            var content = result.Content.As<ObjectContent>().Value.As<ConcurrencyErrorModel>();
            content.Original.As<UserStory>().Should().Be(original);
            content.Requested.Should().Be(requested);
        }

        [Test]
        public void SetState_WhenEmtpyGuidProvided_ThenReturnValidationError()
        {
            HttpResponseMessage result = this.testee.SetState(22, StoryState.WorkInProgress, new Guid());

            result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Test]
        public void CreateStory_ShouldReturnSuccess()
        {
            UserStory expectedUserStory = new UserStory(13);
            A.CallTo(() => this.storyRepository.AddNewStory(A<NewUserStory>._)).Returns(expectedUserStory);

            HttpResponseMessage result = this.testee.CreateStory(new CreateUserStoryModel()
                                                                     {
                                                                         Title = "A sample title"
                                                                     });

            result.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public void CreateStory_WhenNoTitleProvided_ThenDoNotCreateStoryAndReturnError()
        {
            HttpResponseMessage result = this.testee.CreateStory(new CreateUserStoryModel());

            result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
           
            A.CallTo(() => this.storyRepository.AddNewStory(A<NewUserStory>._)).MustNotHaveHappened();
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
