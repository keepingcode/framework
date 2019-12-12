using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public interface IValueCollection : IValue, IEnumerable, IEnumerable<IValue>
  {
    int Count { get; }

    IValue this[int index] { get; }
  }
}