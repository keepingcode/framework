using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset;
using Toolset.Collections;
using Toolset.Reflection;

namespace Paper.Media
{
  public class VObject : Collection<Property>, IValue, ICloneable
  {
    public VObject()
    {
    }

    public VObject(IEnumerable<Property> initialProperties)
      : base(initialProperties ?? Enumerable.Empty<Property>())
    {
    }

    object IValue.Value
      => this;

    public void CopyFrom(object source)
      => AddMany(Value.CreateObject(source));

    public void CopyTo(object target)
      => throw new NotImplementedException();

    public VObject Clone()
    {
      var collection = new VObject();
      collection.AddMany(this.Select(x => x.Clone()));
      return collection;
    }

    object ICloneable.Clone()
      => Clone();

    protected override void OnCommitAdd(ItemStore store, IEnumerable<Property> items, int index = -1)
    {
      store.RemoveWhen(x => items.Any(y => y.Name.EqualsIgnoreCase(x.Name)));
      base.OnCommitAdd(store, items, index);
    }
  }
}