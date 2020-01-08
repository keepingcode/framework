using System;
using System.Collections.Generic;
using System.Linq;

namespace Paper.Media
{
  public class EntityRef : NodeCollection, IEntityRef
  {
    public object Tag { get; set; }

    public string Type
    {
      get => Get<string>();
      set => Set(value);
    }

    public string Href
    {
      get => Get<string>();
      set => Set(value);
    }

    public string Title
    {
      get => Get<string>();
      set => Set(value);
    }

    protected override void OnCommitAdd(ItemStore store, IEnumerable<INode> items, int index = -1)
    {
      var invalidNode = items.OfType<IMedia>().FirstOrDefault();
      if (invalidNode != null)
        throw new InvalidOperationException($"Um link não suporta propriedades do tipo {invalidNode.GetType().FullName}");

      base.OnCommitAdd(store, items, index);
    }
  }
}