using System;
using Autofac;

namespace IntelTypesetDesign.Modules.ServiceProvider;

public class AutofacServiceProvider(ILifetimeScope scope) : IServiceProvider
{
    object IServiceProvider.GetService(Type serviceType)
    {
        return scope.Resolve(serviceType);
    }
}
