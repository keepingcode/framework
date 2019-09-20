using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Innkeeper.Pipeline
{
  public delegate Task Renderer(Request request, Response response, NextAsync next);
}
