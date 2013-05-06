namespace ScrumboardSPA.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Web.Http;
    using Models;

    public class ScrumboardController : ApiController
    {
        public IEnumerable<string> GetStates()
        {
            var values = Enum.GetValues(typeof(StoryState));
            foreach (Enum value in values)
            {
                FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
                var descriptionAttribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute), false);
                yield return descriptionAttribute.Description;
            }
        }

        public IEnumerable<Story> GetStories()
        {
            return new[]
                       {
                           new Story{Title = "Show all stories on the scrumboard", Description = "As a developer, I want to see all stories of the current sprint.", State = StoryState.Wip, StackRank = 999, StoryPoints = 3},
                           new Story{Title = "Progress story", Description = "As a developer, I want to adjust the state of a story according to the developement process.", State = StoryState.Todo, StackRank = 900, StoryPoints = 5}
                       };
        } 

    }
}
