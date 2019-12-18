using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Toolset.Collections;
using Toolset.Net;
using Toolset.Serialization;
using Toolset.Serialization.Csv;
using Toolset.Serialization.Json;
using Toolset.Serialization.Xml;

namespace Paper.Media.Serialization
{
  public class MediaSerializer
  {
    private bool isJson;

    public MediaSerializer(MimeType mimeType)
      : this(mimeType.GetName())
    {
    }

    public MediaSerializer(string mimeType)
    {
      this.isJson = mimeType.Contains("json");
    }

    public void Serialize(IEntity entity, TextWriter writer)
    {
      var documentWriter = isJson
        ? (Writer)new JsonWriter(writer)
        : (Writer)new XmlDocumentWriter(writer);

      var mediaWriter = new MediaWriter();
      mediaWriter.Write(entity, documentWriter);

      documentWriter.Flush();
      writer.Flush();
    }

    public ICollection<IEntity> Deserialize(TextReader reader)
    {
      var documentReader = isJson
        ? (Reader)new JsonReader(reader)
        : (Reader)new XmlDocumentReader(reader);

      var mediaReader = new MediaReader();
      var medias = mediaReader.Read(documentReader);

      var entities = medias.OfType<IEntity>().ToArray();
      return entities;
    }
  }
}