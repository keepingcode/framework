using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Commons
{
  public interface ICatalogCollection<T>
    where T : class
  {
    string Name { get; }

    ICollection<T> Items { get; }
  }
}