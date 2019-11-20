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
      var entity = HttpEntity.Create(req.RequestUri, status);
      await WriteEntityAsync(res, entity, status);
    }

    public static async Task SendEntityStatusAsync(this IResponse res, Ret ret)
    {
      var req = res.Context.Request;
      var entity = HttpEntity.CreateFromRet(req.RequestUri, ret);
      await WriteEntityAsync(res, entity, ret.Status.Code);
    }

    public static async Task SendEntityObjectAsync(this IResponse res, object payload)
    {
      var entity = payload as Entity;
      if (entity == null)
      {
        entity = CreateEntity(payload);
      }
      await WriteEntityAsync(res, entity);
    }

    public static async Task SendEntityAsync<T>(this IResponse res, T payload, Func<T, Entity> mediaFormat)
    {
      Entity entity;

      var req = res.Context.Request;
      var mediaType = req.Headers[HeaderNames.Accept] ?? MimeTypeNames.JsonSiren;

      var isHypermedia = mediaType.Contains("siren");
      if (isHypermedia)
      {
        entity = mediaFormat?.Invoke(payload) ?? CreateEntity(payload);
      }
      else
      {
        var payloadInstance = Payload.FromGraph(payload);
        entity = payloadInstance.ToEntity();
      }

      await WriteEntityAsync(res, entity);
    }

    public static async Task SendEntityAsync(this IResponse res, Entity entity)
    {
      await WriteEntityAsync(res, entity);
    }

    private static async Task WriteEntityAsync(IResponse res, Entity entity, HttpStatusCode status = HttpStatusCode.OK)
    {
      var req = res.Context.Request;
      var accept = req.Headers[HeaderNames.Accept] ?? MimeTypeNames.JsonSiren;
      var contentType = MediaSerializer.ParseFormat(accept);

      var hasSelf = entity.Links?.Any(x => x.Rel?.Any(y => y == RelNames.Self) == true) == true;
      if (!hasSelf)
      {
        entity.WithLinks().Add(new Link { Rel = RelNames.Self, Href = req.RequestUri });
      }

      res.Status = status;
      res.Headers[HeaderNames.ContentType] = $"{contentType}; charset=UTF-8";

      var serializer = new MediaSerializer(contentType);
      serializer.Serialize(entity, res.Body);

      await Task.Yield();
    }

    private static Entity CreateEntity(object payload)
    {
      var entity = new Entity();
      entity.Properties = new PropertyMap();
      foreach (var property in payload._GetPropertyNames())
      {
        var value = payload._Get(property);
        entity.Properties[property] = value;
      }
      return entity;
    }
  }
}
