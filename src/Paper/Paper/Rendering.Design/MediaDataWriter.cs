using Paper.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Rendering.Design
{
  public class MediaDataWriter<T> : IDataWriter
    where T : IEntity
  {
    public MediaDataWriter()
    {
    }

    public bool IsPayload => typeof(T) != typeof(Entity);

    public T Result { get; internal set; }

    public void WriteData(IEntity result)
    {
      Result = (T)result;
    }
  }
}
