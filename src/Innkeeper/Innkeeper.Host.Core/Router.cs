using System.Collections.Generic;

namespace Innkeeper.Host.Core
{
  internal class Router : List<string>, IRouter
  {
    public void Map(string route)
    {
      this.Add(route);
    }
  }
}