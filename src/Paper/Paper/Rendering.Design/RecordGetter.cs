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
  class RecordGetter<TValue> : IRecordObjectGetter<TValue>
  {
    private readonly string recordKey;
    private readonly string nodeKey;

    public RecordGetter(string recordKey, string nodeKey)
    {
      this.recordKey = recordKey;
      this.nodeKey = nodeKey;
    }

    public TValue GetRecord(IPaperContext ctx)
    {
      try
      {
        return (TValue)ctx.Cache[recordKey];
      }
      catch (Exception ex)
      {
        throw new InvalidCastException($"O valor obtido não corresponde ao tipo esperado: {typeof(TValue).FullName}", ex);
      }
    }

    public INode<TValue> GetNode(IPaperContext ctx)
    {
      try
      {
        return (INode<TValue>)ctx.Cache[nodeKey];
      }
      catch (Exception ex)
      {
        throw new InvalidCastException($"O valor obtido não corresponde ao tipo esperado: {typeof(TValue).FullName}", ex);
      }
    }
  }
}