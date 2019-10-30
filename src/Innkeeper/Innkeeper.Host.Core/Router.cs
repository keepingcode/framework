using System;
using System.Collections.Generic;

namespace Innkeeper.Host.Core
{
  internal class Router : IRouter
  {
    public void Map<T>(string route) where T : IPipeline => Map(route, typeof(T));

    public void Map(string route, Type pipelineType)
    {

    }
  }
}