﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media
{
  public interface IMediaObject
  {
    Payload ExtractPayload();
  }
}
