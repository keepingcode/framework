using Innkeeper.Host;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Toolset;
using Toolset.Collections;
using Toolset.Net;

namespace Innkeeper.Rest
{
  public abstract class RestPipeline : IPipeline
  {
    private static readonly object @lock = new object();
    private static readonly Map<Type, PathIndex<RestPath>> indexCache = new Map<Type, PathIndex<RestPath>>();

    protected IRequestContext Ctx { get; private set; }
    protected IRequest Req { get; private set; }
    protected IResponse Res { get; private set; }
    protected NextAsync Next { get; private set; }

    private PathIndex<RestPath> GetIndex()
    {
      lock (@lock)
      {
        var type = GetType();
        var index = indexCache[type];
        if (index == null)
        {
          index = indexCache[type] = CreateRouteIndex();
        }
        return index;
      }
    }

    public async Task RunAsync(IRequestContext ctx, NextAsync next)
    {
      this.Ctx = ctx;
      this.Req = ctx.Request;
      this.Res = ctx.Response;
      this.Next = next;
      try
      {
        await SendResponseAsync();
      }
      catch (Exception ex)
      {
        await SendFaultAsync(ex);
      }
    }

    protected virtual async Task SendFaultAsync(Exception exception)
    {
      await Task.Yield();
      // Produzir uma resposta de erro com Request.Body.Send()
      throw exception;
    }

    protected virtual async Task SendResponseAsync()
    {
      var index = GetIndex();
      var path = Ctx.Request.Path;

      var keyPath = $"/{Ctx.Request.Method}{Ctx.Request.Path}";
      var route = index.Find(keyPath).FirstOrDefault();
      if (route == null)
      {
        await Next();
        return;
      }

      var args = new HashMap<object>();

      if (route.Pattern.PathArgs != null)
      {
        var tokens = path.Split('/');
        foreach (var arg in route.Pattern.PathArgs)
        {
          string value;

          if (arg.Expanded)
          {
            value = string.Join("/", tokens.Skip(arg.SourceIndex));
            if (!value.StartsWith("/"))
            {
              value = "/" + value;
            }
          }
          else
          {
            value = tokens[arg.SourceIndex];
          }

          var name = arg.Target;
          args[name] = value;
        }
      }

      // Extraindo parametros de URI
      //
      if (route.Pattern.QueryArgs != null)
      {
        var uri = new UriString(Req.RequestUri);
        foreach (var arg in route.Pattern.QueryArgs)
        {
          var name = arg.Target;
          var value = uri.GetArg(arg.Source);
          args[name] = value;
        }
      }

      var parameterValues = new List<object>(route.Method.GetParameters().Length);
      foreach (var parameter in route.Method.GetParameters())
      {
        var value = args[parameter.Name];
        try
        {
          var parameterType = parameter.ParameterType;
          var parameterValue = Change.To(value, parameterType);
          parameterValues.Add(parameterValue);
        }
        catch (Exception ex)
        {
          throw new HttpException(HttpStatusCode.BadRequest,
            $"O parâmetro {parameter.Name} não suporta o valor: {value ?? "(null)"}",
            ex);
        }
      }

      var result = route.Method.Invoke(this, parameterValues.ToArray());
      if (result is Task task)
      {
        await task;
      }
    }

    private PathIndex<RestPath> CreateRouteIndex()
    {
      var flags = BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.Instance;
      var entries =
        from method in GetType().GetMethods(flags)
        from attr in method.GetCustomAttributes()
        let invoke = attr as InvokeAttribute
        where invoke != null
        from verb in invoke.Verbs
        orderby invoke.Route, verb
        select new { method, invoke.Route, verb };

      var index = new PathIndex<RestPath>();

      foreach (var entry in entries)
      {
        try
        {
          var route = entry.Route;
          if (entry.Route == null)
          {
            var name = entry.method.Name;
            if (name.EndsWith("Async"))
            {
              name = name.Substring(0, name.Length - "Async".Length);
            }
            route = $"/{name}";
          }

          var pattern = new UrlPattern(route);
          var restPath = new RestPath
          {
            PipelineType = GetType(),
            Method = entry.method,
            Pattern = pattern,
            Verb = entry.verb
          };

          var keyPath = $"/{entry.verb}{route.Split('?').First()}";
          index.Set(keyPath, restPath);
        }
        catch (Exception ex)
        {
          ex.Trace($"Não é possível mapear uma rota para o método: {entry.method.DeclaringType.FullName}:{entry.method.Name}(...)");
        }
      }

      return index;
    }
  }
}
