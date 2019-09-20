using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Toolset.Collections;

namespace Innkeeper.Pipelines
{
  public interface IResponse
  {
    IHeaders Headers { get; }

    HttpStatusCode Status { get; set; }

    Stream Body { get; }
  }
}