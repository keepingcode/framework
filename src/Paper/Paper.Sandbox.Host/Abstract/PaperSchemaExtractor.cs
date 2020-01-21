using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Paper.Sandbox.Host.Abstract
{
  public class PaperSchemaExtractor
  {
    public PaperSchema ExtractSchema(Type paperType)
    {
      var schema = new PaperSchema();

      var flags = BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance;
      foreach (var method in paperType.GetMethods(flags))
      {
        Debug.WriteLine(method.Name);
      }

      return schema;
    }
  }
}
