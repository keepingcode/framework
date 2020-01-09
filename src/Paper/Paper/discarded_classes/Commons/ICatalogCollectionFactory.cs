using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Commons
{
  public interface ICatalogCollectionFactory<T>
    where T : class
  {
    ICatalogCollection<T> CreateCollection();
  }
}