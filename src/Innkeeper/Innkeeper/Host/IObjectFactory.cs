using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Host
{
  public interface IObjectFactory
  {
    object CreateObject(Type type, params object[] extraArgs);

    object GetInstance(Type type);
  }
}
