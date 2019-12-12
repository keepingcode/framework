using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public class Class
  {
    public string Name { get; set; }

    private Class()
    {
    }

    public static implicit operator string(Class @class)
    {
      return @class.Name;
    }

    public static implicit operator Class(string @class)
    {
      return new Class { Name = @class };
    }
  }

  public class Relation
  {
    public string Name { get; set; }

    private Relation()
    {
    }

    public static implicit operator string(Relation @class)
    {
      return @class.Name;
    }

    public static implicit operator Relation(string @class)
    {
      return new Relation { Name = @class };
    }
  }

  public interface IObject : ICollection<object>
  {
  }

  public interface IEntity : IObject
  {
    public string Title { get; set; }
    public IEnumerable<Class> UserClasses();
    public IEnumerable<Class> MetaClasses();
    public IEnumerable<Relation> Relations();
    public IEnumerable<Property> UserProperties();
    public IEnumerable<Property> MetaProperties();
    public IEnumerable<IRecord> Records();
    public IEnumerable<IAction> Actions();
    public IEnumerable<IEntity> MetaEntities();
    public IEnumerable<Link> Links();
  }

  public interface IRecord : IObject
  {
    public IEnumerable<Class> UserClasses();
    public IEnumerable<Property> UserProperties();
    public IEnumerable<IRecord> Records();
  }

  public interface IAction : IObject
  {
    public string Title { get; set; }
    public IEnumerable<Class> UserClasses();
    public IEnumerable<Class> MetaClasses();
    public IEnumerable<Relation> Relations();
    public IEnumerable<Property> MetaProperties();
    public IEnumerable<IField> Fields();
    public IEnumerable<IEntity> MetaEntities();
    public IEnumerable<Link> Links();
  }

  public interface IField : IObject
  {
    public string Title { get; set; }
    public IEnumerable<Class> UserClasses();
    public IEnumerable<Class> MetaClasses();
    public IEnumerable<Relation> Relations();
    public IEnumerable<Property> MetaProperties();
    public IEnumerable<IEntity> MetaEntities();
    public IEnumerable<Link> Links();
  }

  public class Entity : Collection<object>, IEntity, IRecord, IAction, IField
  {
    public string Title
    {
      get
      {
        var property = this.OfType<Property>().Where(x => x.Name == "__Title").FirstOrDefault();
        return property?.Value.Value as string;
      }
      set
      {
        var property = this.OfType<Property>().Where(x => x.Name == "__Title").FirstOrDefault();
        this.Remove(property);
        this.Add(new Property { Name = "__Title", Value = Value.Create(value) });
      }
    }

    public IEnumerable<Class> UserClasses()
      => this.OfType<Class>().Where(x => char.IsUpper(x.Name.First()));

    public IEnumerable<Class> MetaClasses()
      => this.OfType<Class>().Where(x => char.IsLower(x.Name.First()));

    public IEnumerable<Relation> Relations()
      => this.OfType<Relation>();

    public IEnumerable<Property> UserProperties()
      => this.OfType<Property>().Where(x => !x.Name.StartsWith("__"));

    public IEnumerable<Property> MetaProperties()
      => this.OfType<Property>().Where(x => x.Name.StartsWith("__"));

    public IEnumerable<IRecord> Records()
      => this.OfType<Entity>().Where(x => x.MetaClasses().Any(y => y == ClassNames.Record));

    public IEnumerable<IAction> Actions()
      => this.OfType<Entity>().Where(x => x.MetaClasses().Any(y => y == ClassNames.Action));

    public IEnumerable<IField> Fields()
      => this.OfType<Entity>().Where(x => x.MetaClasses().Any(y => y == ClassNames.Field));

    public IEnumerable<IEntity> MetaEntities()
      => this.OfType<Entity>().Where(x => x.MetaClasses().Any(y =>
           y != ClassNames.Record && y != ClassNames.Action && y != ClassNames.Field
         ));

    public IEnumerable<Link> Links()
      => this.OfType<Link>();

    public static IEntity CreateRecord()
    {
      var entity = new Entity();
      entity.Add((Class)ClassNames.Record);
      return entity;
    }

    public static IEntity CreateAction()
    {
      var entity = new Entity();
      entity.Add((Class)ClassNames.Action);
      return entity;
    }

    public static IEntity CreateField()
    {
      var entity = new Entity();
      entity.Add((Class)ClassNames.Field);
      return entity;
    }
  }
}
