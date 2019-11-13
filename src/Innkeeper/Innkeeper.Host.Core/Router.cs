using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset;
using Toolset.Collections;

namespace Innkeeper.Host.Core
{
  internal class Router : IRouter
  {
    private readonly object @lock = new object();

    private readonly HashMap<RouteCollection> map = new HashMap<RouteCollection>();

    private Router()
    {
      // nada a fazer
    }

    public ICollection<string> Keys => map.Keys;

    public IRouteCollection this[string key] => map[key];

    public void Add(IRoute route)
    {
      lock (@lock)
      {
        var routes = map[route.Path];
        if (routes == null)
        {
          routes = map[route.Path] = new RouteCollection();
        }
        routes.Add(route);
      }
    }

    public IRoute Map<T>(PathPrefix route) where T : IPipeline
    {
      var instance = new Route(route, typeof(T));
      Add(instance);
      return instance;
    }

    public IRoute Map(PathPrefix route, Type pipelineType)
    {
      var instance = new Route(route, pipelineType);
      Add(instance);
      return instance;
    }

    public ICollection<IRoute> Find(string path)
    {
      if (!path.EndsWith("/")) path += "/";
      if (!path.StartsWith("/")) path = "/" + path;

      // FIXME: O algoritmo deveria usar um índice em vez de varregar a fila
      var routes =
        (
          from key in map.Keys
          where path.StartsWith(key)
          orderby key.Length descending
          select map[key]
        )
        .SelectMany()
        .ToArray();

      return routes;
    }

    public static Router Create(WebApp webApp)
    {
      var router = new Router();

      var routerTypes = ExposedTypes.GetTypes<IPipelineRouter>().ToArray();
      foreach (var routerType in routerTypes)
      {
        try
        {
          IPipelineRouter instance;

          // XXX: O roteador customizado pode ser instanciado com ou sem IWebApp,
          // mas isso é muito pouco. Melhor seria disponibilizar todos os objetos
          // de contexto da plataforma disponíveis no ato da instanciação do roteador.
          var ctor = routerType.GetConstructor(new[] { typeof(IWebApp) });
          if (ctor != null)
          {
            instance = (IPipelineRouter)ctor.Invoke(new[] { webApp });
          }
          else
          {
            instance = (IPipelineRouter)Activator.CreateInstance(routerType);
          }

          instance.Map(router);
        }
        catch (Exception ex)
        {
          ex.Trace();
        }
      }

      var pipelineTypes = ExposedTypes.GetTypes<IPipeline>().ToArray();
      foreach (var pipelineType in pipelineTypes)
      {
        try
        {
          var instance = new PipelineRouter(pipelineType, webApp.UrlPrefix);
          instance.Map(router);
        }
        catch (Exception ex)
        {
          ex.Trace();
        }
      }

      return router;
    }

  }
}