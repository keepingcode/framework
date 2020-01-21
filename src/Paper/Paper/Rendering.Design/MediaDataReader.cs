using Paper.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Rendering.Design
{
  public class MediaDataReader<T> : IDataReader
    where T : IEntity
  {
    private readonly T mediaObject;

    public MediaDataReader(T mediaObject)
    {
      this.mediaObject = mediaObject;
    }

    public ICollection<IEntity> ReadData() => new[] { (IEntity)mediaObject };
  }
}
