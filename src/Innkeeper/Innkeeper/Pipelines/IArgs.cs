using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Innkeeper.Pipelines
{
  public interface IArgs : IEnumerable<KeyValuePair<string, Var>>
  {
    int Count { get; }

    ICollection<string> Keys { get; }

    Var this[string key] { get; }
  }
}
