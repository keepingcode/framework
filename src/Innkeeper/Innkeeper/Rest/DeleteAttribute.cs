using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Rest
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
  public class DeleteAttribute : InvokeAttribute
  {
    public DeleteAttribute()
      : base("DELETE", null)
    {
    }

    public DeleteAttribute(string route)
      : base("DELETE", route)
    {
    }
  }
}
