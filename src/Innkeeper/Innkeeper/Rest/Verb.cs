using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Rest
{
  [Flags]
  public enum Verb
  {
    Get = 1,
    Post = 2,
    Put = 4,
    Patch = 8,
    Delete = 16,
    All = Get | Post | Put | Patch | Delete
  }
}