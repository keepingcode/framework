using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Toolset;
using Toolset.Reflection;

namespace Paper.Media
{
  public static class Value
  {
    public static IValue Create(object value)
    {
      if (value == null) return Null.Default;
      if (value is IValue cloneable) return cloneable.Clone();

      if (value is string @string) return (Text)@string;

      if (value is bool @bool) return (Bit)@bool;

      if (value is int @int) return (Number)@int;
      if (value is float @float) return (Number)@float;
      if (value is double @double) return (Number)@double;
      if (value is decimal @decimal) return (Number)@decimal;

      if (value is DateTime dateTime) return (Time)dateTime;

      if (value is IEnumerable<Property> props) return new PropertyCollection(props.Select(x => x.Clone()));
      if (value is IEnumerable terms) return CreateArray(terms);

      var target = new PropertyCollection();
      target.CopyFrom(value);
      return target;
    }

    public static ValueCollection CreateArray(params object[] arrayItems)
    {
      var values = arrayItems.Select(Create);
      var collection = new ValueCollection(values);
      return collection;
    }

    public static ValueCollection CreateArray(IEnumerable enumerable)
    {
      var items = enumerable.Cast<object>();
      var values = items.Select(Create);
      var collection = new ValueCollection(values);
      return collection;
    }

    public static void CopyFrom(this PropertyCollection target, object source)
    {
      IEnumerable<Tuple<string, PropertyInfo>> properties;

      if (source._HasAttribute<DataContractAttribute>())
      {
        properties =
          from property in source.GetType().GetProperties()
          let attribute = property._GetAttribute<DataMemberAttribute>()
          where attribute != null
          select Tuple.Create(attribute.Name ?? property.Name, property);
      }
      else
      {
        properties =
          from property in source.GetType().GetProperties()
          where !property._HasAttribute<XmlIgnoreAttribute>()
          let attribute = property._GetAttribute<XmlElementAttribute>()
          select Tuple.Create(attribute?.ElementName ?? property.Name, property);
      }

      foreach (var property in properties)
      {
        var name = property.Item1;
        var getter = property.Item2;

        var value = getter.GetValue(source);
        var supportedValue = Value.Create(value);

        target.Add(new Property
        {
          Name = name,
          Value = supportedValue
        });
      }
    }

    public static void CopyTo(this PropertyCollection source, object target)
    {
      if (target is Type type)
      {
        target = Activator.CreateInstance(type);
      }

      var isDataContract = target._HasAttribute<DataContractAttribute>();

      foreach (var property in source)
      {
        string propertyName;

        if (isDataContract)
        {
          var targetPropertyName = (
            from prop in target.GetType().GetProperties()
            from attr in prop._GetAttributes<DataMemberAttribute>()
            where attr.Name?.EqualsIgnoreCase(property.Name) == true
            select prop.Name
          ).FirstOrDefault();
          propertyName = targetPropertyName ?? property.Name;
        }
        else
        {
          var targetPropertyName = (
            from prop in target.GetType().GetProperties()
            from attr in prop._GetAttributes<XmlElementAttribute>()
            where attr.ElementName?.EqualsIgnoreCase(property.Name) == true
            select prop.Name
          ).FirstOrDefault();
          propertyName = targetPropertyName ?? property.Name;
        }

        if (isDataContract && !target._HasAttribute<DataMemberAttribute>(propertyName))
          continue;

        if (target._HasAttribute<XmlIgnoreAttribute>(propertyName))
          continue;

        if (property.Value is PropertyCollection collection)
        {
          var propertyValue = target._SetNew(propertyName);
          CopyTo(collection, propertyValue);
        }
        else
        {
          target._Set(propertyName, property.Value.Value);
        }
      }
    }

    public static object CopyTo(this PropertyCollection source, Type targetType)
    {
      var @object = Activator.CreateInstance(targetType);
      CopyTo(source, @object);
      return @object;
    }

    public static T CopyTo<T>(this PropertyCollection source)
      where T : new()
    {
      var @object = new T();
      CopyTo(source, @object);
      return @object;
    }
  }
}
