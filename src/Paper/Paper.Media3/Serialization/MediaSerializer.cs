using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Paper.Media3.Serialization
{
  public class MediaSerializer
  {
    public void Serialize(Entity entity, Stream output)
    {
    }

    public void Serialize(Entity entity, TextWriter output)
    {
    }

    public IEntity Deserialize(Stream input)
    {
      return null;
    }

    public IEntity Deserialize(TextReader input)
    {
      return null;
    }
  }
}
