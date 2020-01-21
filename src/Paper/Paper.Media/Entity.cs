using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Toolset;
using Toolset.Collections;

namespace Paper.Media
{
  public class Entity : NodeCollection, INode, IEntity
  {
    public object Tag { get; set; }

    public virtual string Title { get => Get<string>(); set => Set(value); }

    #region Factories

    public static IEntity Create(Ret ret)
    {
      if (ret.Value is IEntity entity)
        return entity;

      return Create(ret.Status.Code, ret.Fault.Message, ret.Fault.Exception);
    }

    public static IEntity Create(HttpStatusCode status)
    {
      return Create(status, null, null);
    }

    public static IEntity Create(HttpStatusCode status, string message)
    {
      return Create(status, null, null);
    }

    public static IEntity Create(Exception exception)
    {
      return Create(HttpStatusCode.InternalServerError, null, exception);
    }

    public static IEntity Create(Exception exception, string message)
    {
      return Create(HttpStatusCode.InternalServerError, message, exception);
    }

    public static IEntity Create(HttpStatusCode status, string message, Exception exception)
    {
      var causes = EnumerateCauses(message, exception).Distinct();
      var description =
        causes.Any()
          ? string.Join(Environment.NewLine, causes)
          : null;

      var entity = new Entity();

      entity.Add(Class.Status);

      if ((int)status >= 400)
      {
        entity.Add(Class.Error);
      }

      entity.Add(new Property("Code", (int)status));
      entity.Add(new Property("Status", status.ToString().ChangeCase(TextCase.ProperCase)));

      if (description != null)
      {
        entity.Add(new Property("Description", description));
      }

      if (exception != null)
      {
        entity.Add(new Property("StackTrace", exception.GetStackTrace()));
      }

      return entity;
    }

    private static IEnumerable<string> EnumerateCauses(string message, Exception exception)
    {
      if (message != null) yield return message;
      while (exception != null)
      {
        yield return exception.Message;
        exception = exception.InnerException;
      }
    }

    #endregion
  }
}
