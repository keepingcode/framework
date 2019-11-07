using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Innkeeper.Rest
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
  public class InvokeAttribute : Attribute
  {
    public InvokeAttribute(string verbs)
    {
      this.Verbs = verbs.Split(',').NotNullOrEmpty().Select(x => x.Trim().ToUpper()).ToArray();
    }

    public InvokeAttribute(string verbs, string route)
    {
      this.Verbs = verbs.Split(',').NotNullOrEmpty().Select(x => x.Trim().ToUpper()).ToArray();
      if (route != null)
      {
        if (!route.StartsWith("/"))
        {
          route = "/" + route;
        }
        this.Route = route;
      }
    }

    public InvokeAttribute(Verb verbs)
      : this(verbs.GetValue(), route: null)
    {
    }

    public InvokeAttribute(Verb verbs, string route)
      : this(verbs.GetValue(), route)
    {
    }

    public string[] Verbs { get; }

    public string Route { get; }
  }
}
