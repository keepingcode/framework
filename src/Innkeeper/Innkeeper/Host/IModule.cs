using Innkeeper.Host;
using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Host
{
  public interface IModule
  {
    void Install(IObjectFactoryBuilder builder);
  }
}
