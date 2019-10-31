using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Host
{
  public interface IRouteCollection : IEnumerable<IRoute>
  {
    int Count { get; }

    IRoute this[int index] { get; }
  }
}
