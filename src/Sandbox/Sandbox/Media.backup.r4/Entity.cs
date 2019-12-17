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
    private Class()
    {
    }

    public string Name { get; set; }

    public bool MetaClass => char.IsLower(Name.First());

    public static implicit operator string(Class @class)
      => @class.Name;

    public static implicit operator Class(string @class)
      => string.IsNullOrWhiteSpace(@class) ? null : new Class { Name = @class.Trim() };
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

  public interface IEntity : ICollection<object>
  {
    string Title { get; set; }

    IEnumerable<Class> Classes();

    IEnumerable<Relation> Relations();

    IEnumerable<Property> UserProperties();

    IEnumerable<Property> MetaProperties();

    IEnumerable<Record> Records();

    IEnumerable<Action> Actions();

    IEnumerable<Field> Fields();

    IEnumerable<IEntity> MetaEntities();

    IEnumerable<Link> Links();
  }




  public class Record : EntityBase
  {
  }

  public abstract class EntityBase : Collection<object>
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

    public IEnumerable<Class> Classes()
      => this.OfType<Class>();

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
