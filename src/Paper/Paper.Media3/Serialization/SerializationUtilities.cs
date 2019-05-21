using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset;

namespace Paper.Media3.Serialization
{
  public static class SerializationUtilities
  {
    /// <summary>
    /// Determina se o tipo é suportado pelo algoritmo de serialização.
    /// </summary>
    /// <param name="item">O item analisado.</param>
    /// <returns>Verdadeiro se o item é serializável; Falso caso contrário.</returns>
    public static bool IsSerializable(object item)
    {
      if (item == null || item is PropertyMap)
        return true;

      if (item is IEnumerable list && !(item is string))
      {
        if (item is IDictionary map)
        {
          var isSerializable =
               map.Keys.Cast<object>().All(IsStringCompatible)
            && map.Values.Cast<object>().All(IsSerializable);
          return isSerializable;
        }

        var elementType = TypeOf.CollectionElement(item);
        if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
          return false;

        return list.Cast<object>().All(IsSerializable);
      }

      if (item.GetType().IsGenericType && item.GetType().GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
        return false;

      return item.GetType().IsValueType || IsStringCompatible(item);
    }

    /// <summary>
    /// Determina se o tipo deve ser considerado compatível com string.
    /// </summary>
    /// <param name="objectOrType">O valor a ser verificado.</param>
    /// <returns>Verdadeiro se o tipo pode ser considerado string; Falso caso contrário.</returns>
    public static bool IsStringCompatible(object objectOrType)
    {
      var type = objectOrType as Type ?? objectOrType?.GetType();
      if (type == null)
        return false;

      return type == typeof(string)
          || type == typeof(CaseVariantString)
          || type == typeof(Href)
          || type == typeof(Uri)
          || type == typeof(UriString)
          || type == typeof(Guid)
          || type == typeof(Version)
          || type.IsEnum
          || type.FullName == "Microsoft.AspNetCore.Http.PathString";
    }

    /// <summary>
    /// Determina se o objeto é considerado um grafo.
    /// Grafo é qualquer objeto que contenha propriedades em vez de representar
    /// um valor final.
    /// </summary>
    /// <param name="objectOrType">Objeto ou tipo do objeto.</param>
    /// <returns>
    /// Verdadeiro se o objeto é considerado um grafo; Falso caso contrário.
    /// </returns>
    public static bool IsGraph(object objectOrType)
    {
      if (objectOrType == null)
        return false;

      var type = objectOrType as Type ?? objectOrType.GetType();

      if (type.IsValueType)
        return false;

      if (IsStringCompatible(objectOrType))
        return false;

      if (Is.Collection(type) || Is.Dictionary(type))
        return false;

      return true;
    }
  }
}
