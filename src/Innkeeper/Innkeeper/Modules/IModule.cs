using Innkeeper.Pipelines;
using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Modules
{
  public interface IModule
  {
    void Ignite(IObjectFactoryBuilder builder);
  }
}
