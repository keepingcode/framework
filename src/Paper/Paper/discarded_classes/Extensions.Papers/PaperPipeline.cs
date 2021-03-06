﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Paper.Commons;
using Paper.Rendering;
using Paper.Media;
using Paper.Media.Data;
using Paper.Media.Design;
using Toolset;
using Toolset.Collections;
using Toolset.Net;
using Toolset.Reflection;
using static Toolset.Ret;

namespace Paper.Extensions.Papers
{
  [Expose]
  public class PaperPipeline : IPipeline
  {
    private readonly IPaperCatalog paperCatalog;
    private readonly IObjectFactory objectFactory;

    public string Route { get; } = "";

    public PaperPipeline(IObjectFactory objectFactory, IPaperCatalog paperCatalog)
    {
      this.objectFactory = objectFactory;
      this.paperCatalog = paperCatalog;
    }

    public async Task RenderAsync(Request req, Response res, NextAsync next)
    {
      var path = req.Path.Substring(Route.Length);

      // Destacando a ação de uma URI, como em /My/Path/-MyAction
      var tokens = path.Split("/-");
      var paperPath = tokens.First();
      var paperAction = tokens.Skip(1).FirstOrDefault() ?? "Index";

      var paper = paperCatalog.FindExact(paperPath).FirstOrDefault();
      if (paper == null)
      {
        await next.Invoke();
        return;
      }

      var hasAction = paper.GetMethod(paperAction) != null;
      if (!hasAction)
      {
        await next.Invoke();
        return;
      }

      var args = new Args();
      args.AddMany(req.QueryArgs);
      args.AddMany(Args.ParsePathArgs(path, paper.PathTemplate));

      var context = new PaperContext();
      context.Paper = paper;
      context.Path = paperPath;
      context.Action = paperAction;
      context.Args = args;
      context.Request = req;
      context.Response = res;

      var caller = objectFactory.CreateObject<PaperCaller>();
      var renderer = objectFactory.CreateObject<PaperRenderer>();

      Ret<Result> result = await caller.CallAsync(context);
      if (result.Status.CodeClass != HttpStatusClass.Success)
      {
        var entity = HttpEntity.CreateFromRet(req.RequestUri, result);
        await SendAsync(res, result, entity);
        return;
      }
      
      Ret<Entity> media = await renderer.RenderAsync(context, result);
      if (!media.Ok)
      {
        var entity = HttpEntity.CreateFromRet(req.RequestUri, result);
        await SendAsync(res, media, entity);
        return;
      }

      await SendAsync(res, media, media.Value);
    }

    private async Task SendAsync(Response res, Ret ret, Entity entity)
    {
      res.Status = ret.Status.Code;
      foreach (var entry in ret.Status.Headers)
      {
        res.Headers[entry.Key] = entry.Value;
      }
      await res.WriteEntityAsync(entity);
    }
  }
}