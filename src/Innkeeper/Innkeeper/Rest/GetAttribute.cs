using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Rest
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
  public class GetAttribute : InvokeAttribute
  {
    public GetAttribute()
      : base("GET", null)
    {
    }

    public GetAttribute(string route)
      : base("GET", route)
    {
    }
  }
}
