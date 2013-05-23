[assembly: WebActivator.PreApplicationStartMethod(typeof(ScrumboardSPA.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(ScrumboardSPA.App_Start.NinjectWebCommon), "Stop")]

namespace ScrumboardSPA.App_Start
{
    using System;
    using System.Web;
    using System.Web.Http;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using ScrumboardSPA.Data.Story;
    using ScrumboardSPA.Data.Story.State;
    using ScrumboardSPA.Sockets;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);

            // Fill in some demo data
            var repo = bootstrapper.Kernel.Get<IStoryRepository>();
            repo.AddNewStory(new NewUserStory()
                                 {
                                     Title = "Drag and drop",
                                     Description =
                                         "As a user I want to be able to drag and drop user stories on the board.",
                                     StackRank = 990,
                                     State = StoryState.Done,
                                     StoryPoints = 5
                                 });
            repo.AddNewStory(new NewUserStory()
                                 {
                                     Title = "SignalR support",
                                     Description =
                                         "As a user I want to be notified when another team member changes the state of a story on the board.",
                                     StackRank = 950,
                                     State = StoryState.WorkInProgress,
                                     StoryPoints = 8
                                 });
            repo.AddNewStory(new NewUserStory()
                                 {
                                     Title = "Show Scrum board in offline mode",
                                     Description =
                                         "As a user I want to see the scrumboard even when I am offline",
                                     StackRank = 940,
                                     State = StoryState.SprintBacklog,
                                     StoryPoints = 3
                                 });
            repo.AddNewStory(new NewUserStory()
                                 {
                                     Title = "Move Stories in offline mode",
                                     Description =
                                         "As a user I want to move stories when I am offline and get them synced when connection is reestablished.",
                                     StackRank = 920,
                                     State = StoryState.SprintBacklog,
                                     StoryPoints = 8
                                 });
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);

            // Install our Ninject-based IDependencyResolver into the Web API config
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);

            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IStoryRepository>().To<StoryRepository>().InSingletonScope();
            kernel.Bind<IStateDetailRepository>().To<StateDetailRepository>().InSingletonScope();
            kernel.Bind<IStoryHubContextWrapper>().To<StoryHubContextWrapper>();
        }        
    }
}
