using Innkeeper.Host;
using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Sandbox.Host
{
  [Expose]
  public class MyInnkeerperModule : IInnkeeperModule
  {
    public void Configure(IObjectFactoryBuilder builder)
    {
      builder.AddSingleton<SomeDependency>();
    }
  }
}
