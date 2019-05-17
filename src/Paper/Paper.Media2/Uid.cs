using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using Toolset;
using Toolset.Collections;

namespace Paper.Media2
{
  public class Uid
  {
    private static int nextUid;
    
    public Uid()
    {
      this.Value = Interlocked.Increment(ref nextUid);
    }

    public Uid(long value)
    {
      this.Value = value;
    }

    public long Value { get; }

    public override bool Equals(object obj)
    {
      if (obj is Uid uid)
      {
        obj = uid.Value;
      }
      else if (obj is IConvertible convertible)
      {
        obj = convertible.ToInt64(CultureInfo.InvariantCulture);
      }
      return Value.Equals(obj);
    }

    public override int GetHashCode()
    {
      return Value.GetHashCode();
    }

    public override string ToString()
    {
      return Value.ToString();
    }

    public static implicit operator long(Uid uid)
    {
      return uid.Value;
    }

    public static implicit operator Uid(long uid)
    {
      return new Uid(uid);
    }
  }
}
