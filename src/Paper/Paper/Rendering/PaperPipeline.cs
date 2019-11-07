using Innkeeper.Host;
using Innkeeper.Rest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Toolset;
using Toolset.Net;

namespace Paper.Rendering
{
  class PaperPipeline : RestPipeline
  {
    private readonly IPaperCatalog paperCatalog;
    private readonly IObjectFactory objectFactory;

    public PaperPipeline(IPaperCatalog paperCatalog, IObjectFactory objectFactory)
    {
      this.paperCatalog = paperCatalog;
      this.objectFactory = objectFactory;
    }

    protected async override Task SendFaultAsync(Exception exception)
    {
      try
      {
        var payload = Ret.Fail(exception);
        await Res.SendMediaAsync(payload, mediaFormat: null);
      }
      catch
      {
        await base.SendFaultAsync(exception);
      }
    }

    [Invoke(Verb.All, "/{*path}")]
    async Task IgnorePath(string path)
    {
      Res.Headers[HeaderNames.ContentType] = "";
      await Res.SendAsync($"Este caminho não corresponde a qualquer método da API: {path}");
    }

    [Get("/Papers")]
    async Task GetPapers()
    {
      await Res.SendAsync($"/Papers");
    }

    [Get("/Papers/{paper}")]
    async Task GetPaper(string paper)
    {
      await Res.SendAsync($"/Papers/{paper}");
    }

    [Get("/Papers/{paper}/Actions")]
    async Task GetActions(string paper)
    {
      await Res.SendAsync($"/Papers/{paper}/Actions");
    }

    [Get("/Papers/{paper}/Actions/{action}")]
    async Task GetActions(string paper, string action)
    {
      await Res.SendAsync($"/Papers/{paper}/Actions/{action}");
    }
  }
}
