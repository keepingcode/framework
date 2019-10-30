using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Host
{
  [AttributeUsage(AttributeTargets.Class)]
  public class RouteAttribute : Attribute
  {
    public RouteAttribute(string route)
    {
      this.Route = route;
    }

    public string Route { get; }
  }
}
