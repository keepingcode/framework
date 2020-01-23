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
  class CatalogPipeline : RestPipeline
  {
    private readonly string defaultCatalog;
    private readonly IPaperCatalog paperCatalog;
    private readonly IObjectFactory objectFactory;

    public CatalogPipeline(IWebApp webApp, IPaperCatalog paperCatalog, IObjectFactory objectFactory)
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

    [Get("/")]
    async Task GetCatalog()
    {
      var catalog = new Beans.Catalog();
      catalog.Name = defaultCatalog;
      catalog.Path = $"/Paper/Api/Catalogs/{defaultCatalog}";

      await Res.SendEntityAsync(catalog, payload =>
      {
        var entity = FormatPayload(payload);
        
        entity.Add(new Link
        {
          Href = new Url(Req.RequestUri).Combine(".."),
          Title = "Catálogos"
        });

        entity.Add(new Link
        {
          Href = new Url(Req.RequestUri).Append("Papers"),
          Title = "Papers"
        });
        
        return entity;
      });
    }

    [Get("/Papers")]
    async Task GetPapers()
    {
      var descriptors = paperCatalog.FindPaperDescriptor(defaultCatalog);

      var papers = (
        from descriptor in descriptors
        select new Beans.Paper
        {
          Catalog = descriptor.Catalog,
          Name = descriptor.Paper,
          Path = $"/Paper/Api/Catalogs/{descriptor.Catalog}/Papers/{descriptor.Paper}"
        }
      ).ToArray();

      await Res.SendEntityAsync(papers, payload =>
      {
        var entity = FormatPayload(payload);

        entity.Add(new Link
        {
          Href = new Url(Req.RequestUri).Combine(".."),
          Title = "Catálogo"
        });

        foreach (var descriptor in descriptors)
        {
          var paper = descriptor.Paper;
          var title = descriptor.Title;

          entity.Add(new Link
          {
            Href = new Url(Req.RequestUri).Append(paper),
            Title = title
          });
        }
        return entity;
      });
    }

    [Get("/Papers/{paper}")]
    async Task GetPaper(string paper)
    {
      var descriptor = paperCatalog.FindPaperDescriptor(defaultCatalog, paper);
      if (descriptor == null)
      {
        var ret = Ret.Fail(HttpStatusCode.NotFound, $"O obteto não existe: {defaultCatalog}/{paper}");
        await Res.SendEntityStatusAsync(ret);
        return;
      }

      var instance = new Beans.Paper
      {
        Catalog = descriptor.Catalog,
        Name = descriptor.Paper,
        Title = descriptor.Title,
        Path = $"/Paper/Api/Catalogs/{descriptor.Catalog}/Papers/{descriptor.Paper}"
      };

      await Res.SendEntityAsync(instance, payload =>
      {
        var entity = FormatPayload(payload);

        entity.Add(new Link
        {
          Href = new Url(Req.RequestUri).Combine("../.."),
          Title = "Catálogo"
        });

        entity.Add(new Link
        {
          Href = new Url(Req.RequestUri).Combine(".."),
          Title = "Papers"
        });

        entity.Add(new Link
        {
          Href = new Url(Req.RequestUri).Append("Actions"),
          Title = "Ações"
        });

        return entity;
      });
    }

    private Entity FormatPayload(object payload)
    {
      var type = payload.GetType();
      var entity = new Entity();
      if (payload is ICollection list)
      {
        entity.Add(Class.Records);

        var itemType = TypeOf.CollectionElement(type);
        var headers = itemType.GetProperties().Select(x => (VName)x.Name).ToArray();

        entity.Add(new Property("__meta", new
        {
          records = new
          {
            headers = headers.ToArray()
          }
        }));

        foreach (var item in list)
        {
          var child = FormatPayload(item);
          entity.Add(child);
        }
      }
      else
      {
        entity.Add(new Class(type.FullName));
        entity.Add(Class.Record);

        var headers = type.GetProperties().Select(x => (VName)x.Name).ToArray();
        var payloadProperties = Value.CreateObject(payload);

        entity.Add(new Property("__meta", new
        {
          record = new
          {
            headers = headers.ToArray()
          }
        }));

        entity.AddMany(payloadProperties);

        if (payload is Beans.Catalog)
        {
          //entity.WithLinks().Add(new Link { Href = Req.RequestUri.Append("/Papers") });
        }
      }
      return entity;
    }
  }
}
