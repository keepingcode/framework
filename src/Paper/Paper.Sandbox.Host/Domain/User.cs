using System;
using System.Collections.Generic;
using System.Text;

namespace Paper.Sandbox.Host.Domain
{
  public class User
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Login { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }
  }
}
