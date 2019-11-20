using Innkeeper.Host;
using Paper.Design;
using System;
using System.Collections.Generic;
using System.Text;
using Toolset;
using Toolset.Reflection;

namespace Paper.Rendering
{
  public class PaperDescriptorFactory
  {
    private readonly IWebApp webApp;

    public PaperDescriptorFactory(IWebApp webApp)
    {
      this.webApp = webApp;
    }

    public PaperDescriptor CreatePaperDescriptor(Type paperType)
    {
      var attr = paperType._GetAttribute<PaperAttribute>();
      var descriptor = new PaperDescriptor
      {
        Catalog = attr?.Catalog ?? webApp.Name,
        Paper = attr?.Name ?? paperType.Name,
        Title = attr?.Title ?? paperType.Name.ChangeCase(TextCase.ProperCase)
      };
      return descriptor;
    }
  }
}
