using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Host
{
  public interface IHeaders
  {
    int Count { get; }

    ICollection<string> Keys { get; }

    string this[string key] { get; set; }
  }
}
