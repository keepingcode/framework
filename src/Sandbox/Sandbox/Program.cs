using Paper.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Toolset;
using Toolset.Collections;

namespace Sandbox
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      try
      {
        var entity = new Entity();
        entity.Title = "Teste";
        entity.Properties = new NodeCollection<Property>();
        entity.Properties.Add(new Property());
        entity.Properties[0].Name = "Id";
        entity.Properties[0].Value = Value.Get(10);
        entity.Properties.Add(new Property());
        entity.Properties[1].Name = "Name";
        entity.Properties[1].Value = new ValueCollection
        {
          Value.Get("Talz"),
          (CaseVariantText)"Telz",
          Value.GetArray(1, true, "3", null, DateTime.Now)
        };

        foreach (var node in entity.DescendantNodesAndSelf())
        {
          Debug.WriteLine($"{node?.GetType().Name}");
        }
      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }
  }
}