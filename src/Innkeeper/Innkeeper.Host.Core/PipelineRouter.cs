using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Toolset.Reflection;

namespace Innkeeper.Host.Core
{
  public class PipelineRouter : IPipelineRouter
  {
    private Type pipelineType;
    private string urlPrefix;

    public PipelineRouter(Type pipelineType, string urlPrefix)
    {
      if (!typeof(IPipeline).IsAssignableFrom(pipelineType))
        throw new InvalidCastException($"O tipo {pipelineType.GetType().FullName} não implementa a interface {typeof(IPipeline).FullName}");

      this.pipelineType = pipelineType;
      this.urlPrefix = urlPrefix;
    }

    public void Map(IRouter map)
    {
      var attr = pipelineType._GetAttribute<RouteAttribute>();

      var route = attr?.Route;
      if (route == null)
      {
        route = pipelineType.FullName;
      }

      route = string.Concat(urlPrefix, "/", route);

      map.Map(route, pipelineType);
    }
  }
}
