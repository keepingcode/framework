﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Paper.Rendering
{
  public delegate Task Renderer(Request request, Response response, NextAsync next);
}
