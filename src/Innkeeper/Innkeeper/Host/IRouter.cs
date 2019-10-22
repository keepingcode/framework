using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Host
{
  public interface IRouter
  {
    void Map(string route);
  }
}
