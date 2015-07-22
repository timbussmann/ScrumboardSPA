namespace ScrumboardSPA.Test.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.Results;
    using Data.Model;
    using Data.Story;
    using Data.Story.State;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using ScrumboardSPA.Controllers;
    using Sockets;

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

            IHttpActionResult result = this.testee.SetState(42,
                                                              new SetStoryStateCommand
                                                                  {
                                                                      State = StoryState.WorkInProgress,
                                                                      Etag = Guid.NewGuid()
                                                                  });

            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public void SetState_WhenStoryExists_ThenCallUpdateStoryOnService()
        {
            const int UpdatedStoryId = 99;
            A.CallTo(() => this.storyRepository.UpdateStory(A<UserStory>._)).Returns(new UserStory(UpdatedStoryId));

            IHttpActionResult result = this.testee.SetState(42,
                                                              new SetStoryStateCommand
                                                                  {
                                                                      State = StoryState.WorkInProgress,
                                                                      Etag = Guid.NewGuid()
                                                                  });

            A.CallTo(() => this.storyHubService.UpdateStory(A<UserStory>.That.Matches(us => us.Id == UpdatedStoryId))).MustHaveHappened(); 
        }

        [Test]
        public void SetState_WhenStoryDoesNotExists_ThenReturnHttpStatusCodeNotFound()
        {
            A.CallTo(() => this.storyRepository.GetStory(A<int>._)).Returns(null);

            IHttpActionResult result = this.testee.SetState(22,
                                                              new SetStoryStateCommand
                                                                  {
                                                                      State = StoryState.WorkInProgress,
                                                                      Etag = Guid.NewGuid()
                                                                  });

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void SetState_WhenConcurrencyException_ThenReturnHttpStatusCodeConflict()
        {
            UserStory original = new UserStory(44);
            UserStory requested = new UserStory(55);
            A.CallTo(() => this.storyRepository.UpdateStory(A<UserStory>._)).Throws(
                new RepositoryConcurrencyException(original, requested));

            IHttpActionResult result = this.testee.SetState(22,
                                                              new SetStoryStateCommand
                                                                  {
                                                                      State = StoryState.WorkInProgress,
                                                                      Etag = Guid.NewGuid()
                                                                  });

            result.Should().BeOfType<ConflictResult>();
        }

        [Test]
        public void SetState_WhenEmtpyGuidProvided_ThenReturnValidationError()
        {
            IHttpActionResult result = this.testee.SetState(22,
                                                              new SetStoryStateCommand
                                                                  {
                                                                      State = StoryState.WorkInProgress,
                                                                      Etag = Guid.Empty
                                                                  });

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public void SetState_WhenInvalidRequestData_ThenReturnBadRequestCode()
        {
            IHttpActionResult result = this.testee.SetState(22, null);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public void CreateStory_ShouldReturnSuccess()
        {
            UserStory expectedUserStory = new UserStory(13);
            A.CallTo(() => this.storyRepository.AddNewStory(A<NewUserStory>._)).Returns(expectedUserStory);

            IHttpActionResult result = this.testee.CreateStory(new CreateUserStoryModel { Title = "A sample title" });

            result.Should().BeOfType<CreatedNegotiatedContentResult<UserStory>>();
        }

        [Test]
        public void CreateStory_WhenNoTitleProvided_ThenDoNotCreateStoryAndReturnError()
        {
            IHttpActionResult result = this.testee.CreateStory(new CreateUserStoryModel());

            result.Should().BeOfType<BadRequestErrorMessageResult>();
            A.CallTo(() => this.storyRepository.AddNewStory(A<NewUserStory>._)).MustNotHaveHappened();
        }

        [Test]
        public void DeleteStory_WhenStoryDeletedSuccessful_ThenShouldReturnSuccess()
        {
            // Arrange
            const int StoryId = 2;
            A.CallTo(() => this.storyRepository.DeleteStory(A<int>.That.Matches(i => i == StoryId))).Returns(true);

            // Act
            var result = this.testee.DeleteStory(StoryId);

            // Assert
            result.Should().BeOfType<StatusCodeResult>()
                .Which.StatusCode.Should().Be(HttpStatusCode.NoContent);
        } 

        [Test]
        public void DeleteStory_WhenStoryDeletedNotSuccessful_ThenShouldReturnNotFound()
        {
            // Arrange
            const int StoryId = 2;
            A.CallTo(() => this.storyRepository.DeleteStory(A<int>.That.Matches(i => i == StoryId))).Returns(false);

            // Act
            IHttpActionResult result = this.testee.DeleteStory(StoryId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        } 

        private IEnumerable<UserStory> SetupStories(IEnumerable<UserStory> stories)
        {
            A.CallTo(() => this.storyRepository.GetAllStories()).Returns(stories);
            return stories;
        }
    }
}
