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
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Toolset;
using Toolset.Collections;
using Toolset.Reflection;
using Toolset.Xml;

namespace Paper.Rendering.Design
{
  class PaperBlueprint : IPaperBlueprint
  {
    public PaperBlueprint(PaperInfo info)
    {
      this.Info = info;
    }

    public PaperInfo Info { get; }

    public List<Action<IPaperContext>> GetStatements { get; set; }

    public List<Action<IPaperContext>> PostStatements { get; set; }

    public async Task RenderPaperAsync(IPaperContext ctx, TextWriter writer)
    {
      switch (ctx.Verb)
      {
        case VerbNames.Get:
          {
            if (GetStatements != null)
            {
              await RenderPaperAsync(ctx, writer, GetStatements);
              return;
            }
            break;
          }
        case VerbNames.Post:
          {
            if (PostStatements != null)
            {
              await RenderPaperAsync(ctx, writer, PostStatements);
              return;
            }
            break;
          }
      }
      throw new HttpException(HttpStatusCode.MethodNotAllowed);
    }

    private async Task RenderPaperAsync(IPaperContext ctx, TextWriter writer, List<Action<IPaperContext>> statements)
    {
      foreach (var statement in statements)
      {
        statement.Invoke(ctx);
      }
      await Task.Yield();
    }
  }
}