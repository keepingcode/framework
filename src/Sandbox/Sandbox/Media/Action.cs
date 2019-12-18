using System;
using Toolset.Collections;

namespace Paper.Media
{
  public class Action : Entity
  {
    public Action()
    {
      this.Add(Class.Action);
    }

    public string Name { get => Get<string>(); set => Set(value); }
    public string Method { get => Get<string>(); set => Set(value); }
    public string Type { get => Get<string>(); set => Set(value); }
    public string Href { get => Get<string>(); set => Set(value); }
    public override string Title { get => Get<string>(); set => Set(value); }
  }
}
