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

namespace Paper.Rendering.Design
{
  public class PaperBuilder<TTarget> : IPaperBuilder<TTarget>
  {
    private readonly PaperBlueprint blueprint;
    private readonly Func<IPaperContext, TTarget> targetFactory;

    public PaperBuilder(PaperInfo info, Func<IPaperContext, TTarget> targetFactory)
    {
      this.blueprint = new PaperBlueprint(info);
      this.targetFactory = targetFactory;
    }

    public IStepBuilder<TTarget, TResult> Get<TResult>()
    {
      return Get((ctx, target) => default(TResult));
    }

    public IStepBuilder<TTarget, TResult> Get<TResult>(Func<IPaperContext, TTarget, TResult> resultFactory)
    {
      if (blueprint.GetStatements != null)
        throw new InvalidOperationException("Já existe uma sintaxe declarada para processamento de requisições do tipo GET.");

      blueprint.GetStatements = new List<Action<IPaperContext>>();
      return new StepBuilder<TTarget, TResult>(targetFactory, resultFactory, blueprint.GetStatements);
    }

    public IStepBuilder<TTarget, TResult> Post<TResult>(Func<IPaperContext, TTarget, FormData, TResult> resultFactory)
    {
      if (blueprint.PostStatements != null)
        throw new InvalidOperationException("Já existe uma sintaxe declarada para processamento de requisições do tipo POST.");

      blueprint.PostStatements = new List<Action<IPaperContext>>();
      return new StepBuilder<TTarget, TResult>(targetFactory, resultFactory, blueprint.PostStatements);
    }

    public IPaperBlueprint BuildPaper()
    {
      return blueprint;
    }
  }
}