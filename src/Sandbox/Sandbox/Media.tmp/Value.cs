using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Reflection;

namespace Paper.Media
{
  public static class Value
  {
    public static IValue Get(object value)
    {
      if (value == null) return Null.Default;
      if (value is IValue) return (IValue)value;

      if (value is string @string) return (Text)@string;

      if (value is bool @bool) return (Bit)@bool;

      if (value is int @int) return (Number)@int;
      if (value is float @float) return (Number)@float;
      if (value is double @double) return (Number)@double;
      if (value is decimal @decimal) return (Number)@decimal;

      if (value is DateTime dateTime) return (Time)dateTime;

      throw new NotSupportedException($"O valor não corresponde a um tipo suportado pelo Media Paper: {value.GetType().FullName}");
    }

    public static ValueCollection GetArray(params object[] arrayItems)
    {
      var values = arrayItems.Select(Get);
      var collection = new ValueCollection(values);
      return collection;
    }
  }
}
