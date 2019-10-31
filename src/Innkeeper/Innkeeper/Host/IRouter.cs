using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Host
{
  public interface IRouter
  {
    ICollection<string> Keys { get; }

    IRouteCollection this[string key] { get; }

    void Add(IRoute route);

    IRoute Map<T>(PathPrefix route) where T : IPipeline;

    IRoute Map(PathPrefix route, Type pipelineType);

    ICollection<IRoute> Find(string path);
  }
}
