using System;
using System.Collections.Generic;
using System.Text;
using Innkeeper.Host;

namespace Innkeeper.Host
{
  public abstract class Module : IModule
  {
    public virtual void Configure(IObjectFactoryBuilder builder)
    {
      // Nada a fazer...
    }
  }
}
