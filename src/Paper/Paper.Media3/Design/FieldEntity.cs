using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Media3.Design
{
  public class FieldEntity : AbstractEntity
  {
    private NameCollection _type;

    public string Name { get; set; }

    public NameCollection Type
    {
      get => _type ?? (_type = new NameCollection());
      set => _type = value;
    }

    public Var Value { get; set; }
  }
}