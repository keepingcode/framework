using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Host
{
  public interface IModuleCollection : IEnumerable<IModule>
  {
    int Count { get; }

    IModule this[int index] { get; }
  }
}
