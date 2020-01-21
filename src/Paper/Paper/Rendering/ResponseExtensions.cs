using Innkeeper.Host;
using Paper.Media;
using Paper.Media.Design;
using Paper.Media.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Toolset;
using Toolset.Net;
using Toolset.Reflection;

namespace Paper.Rendering
{
  public static class ResponseExtensions
  {
    public static async Task SendEntityStatusAsync(this IResponse res, HttpStatusCode status)
    {
      var req = res.Context.Request;
      var entity = Entity.Create(status);
      await WriteEntityAsync(res, entity, status);
    }

    public static async Task SendEntityStatusAsync(this IResponse res, Ret ret)
    {
      var req = res.Context.Request;
      var entity = Entity.Create(ret);
      await WriteEntityAsync(res, entity, ret.Status.Code);
    }

    public static async Task SendEntityObjectAsync(this IResponse res, object payload)
    {
      var entity = payload as IEntity;
      if (entity == null)
      {
        entity = CreateEntity(payload);
      }
      await WriteEntityAsync(res, entity);
    }

    public static async Task SendEntityAsync<T>(this IResponse res, T payload, Func<T, IEntity> mediaFormat)
    {
      var req = res.Context.Request;
      var mediaType = req.Headers[HeaderNames.Accept] ?? MimeTypeNames.JsonSiren;
      var entity = mediaFormat?.Invoke(payload) ?? CreateEntity(payload);
      await WriteEntityAsync(res, entity);
    }

    public static async Task SendEntityAsync(this IResponse res, IEntity entity)
    {
      await WriteEntityAsync(res, entity);
    }

    private static async Task WriteEntityAsync(IResponse res, IEntity entity, HttpStatusCode status = HttpStatusCode.OK)
    {
      var req = res.Context.Request;
      var accept = req.Headers[HeaderNames.Accept] ?? MimeTypeNames.JsonSiren;
      var contentType = MediaSerializer.ParseMediaType(accept);

      var hasSelf = (
        from link in entity.OfType<Link>()
        from rel in link.OfType<Rel>()
        where rel.Name == Rel.Self.Name
        select link
      ).Any();

      if (!hasSelf)
      {
        var link = new Link();
        link.Href = req.RequestUri;
        link.Add(Rel.Self);
        entity.Add(link);
      }

      res.Status = status;
      res.Headers[HeaderNames.ContentType] = $"{contentType}; charset=UTF-8";

      var serializer = new MediaSerializer(contentType);
      serializer.Serialize(entity, res.Body);

      await Task.Yield();
    }

    private static IEntity CreateEntity(object payload)
    {
      var entity = new Entity();
      foreach (var property in payload._GetPropertyNames())
      {
        var value = payload._Get(property);
        entity.Add(new Property(property, value));
      }
      return entity;
    }
  }
}
