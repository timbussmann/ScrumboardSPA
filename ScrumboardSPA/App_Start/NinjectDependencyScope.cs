// --------------------------------------------------------------------------------------------------------------------
// <summary>
//  Ninject DependencyScope and Resolver for Web API. Based on https://github.com/WebApiContrib/WebApiContrib.IoC.Ninject
//  with some modifications (fixing Singletonscope, see: http://stackoverflow.com/questions/11356864/ninject-insingletonscope-with-web-api-rc )
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ScrumboardSPA
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web.Http.Dependencies;
    using Ninject;
    using Ninject.Syntax;

    public class NinjectDependencyScope : IDependencyScope
    {
        private IResolutionRoot resolver;

        internal NinjectDependencyScope(IResolutionRoot resolver)
        {
            Contract.Assert(resolver != null);

            this.resolver = resolver;
        }

        public void Dispose()
        {
            resolver = null;
        }

        public object GetService(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed");

            return resolver.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed");

            return resolver.GetAll(serviceType);
        }
    }

    public class NinjectResolver : NinjectDependencyScope, IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(this.kernel);
        }
    }
}