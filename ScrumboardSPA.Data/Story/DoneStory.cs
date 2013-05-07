namespace ScrumboardSPA.Data.Story
{
    public class DoneStory : Story
    {
        public override string State
        {
            get { return "Done"; }
        }

        public string Verifier { get; set; }
    }
}