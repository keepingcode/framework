using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media
{
  public interface IValue : ICloneable
  {
    object Value { get; }

    new IValue Clone();
  }
}
