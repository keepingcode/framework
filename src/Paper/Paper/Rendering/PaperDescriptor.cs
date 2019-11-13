using Paper.Design;
using System;
using System.Collections.Generic;
using System.Text;
using Toolset.Reflection;

namespace Paper.Rendering
{
  public class PaperDescriptor : IPaperDescriptor
  {
    public string Module { get; set; }

    public string Schema { get; set; }

    public Type PaperType { get; set; }

    public static string IdentifyModule(Type paperType)
    {
      var attr = paperType._GetAttribute<PaperAttribute>();
      var module = attr?.Module ?? paperType.Namespace;
      return module;
    }

    public static string IdentifySchema(Type paperType)
    {
      var attr = paperType._GetAttribute<PaperAttribute>();
      var schema = attr?.Schema ?? paperType.Name;
      return schema;
    }
  }
}
