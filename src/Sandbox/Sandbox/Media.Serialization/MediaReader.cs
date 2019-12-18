using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Toolset;
using Toolset.Collections;
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

    private void ParseNodes(ObjectModel objectNode, IExtendedCollection<INode> target)
    {
      var classes = GetStringArray(objectNode, "Class");
      foreach (var @class in classes)
      {
        target.Add(new Class(@class));
      }

      var rels = GetStringArray(objectNode, "Rel");
      foreach (var rel in rels)
      {
        target.Add(new Rel(rel));
      }

      var entities = GetObjectArray(objectNode, "Entities");
      foreach (var entity in entities)
      {
        var child = ParseMedia(entity);
        target.Add(child);
      }

      var selfProperties = ParseProperties(objectNode,
        exceptions: new[] { "Class", "Rel", "Entities", "Properties", "Links" }
      );
      target.AddMany(selfProperties);

      var @object = GetObject(objectNode, "Properties");
      if (@object != null)
      {
        var otherProperties = ParseProperties(@object);
        target.AddMany(otherProperties);
      }

      var links = GetObjectArray(objectNode, "Links");
      foreach (var link in links)
      {
        var child = ParseLink(link);
        target.Add(child);
      }
    }

    private IEnumerable<Property> ParseProperties(ObjectModel objectModel, params string[] exceptions)
    {
      foreach (var propertyModel in objectModel.ChildProperties())
      {
        if (propertyModel.Name.EqualsAnyIgnoreCase(exceptions))
          continue;

        var nodeModel = propertyModel.Value;
        if (nodeModel == null)
          continue;

        var propertyName = propertyModel.Name;
        var propertyValue = ParseValue(nodeModel);

        yield return new Property(propertyName, propertyValue);
      }
    }

    private IValue ParseValue(NodeModel nodeModel)
    {
      if (nodeModel is ValueModel valueModel)
      {
        return Value.Create(valueModel.Value);
      }

      if (nodeModel is CollectionModel collectionModel)
      {
        var values = collectionModel.Children().Select(ParseValue);
        return Value.CreateArray(values);
      }

      if (nodeModel is ObjectModel objectModel)
      {
        var properties = ParseProperties(objectModel);
        return Value.CreateObject(properties);
      }

      throw new NotSupportedException($"Elemento inválido para esta posição: {nodeModel.GetType().FullName}");
    }

    private IMedia ParseMedia(NodeModel nodeModel)
    {
      var objectModel = nodeModel as ObjectModel
        ?? (nodeModel as DocumentModel)?.Root as ObjectModel;

      if (objectModel == null)
        return null;

      IMedia media = CreateMedia(nodeModel);
      ParseNodes(objectModel, media);
      return media;
    }

    private Link ParseLink(ObjectModel node)
    {
      var link = new Link();
      ParseNodes(node, link);
      return link;
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
      var text = (
        from property in node.ChildProperties()
        where property.Name.EqualsIgnoreCase(propertyName)
        let value = property.Value as ValueModel
        where value != null
        select value.Value as string
      ).FirstOrDefault();
      return text;
    }

    private ObjectModel GetObject(NodeModel node, string propertyName)
    {
      var @object = (
        from property in node.ChildProperties()
        where property.Name.EqualsIgnoreCase(propertyName)
        let value = property.Value as ObjectModel
        where value != null
        select value
      ).FirstOrDefault();
      return @object;
    }

    private string[] GetStringArray(NodeModel node, string propertyName)
    {
      var classes = (
        from property in node.ChildProperties()
        where property.Name.EqualsIgnoreCase(propertyName)
        let values = property.Value as CollectionModel
        where values != null
        from value in values.ChildValues()
        select value.Value as string
      ).ToArray();
      return classes;
    }

    private ObjectModel[] GetObjectArray(NodeModel node, string propertyName)
    {
      var objects = (
        from property in node.ChildProperties()
        where property.Name.EqualsIgnoreCase(propertyName)
        let values = property.Value as CollectionModel
        where values != null
        from value in values.ChildObjects()
        select value
      ).ToArray();
      return objects;
    }
  }
}
