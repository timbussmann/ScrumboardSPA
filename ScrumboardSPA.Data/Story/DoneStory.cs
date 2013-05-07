namespace ScrumboardSPA.Data.Story
{
    public class DoneStory : Story
    {
        public DoneStory(int id) : base(id)
        {
        }

        public override string State
        {
            get { return "Done"; }
        }

        public string Verifier { get; set; }
    }
}