using System;
namespace Paper.Media
{
  public interface IEntityRef : IMedia
  {
    string Type { get; set; }
    string Href { get; set; }
  }
}
