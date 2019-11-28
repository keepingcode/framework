using Innkeeper.Host;
using Innkeeper.Rest;
using Paper.Media;
using Paper.Media.Design;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Toolset;
using Toolset.Collections;
using Toolset.Reflection;
using Toolset.Xml;

namespace Sandbox
{
  public static class Program
  {
    public static void Main(string[] commandArgs)
    {
      try
      {
        IPaperBuilderFactory factory = null;

        var builder = factory.CreatePaperBuilder(info: null,
          factory: ctx => new { ArgCount = ctx.Args.Count }
        );

        builder.Info.Catalog = "MyCatalog";
        builder.Info.Name = "MyPaper";
        builder.Info.Action = "MyAction";
        builder.Info.Description = "My Paper Action";
        builder.Info.Title = "My action performed over my paper.";

        {
          var resultGetter = builder.Get((ctx, target) => target);

          var recordGetter = builder.PopulateRecord(resultGetter, (ctx, target, result) => new
          {
            InvoiceId = ctx.Args.Get<int>("InvoiceId"),
            Number = 2,
            Series = 3
          });
          var recordsGetter = builder.PopulateRecords(resultGetter, (ctx, target, result) => new[]
          {
            new {
              recordGetter.Get(ctx, target).InvoiceId,
              ItemId = 10,
              Name = "Ten"
            },
            new {
              recordGetter.Get(ctx, target).InvoiceId,
              ItemId = 11,
              Name = "Eleven"
            }
          });

          builder.FormatEntity(recordGetter, (ctx, target, item, entity) =>
            entity.AddClass($"MyFormattedInvoice")
          );
          builder.FormatEntity(recordsGetter, (ctx, target, all, item, entity) =>
            entity.AddClass($"MyFormattedItem")
          );
        }

        {
          var resultGetter = builder.Act((ctx, target, formData) => new { Message = "Action succeeded" });

          var recordGetter = builder.PopulateRecord(resultGetter, (ctx, target, result) => result);
          var recordsGetter = builder.PopulateRecords(resultGetter, (ctx, target, result) => new[] { result });

          builder.FormatEntity(recordGetter, (ctx, target, item, entity) =>
            entity.AddClass($"MyFormattedResult")
          );
          builder.FormatEntity(recordsGetter, (ctx, target, all, item, entity) =>
            entity.AddClass($"MyFormattedResults")
          );
        }

        {
          IPaperContext ctx = null;
          TextWriter output = Console.Out;

          var paper = builder.BuildPaper();
          paper.RenderPaper(ctx, output);
        }
      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }

    #region Types

    class PaperInfo
    {
      public string Catalog { get; set; }
      public string Name { get; set; }
      public string Action { get; set; }
      public string Title { get; set; }
      public string Description { get; set; }
    }

    interface IPaperRenderer
    {
      void Render(IPaperContext ctx);
    }

    interface IPaperContext
    {
      IMap<string, Var> Args { get; }
      IObjectFactory Factory { get; }
    }

    interface IRecordGetter<TValue>
    {
      TValue Get<TTarget>(IPaperContext ctx, TTarget target);
    }

    interface IRecordCollectionGetter<TValue>
    {
      ICollection<TValue> Get<TTarget>(IPaperContext ctx, TTarget target);
    }

    interface IResultGetter<TResult>
    {
      TResult Get<TTarget>(IPaperContext ctx, TTarget target);
    }

    interface IFormData
    {
    }

    interface IPaperBlueprint
    {
      PaperInfo Info { get; }
      void RenderPaper(IPaperContext ctx, TextWriter output);
    }

    interface IPaperBuilder<TTarget>
    {
      PaperInfo Info { get; set; }

      IRecordGetter<TValue> PopulateRecord<TResult, TValue>(IResultGetter<TResult> resultGetter, Func<IPaperContext, TTarget, TResult, TValue> populator);
      IRecordCollectionGetter<TValue> PopulateRecords<TResult, TValue>(IResultGetter<TResult> resultGetter, Func<IPaperContext, TTarget, TResult, TValue> populator);

      void FormatEntity<TValue>(IRecordGetter<TValue> getter, Action<IPaperContext, TTarget, TValue, Entity> formatter);
      void FormatEntity<TValue>(IRecordCollectionGetter<TValue> getter, Action<IPaperContext, TTarget, ICollection<TValue>, TValue, Entity> formatter);

      IResultGetter<TResult> Get<TResult>(Func<IPaperContext, TTarget, TResult> process);
      IResultGetter<TResult> Act<TResult>(Func<IPaperContext, TTarget, IFormData, TResult> process);

      IPaperBlueprint BuildPaper();
    }

    interface IPaperBuilderFactory
    {
      IPaperBuilder<T> CreatePaperBuilder<T>(PaperInfo info, Func<IPaperContext, T> factory);
    }

    #endregion
  }
}