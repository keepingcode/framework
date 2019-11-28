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
  class RecordCollectionGetter<TValue> : IRecordCollectionGetter<TValue>
  {
    private string recordsKey;
    private string nodesKey;

    public RecordCollectionGetter(string recordsKey, string nodesKey)
    {
      this.recordsKey = recordsKey;
      this.nodesKey = nodesKey;
    }

    public ICollection<TValue> GetRecords(IPaperContext ctx)
    {
      try
      {
        return (ICollection<TValue>)ctx.Cache[recordsKey];
      }
      catch (Exception ex)
      {
        throw new InvalidCastException($"O valor obtido não corresponde ao tipo esperado: {typeof(TValue).FullName}", ex);
      }
    }

    public ICollection<INode<TValue>> GetNodes(IPaperContext ctx)
    {
      try
      {
        return (ICollection<INode<TValue>>)ctx.Cache[nodesKey];
      }
      catch (Exception ex)
      {
        throw new InvalidCastException($"O valor obtido não corresponde ao tipo esperado: {typeof(TValue).FullName}", ex);
      }
    }
  }
}