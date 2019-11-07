using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Toolset;
using Toolset.Collections;

namespace Innkeeper.Host
{
  public interface IRequest
  {
    string RequestUri { get; }

    string RequestPath { get; }

    string PathBase { get; }

    string Path { get; }

    string Method { get; }

    IHeaders Headers { get; }

    IArgs QueryArgs { get; }

    Stream Body { get; }

    IRequestContext GetContext();

    void SetBody(Func<Stream, Stream> bodyMaker);
  }
}
