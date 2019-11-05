using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Rest
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
  public class PostAttribute : InvokeAttribute
  {
    public PostAttribute()
      : base("POST", null)
    {
    }

    public PostAttribute(string route)
      : base("POST", route)
    {
    }
  }
}
