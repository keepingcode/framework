using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Toolset;
using Toolset.Collections;

namespace Paper.Media
{
  public class NodeCollection : Collection<INode>
  {
    public IEnumerable<INode> Descendants()
    {
      return DescendantsAndSelf().Skip(1);
    }

    public IEnumerable<INode> DescendantsAndSelf()
    {
      var stack = new Stack<NodeCollection>();
      stack.Push(this);
      while (stack.Count > 0)
      {
        var nodes = stack.Pop();
        if (nodes is INode node) yield return node;
        nodes.OfType<NodeCollection>().ForEach(stack.Push);
      }
    }

    public T GetProperty<T>(string path)
      where T : class, IValue
    {
      var property = GetProperty(path);
      return property?.Value as T;
    }

    public Property GetProperty(string path)
    {
      Property property = null;

      var tokens = path.Split('.');
      var properties = this.OfType<Property>();

      foreach (var token in tokens)
      {
        if (properties == null)
          return null;

        property = properties.FirstOrDefault(x => x.Name.EqualsIgnoreCase(token));

        if (property == null)
          return null;

        properties = property.Value as IEnumerable<Property>;
      }

      return property;
    }

    public void SetProperty(string path, object value)
    {
      var tokens = path.Split('.');
      var parentNames = tokens.Take(tokens.Length - 1);
      var targetName = tokens.Last();

      IList nodes = this;

      foreach (var parentName in parentNames)
      {
        var parent = nodes
          .OfType<Property>()
          .FirstOrDefault(x => x.Name.EqualsIgnoreCase(parentName));

        if (parent == null)
        {
          parent = new Property(parentName);
          nodes.Add(parent);
        }

        nodes = parent.Value as VObject;
        if (nodes == null)
        {
          nodes = new VObject();
          parent.Value = (VObject)nodes;
        }
      }

      nodes.Add(new Property(targetName, value));
    }

    protected T Get<T>([CallerMemberName]string propertyName = null)
    {
      var property =
        this.OfType<Property>().FirstOrDefault(x => x.Name == propertyName);
      var value = property?.Value?.Value;
      var castValue = Change.To<T>(value);
      return castValue;
    }

    protected void Set<T>(T value, [CallerMemberName]string propertyName = null)
    {
      var property =
        this.OfType<Property>().FirstOrDefault(x => x.Name == propertyName);

      if (property != null)
      {
        property.Value = Value.Create(value);
      }
      else
      {
        this.Add(new Property(propertyName, value));
      }
    }

    protected override void OnCommitAdd(ItemStore store, IEnumerable<INode> items, int index = -1)
    {
      {
        var olders = store.OfType<Class>();
        var newers = items.OfType<Class>();
        if (olders.Any() && newers.Any())
        {
          var discards = olders.Where(older =>
            newers.Any(newer => newer.Name.Equals(older.Name))
          ).ToArray();
          store.RemoveMany(discards);
        }
      }

      {
        var olders = store.OfType<Rel>();
        var newers = items.OfType<Rel>();
        if (olders.Any() && newers.Any())
        {
          var discards = olders.Where(older =>
            newers.Any(newer => newer.Name.Equals(older.Name))
          ).ToArray();
          store.RemoveMany(discards);
        }
      }

      {
        var olders = store.OfType<Property>();
        var newers = items.OfType<Property>();
        if (olders.Any() && newers.Any())
        {
          var discards = olders.Where(older =>
            newers.Any(newer => newer.Name.Equals(older.Name))
          ).ToArray();
          store.RemoveMany(discards);
        }
      }

      {
        var olders = store.OfType<Action>();
        var newers = items.OfType<Action>();
        if (olders.Any() && newers.Any())
        {
          var discards = olders.Where(older =>
            newers.Any(newer => newer.Name.Equals(older.Name))
          ).ToArray();
          store.RemoveMany(discards);
        }
      }

      {
        var olders = store.OfType<Field>();
        var newers = items.OfType<Field>();
        if (olders.Any() && newers.Any())
        {
          var discards = olders.Where(older =>
            newers.Any(newer => newer.Name.Equals(older.Name))
          ).ToArray();
          store.RemoveMany(discards);
        }
      }

      base.OnCommitAdd(store, items, index);
    }
  }
}
