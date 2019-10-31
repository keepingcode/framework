using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Host
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public class RouteAttribute : Attribute
  {
    public RouteAttribute(string route)
    {
      this.Route = route;
    }

    public string Route { get; }
  }
}