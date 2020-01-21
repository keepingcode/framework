using Paper.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Paper.Rendering.Design
{
  public interface IDataReader
  {
    ICollection<IEntity> ReadData();
  }
}
