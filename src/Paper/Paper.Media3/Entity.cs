using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Paper.Media3
{
  public class Entity : IEntity
  {
    private NameCollection _class;
    private string _title;
    private NameCollection _rel;
    private ValueMap _properties;
    private EntityCollection _entities;
    private LinkCollection _links;

    public NameCollection Class
    {
      get => _class ?? (_class = new NameCollection());
      set => _class = value;
    }

    public string Title
    {
      get => _title ?? Change.To<string>(Properties["title"]);
      set => _title = value;
    }

    public NameCollection Rel
    {
      get => _rel ?? (_rel = new NameCollection());
      set => _rel = value;
    }

    public ValueMap Properties
    {
      get => _properties ?? (_properties = new ValueMap());
      set => _properties = value;
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
  }
}