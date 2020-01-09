using Paper.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Sandbox.Host.Forms
{
  public static class UserForm
  {
    public class Create
    {
      public string Login { get; set; }
      public string Name { get; set; }
      public string User { get; set; }
    }

    public class Edit
    {
      public int Id { get; set; }
      public string Login { get; set; }
      public string Name { get; set; }
      public string User { get; set; }
      public bool? Enabled { get; set; }
    }
  }
}
