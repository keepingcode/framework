using Paper.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Paper.Rendering.Design
{
  public interface IDataWriter
  {
    bool IsPayload { get; }

    void WriteData(IEntity data);
  }
}
