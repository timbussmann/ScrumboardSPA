namespace ScrumboardSPA
{
    using Data.Story;
    using Data.Story.State;
    using Ninject.Modules;
    using Sockets;

    public class ServicesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IStoryRepository>().To<StoryRepository>()
                .InSingletonScope()
                .OnActivation(x => x.Initialize());

            this.Bind<IStateDetailRepository>().To<StateDetailRepository>()
                .InSingletonScope();

            this.Bind<IStoryHubContextWrapper>().To<StoryHubContextWrapper>();
        }
    }
}