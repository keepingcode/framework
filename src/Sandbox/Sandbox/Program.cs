using Paper.Media;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Toolset;
using Toolset.Collections;
using Toolset.Xml;

namespace Sandbox
{
  class Talz
  {
    [XmlElement("MyID")]
    public string Id { get; set; }

    [XmlElement("MyTelz")]
    public Telz Telz { get; set; }
  }

  [DataContract]
  class Telz
  {
    [DataMember(Name = "MyIDs")]
    public string[] Ids { get; set; }
  }

  public static class Program
  {
    public static void Main(string[] commandArgs)
    {
      try
      {
        var talz = new Talz
        {
          Id = "Ten",
          Telz = new Telz
          {
            Ids = new[] { "One", "Two" }
          }
        };

        var entity = new Entity();
        entity.Title = "My Test";
        entity.Add((Class)"record");
        entity.Add((Class)"action");
        entity.Add((Class)"Talz");
        entity.AddMany(PropertyCollection.CreateFrom(talz));
        entity.Add(entity);

        foreach (var @class in entity.UserClasses())
        {
          Debug.WriteLine($"user-class: {@class.Name}");
        }
        foreach (var @class in entity.MetaClasses())
        {
          Debug.WriteLine($"meta-class: {@class.Name}");
        }
        foreach (var prop in entity.UserProperties())
        {
          Debug.WriteLine($"user-prop: {prop.Name} = {prop.Value}");
        }
        foreach (var prop in entity.MetaProperties())
        {
          Debug.WriteLine($"meta-prop: {prop.Name} = {prop.Value}");
        }
        foreach (var record in entity.Records())
        {
          Debug.WriteLine($"record: {record.GetType().FullName}");
        }
        foreach (var action in entity.Actions())
        {
          Debug.WriteLine($"action: {action.GetType().FullName}");
        }
        foreach (var metaEntity in entity.MetaEntities())
        {
          Debug.WriteLine($"meta-entity: {metaEntity.GetType().FullName}");
        }

        //var props = Value.Create(talz) as PropertyCollection;
        //var graph = props.CopyTo<Talz>();
        //
        //Debug.WriteLine(graph);
      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }

    //  private static void TestPaper()
    //  {
    //    //
    //    // Construindo 
    //    //

    //    var factory = PaperBuilderFactory.Create();
    //    var builder = factory.CreatePaperBuilder(
    //      new PaperInfo
    //      {
    //        Catalog = "MyCatalog",
    //        Name = "MyName",
    //        Title = "My Paper",
    //        Description = "This is my sample paper"
    //      },
    //      ctx => new { Name = "MyPaper", Title = "My Paper" }
    //    );

    //    var get = builder.Get((ctx, host) => new { Id = 10, Login = "user", Name = "My User" });

    //    var userGetter = get.PopulateRecord((ctx, host, data) => data);
    //    var postGetter = get.PopulateRecords(userGetter, (ctx, host, data, user) => new[]
    //    {
    //        new { Id = 1, UserId = user.Id, Title = "My 1st Post", Body = "Lorem ipsum dolor sit ament." },
    //        new { Id = 2, UserId = user.Id, Title = "My 2nd Post", Body = "Lorem ipsum dolor sit ament." }
    //      });

    //    get.FormatEntity(userGetter, (ctx, host, data, user, entity) =>
    //    {
    //      entity.WithClass().Add("MySampleUser");
    //    });

    //    get.FormatEntity(postGetter, (ctx, host, data, allPosts, post, entity) =>
    //    {
    //      entity.WithClass().Add("MySamplePost");
    //      entity.WithProperties().Add("PostCount", allPosts.Count);
    //    });

    //    var blueprint = builder.BuildPaper();

    //    //
    //    // Formatando 
    //    //
    //    var writer = new MediaDataWriter<Payload>();

    //    var context = new PaperContext();
    //    context.Verb = VerbNames.Get;
    //    context.OutgoingData = writer;

    //    blueprint.RenderPaper(context);

    //    var result = writer.Result;

    //    Console.WriteLine(result?.ToXElement());
    //  }
  }
}