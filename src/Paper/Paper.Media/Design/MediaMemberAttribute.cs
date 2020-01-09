using System;

namespace Paper.Media.Design
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public class MediaMemberAttribute : Attribute
  {
    public string Name { get; set; }

    public int Order { get; set; }
  }
}
