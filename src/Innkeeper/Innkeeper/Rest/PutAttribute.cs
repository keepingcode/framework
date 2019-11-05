using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Rest
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
  public class PutAttribute : InvokeAttribute
  {
    public PutAttribute()
      : base("PUT", null)
    {
    }

    public PutAttribute(string route)
      : base("PUT", route)
    {
    }
  }
}
