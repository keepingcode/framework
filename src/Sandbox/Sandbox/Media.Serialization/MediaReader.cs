using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Toolset;
using Toolset.Serialization;

namespace Paper.Media.Serialization
{
  public class MediaReader
  {
    public ICollection<IMedia> Read(Reader reader)
    {
      var writer = new DocumentWriter();
      reader.CopyTo(writer);

      var documents = writer.TargetDocuments;
      var entities = documents.Select(ParseMedia).ToArray();
      return entities;
    }

    private IMedia ParseMedia(NodeModel node)
    {
      IMedia media = CreateMedia(node);

      var classes = GetStringArray(node, "Class");
      foreach (var @class in classes)
      {
        media.Add(new Class(@class));
      }

      var rels = GetStringArray(node, "Rel");
      foreach (var rel in rels)
      {
        media.Add(new Rel(rel));
      }

      var entities = GetObjectArray(node, "Entities");
      foreach (var entity in entities)
      {
        var child = ParseMedia(entity);
        media.Add(child);
      }

      return media;
    }

    private IMedia CreateMedia(NodeModel node)
    {
      var @class = GetStringArray(node, "Class");

      var href = GetString(node, "Href");
      var isRef = href != null;

      if (@class.Contains(Class.Field.Name))
        return isRef ? new FieldRef() : (IMedia)new Field();

      if (@class.Contains(Class.Action.Name))
        return isRef ? new ActionRef() : (IMedia)new Action();

      if (@class.Contains(Class.Record.Name))
        return isRef ? new RecordRef() : (IMedia)new Record();

      if (@class.Contains(Class.Resource.Name))
        return isRef ? new ResourceRef() : throw new HttpException(HttpStatusCode.BadRequest, "Um recurso deve ser indicado como referência e não como entidade.");

      return isRef ? new EntityRef() : (IMedia)new Entity();
    }

    private string GetString(NodeModel node, string propertyName)
    {
      var href = (
        from property in node.ChildProperties()
        where property.Name.EqualsIgnoreCase(propertyName)
        from value in property.ChildValues()
        select value.Value as string
      ).FirstOrDefault();
      return href;
    }

    private string[] GetStringArray(NodeModel node, string propertyName)
    {
      var classes = (
        from property in node.ChildProperties()
        where property.Name.EqualsIgnoreCase(propertyName)
        from value in property.ChildValues()
        select value.Value as string
      ).ToArray();
      return classes;
    }

    private ObjectModel[] GetObjectArray(NodeModel node, string propertyName)
    {
      var classes = (
        from property in node.ChildProperties()
        where property.Name.EqualsIgnoreCase(propertyName)
        from value in property.ChildObjects()
        select value
      ).ToArray();
      return classes;
    }
  }
}
