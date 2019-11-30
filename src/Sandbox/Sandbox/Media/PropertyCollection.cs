using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public class PropertyCollection : Collection<Property>, IValue
  {
    public PropertyCollection()
    {
    }

    public PropertyCollection(int capacity)
      : base(capacity)
    {
    }

    public PropertyCollection(IEnumerable<Property> items)
      : base(items ?? Enumerable.Empty<Property>())
    {
    }

    object IValue.Value => this;

    public void Add(CaseVariantText name, IValue value)
    {
      this.Add(new Property
      {
        Name = name,
        Value = value
      });
    }

    public void AddAt(int index, CaseVariantText name, IValue value)
    {
      this.AddAt(index, new Property
      {
        Name = name,
        Value = value
      });
    }
  }
}