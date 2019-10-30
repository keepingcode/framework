using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Host
{
  public interface IRouter
  {
    void Map<T>(string route)
      where T : IPipeline;

    void Map(string route, Type pipelineType);
  }
}
