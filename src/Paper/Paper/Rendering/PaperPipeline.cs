using Innkeeper.Host;
using Innkeeper.Rest;
using Paper.Media;
using Paper.Media.Design;
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
      var payload = HttpEntity.CreateFromRet(Req.RequestUri, ret);
      await Res.SendEntityAsync(payload);
    }

    [Get("/")]
    async Task GetCatalog()
    {
      PaperPipelineRouter.ExtractTokensFromPath(Req.RequestPath, out string module);

      var payload = new Beans.Catalog();
      payload.Name = module;
      payload.Title = module.ChangeCase(TextCase.ProperCase);
      payload.Path = PaperPipelineRouter.CreatePath(module);

      await Res.SendEntityAsync(payload, FormatPayload);
    }

    [Get("/Papers")]
    async Task GetPapers()
    {
      var module = PaperPipelineRouter.ExtractTokensFromPath(Req.RequestPath);
      var descriptors = paperCatalog.FindPaperDescriptor(module);

      var payload = (
        from descriptor in descriptors
        select new Beans.Paper
        {
          Catalog = descriptor.Module,
          Name = descriptor.Schema,
          Title = descriptor.Schema.ChangeCase(TextCase.ProperCase),
          Path = PaperPipelineRouter.CreatePath(descriptor.Module, descriptor.Schema)
        }
      ).ToArray();

      await Res.SendEntityAsync(payload, FormatPayload);
    }

    [Get("/Papers/{paper}")]
    async Task GetPaper(string paper)
    {
      PaperPipelineRouter.ExtractTokensFromPath(Req.RequestPath, out string module);

      var descriptor = paperCatalog.FindPaperDescriptor(module, paper);
      var payload = new Beans.Paper
      {
        Catalog = descriptor.Module,
        Name = descriptor.Schema,
        Title = descriptor.Schema.ChangeCase(TextCase.ProperCase),
        Path = PaperPipelineRouter.CreatePath(descriptor.Module, descriptor.Schema)
      };

      await Res.SendEntityAsync(payload, FormatPayload);
    }

    [Get("/Papers/{paper}/Actions")]
    async Task GetActions(string paper)
    {
      PaperPipelineRouter.ExtractTokensFromPath(Req.RequestPath, out string module);

      var descriptors = paperCatalog.FindPaperDescriptor(module);

      var payload = new Beans.Action[0];

      await Res.SendEntityAsync(payload, FormatPayload);
    }

    [Get("/Papers/{paper}/Actions/{action}")]
    async Task GetActions(string paper, string action)
    {
      PaperPipelineRouter.ExtractTokensFromPath(Req.RequestPath, out string module);

      var descriptors = paperCatalog.FindPaperDescriptor(module);

      var payload = new Beans.Action();

      await Res.SendEntityAsync(payload, FormatPayload);
    }

    private Entity FormatPayload(object payload)
    {
      var type = payload.GetType();
      var entity = new Entity();
      if (payload is ICollection list)
      {
        entity.WithClass().Add("records");

        var headers = type.GetProperties().Select(x => (CaseVariantString)x.Name).ToArray();
        entity.WithProperties().Add("__meta", new
        {
          records = new
          {
            headers = headers.ToArray()
          }
        });

        entity.Entities = new EntityCollection();
        foreach (var item in list)
        {
          var child = FormatPayload(item);
          entity.Entities.Add(child);
        }
      }
      else
      {
        entity.WithClass().Add(type.FullName);
        entity.WithClass().Add("record");
        var headers = type.GetProperties().Select(x => (CaseVariantString)x.Name).ToArray();
        entity.WithProperties().Add("__meta", new
        {
          record = new
          {
            headers = headers.ToArray()
          }
        });
        entity.WithProperties().AddProperties(payload);

        if (payload is Beans.Catalog)
        {
          //entity.WithLinks().Add(new Link { Href = Req.RequestUri.Append("/Papers") });
        }
      }
      return entity;
    }
  }
}
