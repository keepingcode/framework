﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Toolset.Collections;

namespace Paper.Rendering
{
  public interface IHttpResponse
  {
    Headers Headers { get; }

    Stream Body { get; }

    HttpStatusCode Status { get; set; }
  }
}