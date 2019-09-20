using Innkeeper.Modules;
using Innkeeper.Pipelines;
using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Sandbox.Host
{
  [Expose]
  public class MyModule : IModule
  {
    public void Ignite(IObjectFactoryBuilder builder)
    {
      builder.AddSingleton<MyDependency>();
    }
  }
}
