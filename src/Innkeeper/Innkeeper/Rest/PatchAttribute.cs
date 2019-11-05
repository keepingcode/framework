using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Rest
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
  public class PatchAttribute : InvokeAttribute
  {
    public PatchAttribute()
      : base("PATCH", null)
    {
    }

    public PatchAttribute(string route)
      : base("PATCH", route)
    {
    }
  }
}
