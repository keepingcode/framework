using Innkeeper.Host;
using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Host
{
  public interface IInnkeeperModule
  {
    void Configure(IObjectFactoryBuilder builder);
  }
}
