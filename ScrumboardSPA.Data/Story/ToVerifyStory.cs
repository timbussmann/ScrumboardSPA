namespace ScrumboardSPA.Data.Story
{
    public class ToVerifyStory : Story
    {
        public ToVerifyStory(int id) : base(id)
        {
        }

        public override string State
        {
            get { return "To verify"; }
        }
    }
}