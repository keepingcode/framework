using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Innkeeper.Host;
using Paper.Media;
using Paper.Media.Serialization;

namespace Paper.Host
{
  public static class RequestExtensions
  {
    public static async Task<Entity> ReadEntityAsync(this IRequest request)
    {
      var contentHeader = new ContentHeader(request.Headers);
      var mimeType = contentHeader.Type;
      var encoding = contentHeader.Encoding;

      var serializer = new MediaSerializer(mimeType);
      var entity = serializer.Deserialize(request.Body, encoding);
      return await Task.FromResult(entity);
    }
  }
}