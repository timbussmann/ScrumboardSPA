namespace ScrumboardSPA.Models
{
    using System.ComponentModel;

    public enum StoryState
    {
        [Description("ToDo")]
        Todo,
        [Description("WIP")]
        Wip,
        [Description("To Verify")]
        ToVerify,
        [Description("Done")]
        Done
    }
}