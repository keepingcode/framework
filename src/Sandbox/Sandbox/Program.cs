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

        var action = factory.CreatePaperBuilder("/MyCatalog/MyPaper/MyAction",
          ctx => new { ArgCount = ctx.Args.Count }
        );

        var invoiceGetter = action.PopulateOne((ctx, target) => new
        {
          InvoiceId = ctx.Args.Get<int>("InvoiceId"),
          Number = 2,
          Series = 3
        });
        var invoiceItemsGetter = action.PopulateMany((ctx, target) => new[]
        {
          new {
            invoiceGetter(ctx).InvoiceId,
            ItemId = 10,
            Name = "Ten"
          },
          new {
            invoiceGetter(ctx).InvoiceId,
            ItemId = 11,
            Name = "Eleven"
          }
        });

        action.Format(invoiceGetter);
        action.Format(invoiceItemsGetter, (ctx, target, all, item, entity) =>
        {
          entity.AddClass($"MyFormattedItem");
        });

        var payloadGetter = action.Restore((ctx, target, payload) => payload);

        action.Act(payloadGetter, (ctx, target, payload) => Console.WriteLine("It`s done!"));
      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }

    #region Types

    interface IPaperContext
    {
      IMap<string, Var> Args { get; }
      IObjectFactory Factory { get; }
    }

    interface IPaperBuilder<T>
    {
      Func<IPaperContext, TValue> PopulateOne<TValue>(Func<IPaperContext, T, TValue> populator);
      Func<IPaperContext, TValue> PopulateMany<TValue>(Func<IPaperContext, T, TValue> populator);

      void Format(Action<IPaperContext, T, Entity> formatter);
      void Format<TValue>(Func<IPaperContext, TValue> getter, Action<IPaperContext, T, TValue, Entity> formatter = null);
      void Format<TValue>(Func<IPaperContext, ICollection<TValue>> getter, Action<IPaperContext, T, ICollection<TValue>, TValue, Entity> formatter = null);

      Func<IPaperContext, TValue> Restore<TValue>(Func<IPaperContext, T, Payload, TValue> restorer);

      void Act<TValue>(Func<IPaperContext, TValue> getter, Action<IPaperContext, T, TValue> action);
    }

    interface IPaperBuilderFactory
    {
      IPaperBuilder<T> CreatePaperBuilder<T>(string name, Func<IPaperContext, T> factory);
    }

    #endregion
  }
}