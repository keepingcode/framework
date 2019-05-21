using System;
using System.Collections.Generic;
using System.Text;
using Toolset;
using Toolset.Reflection;

namespace Paper.Media3.Design
{
  public class DataEntity<T> : AbstractEntity, IDataEntity
    where T : class
  {
    private string _title;

    public DataEntity()
    {
    }

    public DataEntity(T data)
    {
      this.Data = data;
    }

    public override string Title
    {
      get => _title ?? Data._Get<string>("Title");
      set => _title = value;
    }

    public T Data
    {
      get;
      set;
    }

    object IDataEntity.Data
    {
      get => Data;
      set => Data = (T)value;
    }
  }
}
