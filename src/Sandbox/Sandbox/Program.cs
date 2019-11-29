using Innkeeper.Rest;
using Paper.Media;
using Paper.Media.Design;
using Paper.Rendering.Design;
using System;
using Toolset;
using Toolset.Xml;

namespace Sandbox
{
  public static class Program
  {
    public static void Main(string[] commandArgs)
    {
      try
      {
        //
        // Construindo 
        //

        var factory = PaperBuilderFactory.Create();
        var builder = factory.CreatePaperBuilder(
          new PaperInfo
          {
            Catalog = "MyCatalog",
            Name = "MyName",
            Title = "My Paper",
            Description = "This is my sample paper"
          },
          ctx => new { Name = "MyPaper", Title = "My Paper" }
        );

        var get = builder.Get((ctx, host) => new { Id = 10, Login = "user", Name = "My User" });

        var userGetter = get.PopulateRecord((ctx, host, data) => data);
        var postGetter = get.PopulateRecords(userGetter, (ctx, host, data, user) => new[]
        {
          new { Id = 1, UserId = user.Id, Title = "My 1st Post", Body = "Lorem ipsum dolor sit ament." },
          new { Id = 2, UserId = user.Id, Title = "My 2nd Post", Body = "Lorem ipsum dolor sit ament." }
        });

        get.FormatEntity(userGetter, (ctx, host, data, user, entity) =>
        {
          entity.WithClass().Add("MySampleUser");
        });

        get.FormatEntity(postGetter, (ctx, host, data, allPosts, post, entity) =>
        {
          entity.WithClass().Add("MySamplePost");
          entity.WithProperties().Add("PostCount", allPosts.Count);
        });

        var blueprint = builder.BuildPaper();

        //
        // Formatando 
        //
        var writer = new MediaDataWriter<Payload>();

        var context = new PaperContext();
        context.Verb = VerbNames.Get;
        context.OutgoingData = writer;

        blueprint.RenderPaper(context);

        var result = writer.Result;

        Console.WriteLine(result?.ToXElement());
      }
      catch (Exception ex)
      {
        ex.Trace();
      }
    }

  }
}