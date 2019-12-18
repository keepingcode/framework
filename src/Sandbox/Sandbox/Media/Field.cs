using System;
using Toolset.Collections;

namespace Paper.Media
{
  public class Field : Entity
  {
    public Field()
    {
      this.Add(Class.Field);
    }

    public string Name { get => Get<string>(); set => Set(value); }
    public string Type { get => Get<string>(); set => Set(value); }
    public override string Title { get => Get<string>(); set => Set(value); }
    public string Value { get => Get<string>(); set => Set(value); }
  }
}
