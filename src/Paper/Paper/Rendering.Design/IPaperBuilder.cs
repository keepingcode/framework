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
  public interface IPaperBuilder<TTarget>
  {
    IStepBuilder<TTarget, TResult> Get<TResult>();
    IStepBuilder<TTarget, TResult> Get<TResult>(Func<IPaperContext, TTarget, TResult> process);
    IStepBuilder<TTarget, TResult> Post<TResult>(Func<IPaperContext, TTarget, FormData, TResult> process);
    IPaperBlueprint BuildPaper();
  }
}