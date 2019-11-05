using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Toolset.Net;
using Toolset.Xml;

namespace Innkeeper.Host
{
  public static class RequestExtensions
  {
    public async static Task SendAsync(this IResponse res, string data)
    {
      if (res.Headers[HeaderNames.ContentType] == null)
      {
        res.Headers[HeaderNames.ContentType] = "text/plain; charset=UTF-8";
      }

      var writer = new StreamWriter(res.Body);
      await writer.WriteAsync(data);
      await writer.FlushAsync();
    }

    public async static Task SendAsync(this IResponse res, XElement xml)
    {
      if (res.Headers[HeaderNames.ContentType] == null)
      {
        res.Headers[HeaderNames.ContentType] = "application/xml; charset=UTF-8";
      }

      var data = xml.ToXmlString();
      var writer = new StreamWriter(res.Body);
      await writer.WriteAsync(data);
      await writer.FlushAsync();
    }
  }
}
