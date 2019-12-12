using System;
using System.Collections.Generic;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public static class EntityExtensions
  {
    public static NameCollection WithClasses(this Entity entity)
    {
      return entity.Classes ?? (entity.Classes = new NameCollection());
    }

    public static NameCollection WithRelations(this Entity entity)
    {
      return entity.Relations ?? (entity.Relations = new NameCollection());
    }

    public static PropertyCollection WithDataProperties(this Entity entity)
    {
      return entity.DataProperties ?? (entity.DataProperties = new PropertyCollection());
    }

    public static PropertyCollection WithMetaProperties(this Entity entity)
    {
      return entity.MetaProperties ?? (entity.MetaProperties = new PropertyCollection());
    }

    public static Collection<Entity> WithMetaEntities(this Entity entity)
    {
      return entity.Entities ?? (entity.Entities = new Collection<Entity>());
    }

    public static Collection<Link> WithLinks(this Entity entity)
    {
      return entity.Links ?? (entity.Links = new Collection<Link>());
    }
  }
}
