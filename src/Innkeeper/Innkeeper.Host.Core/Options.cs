using System.Collections.Generic;

namespace Innkeeper.Host.Core
{
  public class Options
  {
    private int _port;

    public int Port
    {
      get => _port == 0 ? 90 : _port;
      set => _port = value;
    }
    public List<string> Prefixes { get; } = new List<string>();
  }
}