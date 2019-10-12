using System;
using System.Collections.Generic;
using System.Text;
using Toolset;

namespace Innkeeper.Host
{
  public interface IWebAppInfo
  {
    Guid Guid { get; }

    string Name { get; }

    string Description { get; }

    VersionInfo Version { get; }

    int Port { get; }

    string PathPrefix { get; }
  }
}
