using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Paper.Media
{
  public interface IPayload
  {
    IEnumerable<string> GetClasses();

    IEnumerable<Property> GetProperties();

    IEnumerable<IPayload> GetRecords();
  }
}
