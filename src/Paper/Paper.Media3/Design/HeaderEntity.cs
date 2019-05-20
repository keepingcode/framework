using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media3.Design
{
  public class HeaderEntity : AbstractEntity
  {
    private NameCollection _type;

    public string Name { get; set; }

    public NameCollection Type
    {
      get => _type ?? (_type = new NameCollection());
      set => _type = value;
    }
  }
}