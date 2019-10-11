using Innkeeper.Host;
using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Sandbox.Host
{
  [Expose]
  public class MyModule : Innkeeper.Host.Module
  {
    public override void Configure(IObjectFactoryBuilder builder)
    {
      builder.AddSingleton<MyDependency>();
    }
  }
}
