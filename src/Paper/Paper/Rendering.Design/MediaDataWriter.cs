using Paper.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Rendering.Design
{
  public class MediaDataWriter<T> : IDataWriter
    where T : IMediaObject
  {
    public MediaDataWriter()
    {
    }

    public bool IsPayload => typeof(T) != typeof(Entity);

    public T Result { get; internal set; }

    public void WriteMediaObject(IMediaObject result)
    {
      Result = (T)result;
    }
  }
}
