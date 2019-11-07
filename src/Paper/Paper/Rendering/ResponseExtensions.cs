using Innkeeper.Host;
using Paper.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Toolset.Net;

namespace Paper.Rendering
{
  public static class ResponseExtensions
  {
    public static async Task SendMediaAsync(this IResponse res, object payload)
    {
      await SendMediaAsync(res, payload, mediaFormat: null);
    }

    public static async Task SendMediaAsync<T>(this IResponse res, T payload, Func<T, Entity> mediaFormat)
    {
      //var req = res.GetContext().Request;
      //var args = req.QueryArgs;
      //
      //
      //
      //
      //var format =
      //  args["out"]
      //  ?? args["f"]
      //  ?? req.Headers[HeaderNames.ContentType];
      //
      //
      //
      //
      //var isJson = req.Headers[HeaderNames.ContentType];
      //
      //await writer.FlushAsync();
      await Task.Yield();
    }
  }
}
