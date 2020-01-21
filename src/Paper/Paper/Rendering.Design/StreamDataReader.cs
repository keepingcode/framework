using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Paper.Media;
using Paper.Media.Serialization;

namespace Paper.Rendering.Design
{
  public class StreamDataReader : IDataReader
  {
    private readonly Stream content;
    private readonly string contentType;
    private readonly Encoding encoding;

    public StreamDataReader(Stream content, string contentType, Encoding encoding = null)
    {
      this.content = content;
      this.contentType = contentType;
      this.encoding = encoding ?? Encoding.UTF8;
    }

    public ICollection<IEntity> ReadData()
    {
      var serializer = new MediaSerializer(contentType);
      var result = serializer.Deserialize(content, encoding);
      return result;
    }
  }
}
