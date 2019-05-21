using System;
using System.Collections.Generic;
using System.Text;
using Toolset.Net;

namespace Paper.Media3.Serialization
{
  public static class EntityExtensions
  {
    public static string Serialize(this IEntity entity)
    {
      return Serialize(entity, MimeTypeNames.SirenXml);
    }

    public static string Serialize(this IEntity entity, string mimeType)
    {
      return null;
    }
  }
}
