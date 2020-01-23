using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Paper.Media.Design;
using Toolset;
using Toolset.Reflection;

namespace Paper.Media
{
  public static class Value
  {
    public static IValue Create(object value)
    {
      if (value == null) return null;

      if (value is string @string) return (VString)@string;
      if (value is bool @bool) return (VBoolean)@bool;
      if (value is int @int) return (VInteger)@int;
      if (value is long @long) return (VLong)@long;
      if (value is float @float) return (VDecimal)@float;
      if (value is double @double) return (VDecimal)@double;
      if (value is decimal @decimal) return (VDecimal)@decimal;
      if (value is DateTime dateTime) return (VDateTime)dateTime;
      if (value is TimeSpan timeSpan) return (VTimeSpan)timeSpan;

      if (value is IValue instance)
        return (IValue)(value as ICloneable)?.Clone() ?? instance;

      if (value is IEnumerable<Property> props)
        return new VObject(props.Select(x => x.Clone()));

      if (value is IEnumerable terms)
        return CreateArray(terms);

      if (value._HasAttribute<DataContractAttribute>()
       || value._HasAttribute<MediaContractAttribute>()
       || value._HasAttribute<CompilerGeneratedAttribute>())
        return CreateObject(value);

      throw new NotSupportedException($"O tipo do valor não é suportado pelo Paper: {value.GetType().FullName}");
    }

    public static VArray CreateArray(params object[] arrayItems)
    {
      var values = arrayItems.Select(Create);
      var collection = new VArray(values);
      return collection;
    }

    public static VArray CreateArray(IEnumerable enumerable)
    {
      var items = enumerable.Cast<object>();
      var values = items.Select(Create);
      var collection = new VArray(values);
      return collection;
    }

    public static VObject CreateObject(object source)
    {
      IEnumerable<Tuple<string, PropertyInfo>> properties;

      if (source._HasAttribute<DataContractAttribute>())
      {
        properties =
          from property in source.GetType().GetProperties()
          let attribute = property._GetAttribute<DataMemberAttribute>()
          where attribute != null
          orderby attribute.Order
          select Tuple.Create(attribute.Name ?? property.Name, property);
      }
      else if (source._HasAttribute<MediaContractAttribute>())
      {
        properties =
          from property in source.GetType().GetProperties()
          let attribute = property._GetAttribute<MediaMemberAttribute>()
          where attribute != null
          orderby attribute.Order
          select Tuple.Create(attribute.Name ?? property.Name, property);
      }
      else
      {
        properties =
          from property in source.GetType().GetProperties()
          where !property._HasAttribute<XmlIgnoreAttribute>()
          let attribute = property._GetAttribute<XmlElementAttribute>()
          orderby attribute?.Order ?? 0
          select Tuple.Create(attribute?.ElementName ?? property.Name, property);
      }

      var @object = new VObject();

      foreach (var property in properties)
      {
        var targetProperty = property.Item1;

        var sourceValueGetter = property.Item2;
        var sourceValue = sourceValueGetter.GetValue(source);
        var targetValue = Create(sourceValue);

        @object.Add(new Property(targetProperty, targetValue));
      }

      return @object;
    }
  }
}
