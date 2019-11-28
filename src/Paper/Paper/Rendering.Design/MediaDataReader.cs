using Paper.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Rendering.Design
{
  public class MediaDataReader<T> : IDataReader
    where T : IMediaObject
  {
    private readonly T mediaObject;

    public MediaDataReader(T mediaObject)
    {
      this.mediaObject = mediaObject;
    }

    public IMediaObject ReadMediaObject() => mediaObject;
  }
}
