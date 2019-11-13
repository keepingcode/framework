using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Toolset.Collections;

namespace Innkeeper.Host
{
  public interface IResponse
  {
    IHeaders Headers { get; }

    HttpStatusCode Status { get; set; }

    Stream Body { get; }

    IRequestContext GetContext();

    void SetBody(Func<Stream, Stream> bodyMaker);
  }
}