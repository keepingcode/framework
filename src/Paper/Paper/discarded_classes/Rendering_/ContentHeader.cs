﻿using System;
using System.Collections.Generic;
using System.Text;
using Toolset;
using Toolset.Collections;
using Toolset.Net;

namespace Paper.Rendering
{
  public class ContentHeader
  {
    private Headers headers;

    public ContentHeader(Headers headers)
    {
      this.headers = headers;
    }

    public string Type
    {
      get => Change.Try<string>(headers[HeaderNames.ContentType]);
      set => headers[HeaderNames.ContentType] = value;
    }

    public int Length
    {
      get => Change.Try<int>(headers[HeaderNames.ContentLength]);
      set => headers[HeaderNames.ContentLength] = value.ToString();
    }

    public Encoding Encoding
    {
      get
      {
        var value = Change.Try<string>(headers[HeaderNames.ContentEncoding]);
        try
        {
          return (value != null) ? Encoding.GetEncoding(value) : Encoding.UTF8;
        }
        catch
        {
          return Encoding.UTF8;
        }
      }
      set
      {
        headers[HeaderNames.ContentEncoding] = value?.BodyName;
      }
    }

    public string Disposition
    {
      get => Change.Try<string>(headers[HeaderNames.ContentDisposition]);
      set => headers[HeaderNames.ContentDisposition] = value;
    }
  }
}