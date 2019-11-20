using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Beans
{
  public class Catalog
  {
    private string _title;

    public string Name { get; internal set; }

    public string Title
    {
      get => _title ?? Name?.ChangeCase(TextCase.ProperCase);
      internal set => _title = value;
    }

    public string Path { get; internal set; }
  }
}
