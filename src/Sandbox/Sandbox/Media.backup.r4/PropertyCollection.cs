using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox;
using Toolset;
using Toolset.Collections;
using Toolset.Reflection;

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

    public void Add(string name, IValue value)
    {
      this.Add(new Property
      {
        Name = name,
        Value = value
      });
    }

    public void AddAt(int index, string name, IValue value)
    {
      this.AddAt(index, new Property
      {
        Name = name,
        Value = value
      });
    }

    public static PropertyCollection CreateFrom(object @object)
    {
      return (PropertyCollection)Value.Create(@object);
    }

    public PropertyCollection Clone()
    {
      var collection = new PropertyCollection();
      collection.AddMany(this.Select(x => x.Clone()));
      return collection;
    }

    IValue IValue.Clone() => Clone();

    object ICloneable.Clone() => Clone();
  }
}