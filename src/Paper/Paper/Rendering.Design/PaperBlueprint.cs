using Innkeeper.Host;
using Innkeeper.Rest;
using Paper.Media;
using Paper.Media.Design;
using Paper.Media.Serialization;
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
  internal class PaperBlueprint : IPaperBlueprint
  {
    public PaperBlueprint(PaperInfo info)
    {
      this.Info = info;
    }

    public PaperInfo Info { get; }

    public Func<IPaperContext, IMediaObject> GetStatement { get; set; }

    public Func<IPaperContext, IMediaObject> PostStatement { get; set; }

    public void RenderPaper(IPaperContext ctx)
    {
      Func<IPaperContext, IMediaObject> statement = null;

      switch (ctx.Verb)
      {
        case VerbNames.Get:
          statement = GetStatement;
          break;
        case VerbNames.Post:
          statement = PostStatement;
          break;
      }

      if (statement == null)
        throw new HttpException(HttpStatusCode.MethodNotAllowed);

      var result = statement.Invoke(ctx);
      ctx.OutgoingData.WriteMediaObject(result);
    }
  }
}