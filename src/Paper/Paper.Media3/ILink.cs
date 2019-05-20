namespace Paper.Media3
{
  public interface ILink
  {
    string Title { get; set; }

    NameCollection Rel { get; set; }

    Href Href { get; set; }

    string Type { get; set; }
  }
}