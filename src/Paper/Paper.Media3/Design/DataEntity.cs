using System;
using System.Collections.Generic;
using System.Text;
using Toolset;
using Toolset.Reflection;

namespace Paper.Media3.Design
{
  public class DataEntity : AbstractEntity, IDataEntity
  {
    private string _title;

    public DataEntity()
    {
    }

    public DataEntity(object data)
    {
      this.Data = data;
    }

    public override string Title
    {
      get => _title ?? Data._Get<string>("Title");
      set => _title = value;
    }

    public object Data { get; set; }

    public static DataEntity<T> Create<T>(T data)
      where T : class
    {
      return new DataEntity<T>(data);
    }
  }
}