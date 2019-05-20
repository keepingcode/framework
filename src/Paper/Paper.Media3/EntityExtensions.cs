using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Media3
{
  public static class EntityExtensions
  {
    public static T GetProperty<T>(this IEntity entity, string property)
    {
      var value = GetProperty(entity, property);
      return Change.To<T>(value);
    }

    public static object GetProperty(this IEntity entity, string property)
    {
      return PropertyHandler.GetProperty(entity, property);
    }

    public static void SetProperty(this IEntity entity, string property, object value)
    {
      PropertyHandler.SetProperty(entity, property, value);
    }
  }
}
