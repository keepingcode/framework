using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Paper.Media;
using Paper.Media.Serialization;

namespace Paper.Rendering.Design
{
  public class StreamDataWriter : IDataWriter
  {
    private readonly Stream content;
    private readonly string contentType;
    private readonly Encoding encoding;

    public StreamDataWriter(Stream content, string contentType, Encoding encoding = null)
    {
      this.content = content;
      this.contentType = contentType;
      this.encoding = encoding ?? Encoding.UTF8;
    }

    public bool IsPayload => contentType?.Contains("siren") == true;

    public void WriteMediaObject(IMediaObject result)
    {
      var serializer = new MediaSerializer(contentType);
      serializer.Serialize((Entity)result, content, encoding);
    }
  }
}
