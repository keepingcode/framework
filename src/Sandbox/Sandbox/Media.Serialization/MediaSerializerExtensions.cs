using System;
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
  }
}
