﻿namespace ScrumboardSPA.Test.Controllers
{
    using System.Collections.Generic;
    using Data.Story;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using ScrumboardSPA.Controllers;

    [TestFixture]
    class ScrumboardControllerTest
    {
        private IStoryRepository storyRepository;

        private ScrumboardController testee;

        [SetUp]
        public void SetUp()
        {
            this.storyRepository = A.Fake<IStoryRepository>();

            this.testee = new ScrumboardController(this.storyRepository);
        }

        [Test]
        public void GetStories_ShouldReturnAllAvailableStories()
        {
            var stories = this.SetupStories(new Story[]
                                           {
                                               new DoneStory(33)
                                           });

            IEnumerable<Story> result = this.testee.GetStories();

            result.ShouldAllBeEquivalentTo(stories);
        }

        

        private IEnumerable<Story> SetupStories(IEnumerable<Story> stories)
        {
            A.CallTo(() => this.storyRepository.GetAllStories()).Returns(stories);
            return stories;
        }
    }
}
