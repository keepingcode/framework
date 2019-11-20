using Innkeeper.Host;
using Paper.Design;
using System;
using System.Collections.Generic;
using System.Text;
using Toolset.Reflection;

namespace Paper.Rendering
{
  public class PaperDescriptor : IPaperDescriptor
  {
    public string Catalog { get; set; }

    public string Paper { get; set; }

    public string Title { get; set; }

    public Type PaperType { get; set; }
  }
}
