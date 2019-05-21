using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media3.Design
{
  public interface IDataEntity : IEntity
  {
    object Data { get; set; }
  }
}
