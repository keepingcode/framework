using Innkeeper.Host;
using Innkeeper.Rest;
using Paper.Media;
using Paper.Media.Design;
using Paper.Media.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Toolset;
using Toolset.Net;
using Toolset.Reflection;

namespace Paper.Rendering
{
  class PaperPipeline : RestPipeline
  {
    private readonly string defaultCatalog;
    private readonly IPaperCatalog paperCatalog;
    private readonly IObjectFactory objectFactory;

    public PaperPipeline(IWebApp webApp, IPaperCatalog paperCatalog, IObjectFactory objectFactory)
    {
      this.defaultCatalog = webApp.Name;
      this.paperCatalog = paperCatalog;
      this.objectFactory = objectFactory;
    }

    protected async override Task SendFaultAsync(Exception exception)
    {
      try
      {
        var payload = Ret.Fail(exception);
        await Res.SendEntityAsync(payload, mediaFormat: null);
      }
      catch
      {
        await base.SendFaultAsync(exception);
      }
    }

    [Invoke(Verb.All, "/{*path}")]
    async Task NotFound(string path)
    {
      var ret = Ret.Fail(HttpStatusCode.NotFound, $"Este caminho não corresponde a qualquer método da API: {path}");
      await Res.SendEntityStatusAsync(ret);
    }

    [Get("/{paper}")]
    async Task GetPaper(string paper)
    {
      var descriptor = paperCatalog.FindPaperDescriptor(defaultCatalog, paper);
      if (descriptor == null)
      {
        var ret = Ret.Fail(HttpStatusCode.NotFound, $"O objeto não existe: {defaultCatalog}/{paper}");
        await Res.SendEntityStatusAsync(ret);
        return;
      }

      object output;

      var instance = objectFactory.CreateObject(descriptor.PaperType);
      if (instance._HasMethod("Render"))
      {
        output = objectFactory.Call(instance, "Render", null);
      }
      else
      {
        throw new HttpException($"Não há um algoritmo de renderização conhecido para objeto do tipo ${descriptor.PaperType.FullName}.");
      }

      if (output is Stream stream)
      {
        await stream.CopyToAsync(Res.Body);
        await Res.Body.FlushAsync();
      }
      else if (output is StreamReader reader)
      {
        var encoding = Encoding.UTF8;
        var writer = new StreamWriter(Res.Body, encoding);
        await reader.CopyToAsync(writer);
        await writer.FlushAsync();
        await Res.Body.FlushAsync();
      }
      else if (output is Entity entity)
      {
        var encoding = Encoding.UTF8;
        var mimeType = "json";
        var serializer = new MediaSerializer(mimeType);
        serializer.Serialize(entity, Res.Body, encoding);
        await Res.Body.FlushAsync();
      }
      else
      {
        throw new HttpException($"Não há suporte para renderização de dados do tipo {output.GetType().FullName}");
      }
    }

    [Get("/{paper}/{action}")]
    async Task GetAction(string paper, string action)
    {
      var ret = Ret.Fail(HttpStatusCode.NotImplemented, $"Ainda náo implementado: {defaultCatalog}/{paper}/{action}");
      await Res.SendEntityStatusAsync(ret);
    }
  }
}
