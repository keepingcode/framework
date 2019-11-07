using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Toolset;
using Toolset.Net;

namespace Innkeeper.Host
{
  public class PipelineInvoker
  {
    public async Task InvokeAsync(IRequestContext ctx, IObjectFactory objectFactory, NextAsync next)
    {
      var req = ctx.Request;
      var res = ctx.Response;

      try
      {
        var router = objectFactory.GetInstance<IRouter>();
        var routes = router.Find(req.RequestPath);
        var iterator = routes.GetEnumerator();

        NextAsync chain = null;
        chain = new NextAsync(async () =>
        {
          if (iterator.MoveNext())
          {
            var route = iterator.Current;
            var pipeline = route.CreatePipeline(objectFactory);
            await pipeline.RunAsync(ctx, chain);
          }
          else
          {
            await next();
          }
        });

        await chain.Invoke();
      }
      catch (Exception ex)
      {
        ex.Trace();

        var status = HttpStatusCode.InternalServerError;

        res.Status = status;
        res.Headers[HeaderNames.ContentType] = "text/plain; charset=UTF-8";

        var ln = Environment.NewLine;
        await res.SendAsync(
          $"{(int)status} - {status.ToString().ChangeCase(TextCase.ProperCase)}{ln}{ex.Message}{ln}Caused by:{ln}{ex.GetStackTrace()}"
        );
      }
    }

    private void ConfigureRequest(IRequest req)
    {
      var args = req.QueryArgs;

      string contentType = null;      // Mime type mais charset, ex: "text/json; charset=UTF-9"
      string contentEncoding = null;  // Algoritmo de compressão.

      contentType = req.Headers[HeaderNames.ContentType] ?? "";
      contentEncoding = req.Headers[HeaderNames.ContentEncoding] ?? "";

      //
      // Aplicando modificadores de URI
      //

      if (args["in"] is Var inVar)
      {
        var format = inVar.RawValue?.ToString() ?? "";

        // exemplos:
        //   text/csv
        //   application/json; charset=UTF-8
        var pattern = @"^([^/=;\s]+)/([^/=;\s]+)(?:;\s*charset=([^/=;\s]+))?$";
        var match = Regex.Match(format, pattern);
        if (match.Success)
        {
          contentType = match.Groups[1].Value;
          if (!string.IsNullOrEmpty(match.Groups[2].Value))
          {
            contentType += $"; charset={match.Groups[2].Value}";
          }
        }

        args["in"] = null;
      }

      //
      // Modificando parâmetros de resposta
      //

      req.Headers[HeaderNames.ContentType] = contentType;
      req.Headers[HeaderNames.ContentEncoding] = contentEncoding;

      // Formato de compressao.
      // Por enquanto apenas "gzip" é suportado.
      // Referencia:
      // - https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept-Encoding

      if (contentEncoding?.Contains("gzip") == true)
      {
        req.Headers[HeaderNames.ContentEncoding] = "gzip";
        req.SetBody(body => new GZipStream(body, CompressionMode.Decompress));
      }
    }

    private void ExpandContentType(string contentType, ref string mimeType, ref string contentCharset)
    {
      // Expande um conteúdo na forma:
      //   text/plain; charset=UTF-8
      // Produzindho:
      //   mimeType=text/plain
      //   contentCharset=UTF-8

      string[] tokens;

      tokens = contentType?.Split(':');
      mimeType = tokens?.First() ?? mimeType;

      tokens = tokens?.Skip(1).LastOrDefault()?.Split('=');
      contentCharset = tokens?.LastOrDefault() ?? contentCharset;
    }

    private void ConfigureResponse(IResponse res)
    {
      var req = res.GetContext().Request;
      var args = req.QueryArgs;

      string acceptType = null;      // Mime type
      string acceptCharset = null;   // Codificação do text, como: UTF-8, Latin1, etc.
      string acceptEncoding = null;  // Algoritmo de compressão.

      acceptType = req.Headers[HeaderNames.Accept] ?? MimeTypeNames.JsonApplication;
      acceptCharset = req.Headers[HeaderNames.AcceptCharset] ?? "UTF-8";
      acceptEncoding = req.Headers[HeaderNames.AcceptEncoding] ?? "";

      //
      // Aplicando modificadores de URI
      //
      if (args["out"] is Var outVar)
      {
        var format = outVar.RawValue?.ToString() ?? "";

        // exemplos:
        //   text/csv
        //   application/json; charset=UTF-8
        var pattern = @"^([^/=;\s]+)/([^/=;\s]+)(?:;\s*charset=([^/=;\s]+))?$";
        var match = Regex.Match(format, pattern);
        if (match.Success)
        {
          acceptType = match.Groups[1].Value;
          if (!string.IsNullOrEmpty(match.Groups[2].Value))
          {
            acceptCharset = match.Groups[2].Value;
          }
        }

        args["out"] = null;
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
        {
          acceptCharset = format;
        }

        args["charset"] = null;
      }

      if (args["encoding"] is Var encodingVar)
      {
        var format = encodingVar.RawValue?.ToString() ?? "";

        // exemplo:
        //   UTF-8
        var pattern = @"^[^/=;\s]+$";
        var match = Regex.Match(format, pattern);
        if (match.Success)
        {
          acceptEncoding = format;
        }

        args["encoding"] = null;
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
        {
          var type = match.Groups[1].Value;
          var compression = match.Groups[2].Value;

          switch (type)
          {
            case "json+siren":
              acceptType = MimeTypeNames.JsonSiren;
              break;
            case "json":
              acceptType = MimeTypeNames.JsonApplication;
              break;
            case "xml+siren":
              acceptType = MimeTypeNames.XmlSiren;
              break;
            case "xml":
              acceptType = MimeTypeNames.XmlApplication;
              break;
            case "csv":
              acceptType = MimeTypeNames.Csv;
              break;
            case "xls":
            case "xlsx":
            case "excel":
              acceptType = MimeTypeNames.Excel;
              break;
            case "binary":
            case "bin":
              acceptType = MimeTypeNames.OctetStream;
              break;
            case "plain":
              acceptType = MimeTypeNames.PlainText;
              break;
          }

          if (compression == "gz" || compression == "gzip")
          {
            acceptEncoding = "gzip";
          }
        }

        // "f" é um parâmetro especial que pode ser tratado pelo pipeline e por isso não é apagado.
        // args["f"] = null;
      }

      //
      // Modificando parâmetros de resposta
      //

      req.Headers[HeaderNames.Accept] = acceptType;
      req.Headers[HeaderNames.AcceptCharset] = acceptCharset;
      req.Headers[HeaderNames.AcceptEncoding] = acceptEncoding;

      // Formato de compressao.
      // Por enquanto apenas "gzip" é suportado.
      // Referencia:
      // - https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept-Encoding

      if (acceptEncoding?.Contains("gzip") == true)
      {
        res.Headers[HeaderNames.ContentEncoding] = "gzip";
        res.SetBody(body => new GZipStream(body, CompressionMode.Compress));
      }
    }
  }
}