using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Innkeeper.Pipelines;
using Microsoft.Extensions.DependencyInjection;

namespace Innkeeper.Host
{
  internal class ObjectFactory : IObjectFactory
  {
    private readonly IServiceProvider serviceProvider;

    public ObjectFactory(IServiceProvider serviceProvider)
    {
      this.serviceProvider = serviceProvider;
    }

    public object CreateObject(Type type, params object[] args)
    {
      return ActivatorUtilities.CreateInstance(serviceProvider, type, args);
    }

    public object GetInstance(Type type)
    {
      return serviceProvider.GetService(type);
    }
  }
}