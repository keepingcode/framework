using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Rendering
{
  public interface IObjectFactoryBuilder
  {
    IObjectFactory ObjectFactory { get; }
    IObjectFactoryBuilder AddSingleton(Type contract, object instance);
  }
}
