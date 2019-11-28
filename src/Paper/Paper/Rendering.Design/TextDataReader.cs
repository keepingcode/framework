using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Paper.Media;
using Paper.Media.Serialization;

namespace Paper.Rendering.Design
{
  public class TextDataReader : IDataReader
  {
    private readonly TextReader content;
    private readonly string contentType;

    public TextDataReader(TextReader content, string contentType)
    {
      this.content = content;
      this.contentType = contentType;
    }

    public IMediaObject ReadMediaObject()
    {
      var serializer = new MediaSerializer(contentType);
      var result = serializer.Deserialize(content);
      return result;
    }
  }
}
