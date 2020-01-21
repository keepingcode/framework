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
using System.Xml.Serialization;
using Toolset;
using Toolset.Collections;
using Toolset.Reflection;
using Toolset.Xml;

namespace Paper.Rendering.Design
{
  internal class PaperDesigner<THost> : IPaperDesigner<THost>
  {
    private readonly PaperInfo info;
    private readonly Func<IPaperContext, THost> hostFactory;

    private Func<Func<IPaperContext, IEntity>> getStatementFactory;
    private Func<Func<IPaperContext, IEntity>> postStatementFactory;

    public PaperDesigner(PaperInfo info, Func<IPaperContext, THost> hostFactory)
    {
      this.info = info;
      this.hostFactory = hostFactory;
    }

    public IStatementDesigner<THost, TData> Get<TData>()
    {
      return Get((ctx, target) => default(TData));
    }

    public IStatementDesigner<THost, TData> Get<TData>(Func<IPaperContext, THost, TData> dataFactory)
    {
      if (getStatementFactory != null)
        throw new InvalidOperationException("Já existe uma sintaxe declarada para processamento de requisições do tipo GET.");

      var builder = new StatementDesigner<THost, TData>(hostFactory, 
        (ctx, host, formData) => dataFactory.Invoke(ctx, host)
      );

      getStatementFactory = () => builder.DesignStatement();

      return builder;
    }

    public IStatementDesigner<THost, TData> Post<TData>(Func<IPaperContext, THost, IEntity, TData> dataFactory)
    {
      if (postStatementFactory != null)
        throw new InvalidOperationException("Já existe uma sintaxe declarada para processamento de requisições do tipo GET.");

      var builder = new StatementDesigner<THost, TData>(hostFactory, dataFactory);

      postStatementFactory = () => builder.DesignStatement();

      return builder;
    }

    public IPaperBlueprint DesignPaper()
    {
      var blueprint = new PaperBlueprint(info);
      blueprint.GetStatement = getStatementFactory?.Invoke() ?? MethodNotAllowed;
      blueprint.PostStatement = getStatementFactory?.Invoke() ?? MethodNotAllowed;
      return blueprint;
    }

    private IEntity MethodNotAllowed(IPaperContext ctx)
    {
      throw new HttpException(HttpStatusCode.MethodNotAllowed);
    }
  }
}