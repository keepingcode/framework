using System;
using System.Collections.Generic;
using System.Text;

namespace Innkeeper.Pipelines
{
  public static class ObjectFactoryBuilderExtensions
  {
    public static IObjectFactoryBuilder AddSingleton<TContract>(this IObjectFactoryBuilder builder)
      where TContract : new()
    {
      builder.AddSingleton(typeof(TContract), new TContract());
      return builder;
    }

    public static IObjectFactoryBuilder AddSingleton<TContract>(this IObjectFactoryBuilder builder, TContract instance)
    {
      builder.AddSingleton(typeof(TContract), instance);
      return builder;
    }
  }
}
