using Paper.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Sandbox.Host.Demo.Forms
{
  public static class UserForm
  {
    public class Create
    {
      public string Login { get; set; }
      public string Name { get; set; }
      public int GroupId { get; set; }
      public bool Enabled { get; set; } = true;
    }

    public class BulkEdit
    {
      public string Name { get; set; }
      public int? GroupId { get; set; }
      public bool? Enabled { get; set; }
    }

    public class MassEdit
    {
      public int? GroupId { get; set; }
      public bool? Enabled { get; set; }
    }
  }
}
