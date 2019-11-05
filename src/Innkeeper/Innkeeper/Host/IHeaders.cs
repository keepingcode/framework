using System;
using System.Collections.Generic;
using System.Text;
using Toolset.Net;

namespace Innkeeper.Host
{
  public interface IHeaders
  {
    int Count { get; }

    ICollection<string> Keys { get; }

    string this[string key] { get; set; }

    string this[Header key] { get; set; }
  }
}
