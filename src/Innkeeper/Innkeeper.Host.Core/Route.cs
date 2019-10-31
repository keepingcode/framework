using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Host.Core
{
  internal class Route : IRoute
  {
    public Route(PathPrefix path, Type pipelineType)
    {
      if (!typeof(IPipeline).IsAssignableFrom(pipelineType))
        throw new InvalidCastException($"O tipo {pipelineType.GetType().FullName} não implementa a interface {typeof(IPipeline).FullName}.");
      this.Path = path ?? throw new NullReferenceException("Uma rota não pode ser criada com um prefixo de caminho nulo: path=(null)");
      this.Type = pipelineType;
    }

    public PathPrefix Path { get; }

    public Type Type { get; }

    public IPipeline CreatePipeline(IObjectFactory objectFactory)
    {
      var pipeline = (IPipeline)objectFactory.CreateObject(Type);
      return pipeline;
    }
  }
}
