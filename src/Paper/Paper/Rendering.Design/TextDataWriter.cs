using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Paper.Media;
using Paper.Media.Serialization;

namespace Paper.Rendering.Design
{
  public class TextDataWriter : IDataWriter
  {
    private readonly TextWriter content;
    private readonly string contentType;

    public TextDataWriter(TextWriter content, string contentType)
    {
      this.content = content;
      this.contentType = contentType;
    }

    public bool IsPayload => contentType?.Contains("siren") == true;

    public void WriteData(IEntity result)
    {
      var serializer = new MediaSerializer(contentType);
      serializer.Serialize((Entity)result, content);
    }
  }
}
