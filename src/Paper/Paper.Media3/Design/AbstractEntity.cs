using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media3.Design
{
  public abstract class AbstractEntity : IEntity
  {
    private NameCollection _class;
    private string _title;
    private NameCollection _rel;
    private EntityCollection _entities;
    private LinkCollection _links;

    public virtual NameCollection Class
    {
      get => _class ?? (_class = new NameCollection());
      set => _class = value;
    }

    public virtual string Title
    {
      get;
      set;
    }

    public virtual NameCollection Rel
    {
      get => _rel ?? (_rel = new NameCollection());
      set => _rel = value;
    }

    public virtual EntityCollection Entities
    {
      get => _entities ?? (_entities = new EntityCollection());
      set => _entities = value;
    }

    public virtual LinkCollection Links
    {
      get => _links ?? (_links = new LinkCollection());
      set => _links = value;
    }
  }
}