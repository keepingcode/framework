using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Paper.Media;
using Toolset;

namespace Paper.Media
{
  public static class HttpEntity
  {
    private static IEnumerable<string> EnumerateCauses(string message, Exception exception)
    {
      if (message != null) yield return message;
      while (exception != null)
      {
        yield return exception.Message;
        exception = exception.InnerException;
      }
    }

    public static Ret<Entity> CreateFromRet(Ret ret)
    {
      if (ret.Value is Entity entity)
        return Ret.OK(entity);
      else
        return Create(ret.Status.Code, ret.Fault.Message, ret.Fault.Exception);
    }

    public static Ret<Entity> Create(HttpStatusCode status, string message, Exception exception)
    {
      var causes = EnumerateCauses(message, exception).Distinct();
      var description =
        causes.Any()
          ? string.Join(Environment.NewLine, causes)
          : null;

      var entity = new Entity();

      entity.Class = ClassNames.Status;
      if ((int)status >= 400)
      {
        entity.Class.Add(ClassNames.Error);
      }

      entity.Properties = new PropertyMap();
      entity.Properties.Add("Code", (int)status);
      entity.Properties.Add("Status", status.ToString());

      if (description != null)
        entity.Properties.Add("Description", description);

      if (exception != null)
        entity.Properties.Add("StackTrace", exception.GetStackTrace());

      return Ret.Create(status, entity);
    }

    public static Ret<Entity> Create(HttpStatusCode status, string message)
    {
      return Create(status, message, null);
    }

    public static Ret<Entity> Create(HttpStatusCode status, Exception exception)
    {
      return Create(status, null, exception);
    }

    public static Ret<Entity> Create(HttpStatusCode status)
    {
      return Create(status, null, null);
    }

    public static Ret<Entity> Create(int status, string message, Exception exception)
    {
      return Create((HttpStatusCode)status, message, exception);
    }

    public static Ret<Entity> Create(int status, string message)
    {
      return Create((HttpStatusCode)status, message, null);
    }

    public static Ret<Entity> Create(int status, Exception exception)
    {
      return Create((HttpStatusCode)status, null, exception);
    }

    public static Ret<Entity> Create(int status)
    {
      return Create((HttpStatusCode)status, null, null);
    }

    public static Ret<Entity> Create(string message, Exception exception)
    {
      var status =
        exception is NotImplementedException
          ? HttpStatusCode.NotImplemented
          : HttpStatusCode.InternalServerError;
      return Create(status, message, exception);
    }

    public static Ret<Entity> Create(string message)
    {
      return Create(HttpStatusCode.InternalServerError, message, null);
    }

    public static Ret<Entity> Create(Exception exception)
    {
      var status =
        exception is NotImplementedException
          ? HttpStatusCode.NotImplemented
          : HttpStatusCode.InternalServerError;
      return Create(status, null, exception);
    }

    public static Ret<Entity> Create(UriString route)
    {
      return Create(HttpStatusCode.InternalServerError, null, null);
    }
  }
}
