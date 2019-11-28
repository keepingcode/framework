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
  class ResultGetter<TValue> : IResultGetter<TValue>
  {
    private readonly string key;

    public ResultGetter(string key)
    {
      this.key = key;
    }

    public TValue Get(IPaperContext ctx)
    {
      try
      {
        return (TValue)ctx.Cache[key];
      }
      catch (Exception ex)
      {
        throw new InvalidCastException($"O valor obtido não corresponde ao tipo esperado: {typeof(TValue).FullName}", ex);
      }
    }
  }
}