using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Toolset.Reflection;

namespace Paper.Design
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public class KeyAttribute : Attribute
  {
    public static PropertyInfo FindKey(object host)
    {
      var property = (
        from propertyName in host._GetPropertyNames()
        let attribute = host._GetAttribute<KeyAttribute>(propertyName)
        where attribute != null
        select host._GetPropertyInfo(propertyName)
      ).FirstOrDefault();

      if (property != null)
        return property;

      property = host._GetPropertyInfo("Id");
      if (property != null)
        return property;

      throw new NotSupportedException("O objeto não possui uma chave primária mapeada.");
    }
  }
}
