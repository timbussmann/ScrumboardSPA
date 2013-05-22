namespace ScrumboardSPA.Test.Model
{
    using FluentAssertions;
    using NUnit.Framework;
    using ScrumboardSPA.Data.Conflicts;
    using ScrumboardSPA.Data.Story;

    [TestFixture]
    public class ConflictRepositoryTest
    {
        private ConflictRepository testee;

        [SetUp]
        public void SetUp()
        {
            this.testee = new ConflictRepository();
        }

        [Test]
        public void AddConflict_ShouldReturnUniqueConflictIds()
        {
            UserStory story1 = new UserStory(1);
            UserStory story2 = new UserStory(2);

            string id1 = this.testee.AddConflict(story1, story2);
            string id2 = this.testee.AddConflict(story1, story2);

            id1.Should().NotBe(id2);
        }

        [Test]
        public void GetConflict_WhenConflictExists_ThenReturnConflict()
        {
            UserStory story1 = new UserStory(1);
            UserStory story2 = new UserStory(2);
            string conflictId = this.testee.AddConflict(story1, story2);

            var conflict = this.testee.GetConflict(conflictId);
        }

        //TODO when adding return unique conflict id
        //TODO get and conflict exists then return existing conflict
        //TODO get and no conflict exists then return ??
        //TODO resolve when conflict exists then resolve all conflicts concerning this story
    }
}