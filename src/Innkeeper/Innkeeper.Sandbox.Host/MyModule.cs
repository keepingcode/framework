using Innkeeper.Host;
using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Innkeeper.Sandbox.Host
{
  [Expose]
  public class MyInnkeerperModule : Innkeeper.Host.IInnkeeperModule
  {
    public void ConfigureServices(IObjectFactoryBuilder builder)
    {
      builder.AddSingleton<MyDependency>();
    }
  }
}
