using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Innkeeper.Host
{
  public interface IWebApp
  {
    Guid Guid { get; }

    string Name { get; }

    string Description { get; }

    VersionInfo Version { get; }
  }
}
