using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paper.Media3.Design;
using Paper.Media3.Serialization;
using Toolset;
using Toolset.Collections;
using Toolset.Reflection;

namespace Paper.Media3
{
  public class PropertyMap : IPropertyMap
  {
    private readonly HashMap cache;
    private readonly Map graph;

    public PropertyMap()
    {
      cache = new HashMap();
      graph = new Map(null);
    }

    internal PropertyMap(IEntity entity)
    {
      cache = new HashMap();

      if (entity is Entity)
      {
        graph = new Map(null);
      }
      else if (entity is IDataEntity dataEntity)
      {
        graph = new Map(() => dataEntity.Data);
      }
      else
      {
        graph = new Map(() => entity, new[] { "Class", "Rel", "Entities", "Links" });
      }
    }

    public IEnumerable<string> PropertyNames
    {
      get => graph.PropertyNames.Union(cache.Keys).ToArray();
    }

    public object this[string propertyName]
    {
      get
      {
        object value;

        value = graph[propertyName];
        if (value != null)
          return value;

        value = cache[propertyName];
        if (value != null)
          return GetCompatibleValue(value);

        return value;
      }
      set
      {
        if (graph.PropertyNames.Contains(propertyName))
        {
          graph[propertyName] = value;
        }
        else
        {
          cache[propertyName] = value;
        }
      }
    }

    private static object GetCompatibleValue(object value)
    {
      if (value is Var var)
        value = var.RawValue;

      if (value == null)
        return value;

      if (value is string)
        return value;

      if (value is IDictionary dictionary)
        return new Map(() => dictionary);

      if (value is IEnumerable enumerable)
        return new List(() => enumerable);

      if (SerializationUtilities.IsGraph(value))
        return new Map(() => value);

      return value;
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
      var enumerable =
        from name in PropertyNames
        let value = this[name]
        select KeyValuePair.Create(name, value);
      return enumerable.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    private class Map : IPropertyMap
    {
      private readonly Func<object> getter;
      private readonly string[] ignoredProperties;

      public Map(Func<object> getter, params string[] ignoredProperties)
      {
        this.getter = getter;
        this.ignoredProperties = ignoredProperties;
      }

      public IEnumerable<string> PropertyNames
      {
        get
        {
          var graph = getter?.Invoke();
          if (graph == null)
            return Enumerable.Empty<string>();

          if (graph is IDictionary dictionary)
            return dictionary.Keys.Cast<string>();

          var propertyNames =
            from name in graph._GetPropertyNames()
            let property = graph._GetPropertyInfo(name)
            where !property._HasAttribute<IgnoreAttribute>()
            select name;

          return propertyNames.Except(ignoredProperties);
        }
      }

      public object this[string propertyName]
      {
        get
        {
          var graph = getter?.Invoke();
          var value = graph is IDictionary dictionary
            ? dictionary[propertyName]
            : graph?._Get(propertyName);
          return GetCompatibleValue(value);
        }
        set
        {
          var graph = getter?.Invoke();
          if (graph is IDictionary dictionary)
          {
            dictionary[propertyName] = value;
          }
          else
          {
            graph._Set(propertyName, value);
          }
        }
      }

      public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
      {
        var enumerable =
          from name in PropertyNames
          let value = this[name]
          select KeyValuePair.Create(name, value);
        return enumerable.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return GetEnumerator();
      }
    }

    private class List : IPropertyList
    {
      private readonly Func<IEnumerable> getter;

      public List(Func<IEnumerable> getter)
      {
        this.getter = getter;
      }

      public int Count
      {
        get
        {
          var enumerable = getter.Invoke();
          return enumerable.Cast<object>().Count();
        }
      }

      public Type ElementType => TypeOf.Collection(getter.Invoke());

      public object this[int index]
      {
        get
        {
          var enumerable = getter.Invoke();
          var value = enumerable.Cast<object>().ElementAt(index);
          return GetCompatibleValue(value);
        }
        set
        {
          var enumerable = getter.Invoke();
          if (enumerable is IList list)
          {
            list[index] = value;
          }
          else
          {
            throw new NotSupportedException(
              $"Operação não suportado em um objeto do tipo: {enumerable.GetType().FullName}"
            );
          }
        }
      }

      public IEnumerator GetEnumerator()
      {
        var enumerable = getter.Invoke();
        var convertedEnumerable = enumerable.Cast<object>().Select(GetCompatibleValue);
        return convertedEnumerable.GetEnumerator();
      }

      public void Add(object item)
      {
        var enumerable = getter.Invoke();
        if (enumerable is IList list)
        {
          list.Add(item);
        }
        else
        {
          throw new NotSupportedException(
            $"Operação não suportado em um objeto do tipo: {enumerable.GetType().FullName}"
          );
        }
      }

      public void Remove(int index)
      {
        var enumerable = getter.Invoke();
        if (enumerable is IList list)
        {
          list.Remove(index);
        }
        else
        {
          throw new NotSupportedException(
            $"Operação não suportado em um objeto do tipo: {enumerable.GetType().FullName}"
          );
        }
      }
    }
  }
}