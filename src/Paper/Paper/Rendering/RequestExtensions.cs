using Innkeeper.Host;
using Paper.Media;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Toolset;
using Toolset.Net;

namespace Paper.Rendering
{
  public static class RequestExtensions
  {
    public static Ret<MediaFormat> GetMediaFormat(this IRequest req)
    {
      var args = req.QueryArgs;

      var accept = req.Headers[HeaderNames.Accept];
      var charset = req.Headers[HeaderNames.AcceptCharset];

      if (args["out"] is Var outVar)
      {
        var format = outVar.RawValue?.ToString() ?? "";

        // exemplos:
        //   text/csv
        //   application/json; charset=UTF-8
        var pattern = @"^([^/=;\s]+)/([^/=;\s]+)(?:;\s*charset=([^/=;\s]+))?$";
        var match = Regex.Match(format, pattern);
        if (match.Success)
          return Ret.Fail(HttpStatusCode.BadRequest, "O valor do parâmetro `out' é inválido.");

        accept = match.Groups[1].Value;
        if (!string.IsNullOrEmpty(match.Groups[2].Value))
        {
          charset = match.Groups[2].Value;
        }
      }

      if (args["charset"] is Var charsetVar)
      {
        var format = charsetVar.RawValue?.ToString() ?? "";

        // exemplos:
        //   UTF-8
        //   Latin1
        //   ISO-8859-15
        var pattern = @"^[^/=;\s]+$";
        var match = Regex.Match(format, pattern);
        if (match.Success)
          return Ret.Fail(HttpStatusCode.BadRequest, "O valor do parâmetro `charset' é inválido.");

        charset = format;
      }

      if (args["encoding"] is Var encodingVar)
      {
        var format = encodingVar.RawValue?.ToString() ?? "";

        // exemplo:
        //   UTF-8
        var pattern = @"^[^/=;\s]+$";
        var match = Regex.Match(format, pattern);
        if (match.Success)
          return Ret.Fail(HttpStatusCode.BadRequest, "O valor do parâmetro `charset' é inválido.");

        charset = format;
      }

      if (args["f"] is Var fVar)
      {
        var format = fVar.RawValue?.ToString() ?? "";

        // pattern:
        //   type.compression
        //   exemplos:
        //     xml
        //     json.gz
        //     csv.gzip
        var pattern = @"^([^/=;\s]+)/([^/=;\s]+)(?:;\s*charset=([^/=;\s]+))?$";
        var match = Regex.Match(format, pattern);
        if (match.Success)
          return Ret.Fail(HttpStatusCode.BadRequest, "O valor do parâmetro `out' é inválido.");
        //[\w\d]+(?:\.([\w\d]+))?
      }

      var mediaFormat = new MediaFormat();

      var acceptType = req.Headers[HeaderNames.Accept];
      if (acceptType?.Contains(MimeTypeNames.JsonSiren) == true)
      {
        mediaFormat.Type = MimeTypeNames.JsonSiren;
      }
      else if (acceptType?.Contains(MimeTypeNames.XmlSiren) == true)
      {
        mediaFormat.Type = MimeTypeNames.XmlSiren;
      }
      else if (acceptType?.Contains(MimeTypeNames.JsonApplication) == true)
      {
        mediaFormat.Type = MimeTypeNames.JsonApplication;
      }
      else if (acceptType?.Contains(MimeTypeNames.JsonText) == true)
      {
        mediaFormat.Type = MimeTypeNames.JsonText;
      }
      else if (acceptType?.Contains(MimeTypeNames.XmlApplication) == true)
      {
        mediaFormat.Type = MimeTypeNames.XmlApplication;
      }
      else if (acceptType?.Contains(MimeTypeNames.XmlText) == true)
      {
        mediaFormat.Type = MimeTypeNames.XmlText;
      }
      else
      {
        mediaFormat.Type = MimeTypeNames.JsonText;
      }

      



      return mediaFormat;
    }
  }
}
