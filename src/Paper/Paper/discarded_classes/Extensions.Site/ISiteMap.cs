using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Extensions.Site
{
  public interface ISiteMap : IRoute
  {
    ICollection<IRoute> Items { get; }
  }
}
