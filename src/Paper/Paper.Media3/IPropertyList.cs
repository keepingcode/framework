using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media3
{
  public interface IPropertyList : IEnumerable
  {
    int Count { get; }

    Type ElementType { get; }

    object this[int index] { get; set; }

    void Add(object item);

    void Remove(int index);
  }
}
