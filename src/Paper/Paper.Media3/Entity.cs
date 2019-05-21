using System;
using System.Collections.Generic;
using System.Text;
using Paper.Media3.Design;
using Toolset;

namespace Paper.Media3
{
  public class Entity : IEntity
  {
    private NameCollection _class;
    private string _title;
    private NameCollection _rel;
    private EntityCollection _entities;
    private LinkCollection _links;

    public Entity()
    {
      this.Properties = new PropertyMap(this);
    }

    public NameCollection Class
    {
      get => _class ?? (_class = new NameCollection());
      set => _class = value;
    }

    public string Title
    {
      get => _title ?? Change.To<string>(Properties["Title"]);
      set => _title = value;
    }

    public NameCollection Rel
    {
      get => _rel ?? (_rel = new NameCollection());
      set => _rel = value;
    }

    public PropertyMap Properties
    {
      get;
    }

    public EntityCollection Entities
    {
      get => _entities ?? (_entities = new EntityCollection());
      set => _entities = value;
    }

    public LinkCollection Links
    {
      get => _links ?? (_links = new LinkCollection());
      set => _links = value;
    }

    public static DataEntity<T> Create<T>(T data)
      where T : class
    {
      return new DataEntity<T>(data);
    }
  }
}