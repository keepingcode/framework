using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Paper.Media.Serialization
{
  public static class MediaSerializerExtensions
  {
    public static string Serialize(this MediaSerializer serializer, IEntity entity)
    {
      using (var writer = new StringWriter())
      {
        serializer.Serialize(entity, writer);
        return writer.ToString();
      }
    }

    public static void Serialize(this MediaSerializer serializer, IEntity entity, Stream stream, Encoding encoding = null)
    {
      var writer = new StreamWriter(stream, encoding ?? Encoding.UTF8);
      serializer.Serialize(entity, writer);
      writer.Flush();
      stream.Flush();
    }

    public static ICollection<IEntity> Deserialize(this MediaSerializer serializer, string image)
    {
      using (var reader = new StringReader(image))
      {
        var result = serializer.Deserialize(reader);
        return result;
      }
    }

    public static ICollection<IEntity> Deserialize(this MediaSerializer serializer, IEntity entity, Stream stream, Encoding encoding = null)
    {
      var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);
      var result = serializer.Deserialize(reader);
      return result;
    }
  }
}
