using System;
using System.Collections.Generic;
using System.Text;
using Innkeeper.Pipelines;
using Microsoft.Extensions.DependencyInjection;

namespace Innkeeper.Host
{
  internal class ObjectFactoryBuilder : IObjectFactoryBuilder
  {
    private readonly IServiceCollection services;

    public ObjectFactoryBuilder(IServiceCollection services)
    {
      this.services = services;
    }

    public IObjectFactoryBuilder AddObjectFactory()
    {
      this.services.AddSingleton<IObjectFactory>(
        serviceProvider => new ObjectFactory(serviceProvider)
      );
      return this;
    }

    public IObjectFactoryBuilder AddSingleton(Type contract, object instance)
    {
      this.services.AddSingleton(contract, instance);
      return this;
    }

    public IObjectFactory BuildObjectFactory()
    {
      var serviceProvider = services.BuildServiceProvider();
      var objectFactory = serviceProvider.GetService<IObjectFactory>();
      return objectFactory;
    }
  }
}
