using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Host
{
  public interface IPipelineRouter
  {
    void Map(IRouter map);
  }
}
