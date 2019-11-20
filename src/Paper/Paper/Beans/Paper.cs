using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Beans
{
  public class Paper
  {
    private string _title;

    public string Catalog { get; internal set; }

    public string Name { get; internal set; }

    public string Title
    {
      get => _title ?? Name?.ChangeCase(TextCase.ProperCase);
      internal set => _title = value;
    }

    public string Path { get; internal set; }
  }
}
