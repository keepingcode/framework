using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Pipelines
{
  public interface IObjectFactory
  {
    object CreateObject(Type type, params object[] extraArgs);

    object GetInstance(Type type);
  }
}
