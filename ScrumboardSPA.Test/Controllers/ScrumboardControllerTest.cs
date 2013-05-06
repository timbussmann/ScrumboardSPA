namespace ScrumboardSPA.Test.Controllers
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Models;
    using NUnit.Framework;
    using ScrumboardSPA.Controllers;

    [TestFixture]
    class ScrumboardControllerTest
    {

        private ScrumboardController testee;

        [SetUp]
        public void SetUp()
        {
            this.testee = new ScrumboardController();
        }

        [Test]
        public void GetStates_ShouldReturnStateDescriptionString()
        {
            IEnumerable<string> result = this.testee.GetStates();

            result.Should().Contain(new[] {"ToDo", "WIP", "To Verify", "Done"});
            result.Should().HaveCount(4);
        }
    }
}
