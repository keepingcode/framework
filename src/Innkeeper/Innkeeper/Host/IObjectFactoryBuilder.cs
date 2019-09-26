using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Host
{
  public interface IObjectFactoryBuilder
  {
    IObjectFactoryBuilder AddSingleton(Type contract, object instance);

    IObjectFactory BuildObjectFactory();
  }
}
