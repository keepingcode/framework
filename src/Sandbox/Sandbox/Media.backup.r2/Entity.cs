using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolset.Collections;

namespace Paper.Media
{
  public class Entity : IPayload
  {
    public string Title { get; set; }

    public NameCollection Classes { get; set; }

    public NameCollection Relations { get; set; }

    public PropertyCollection DataProperties { get; set; }

    public PropertyCollection MetaProperties { get; set; }

    public Collection<Entity> Entities { get; set; }

    public Collection<Link> Links { get; set; }

    #region Implementação de IPayload

    IEnumerable<string> IPayload.GetClasses()
      => Classes?.Where(x => char.IsUpper(x.FirstOrDefault())) ?? Enumerable.Empty<string>();

    IEnumerable<Property> IPayload.GetProperties()
      => DataProperties ?? Enumerable.Empty<Property>();

    IEnumerable<IPayload> IPayload.GetRecords()
      => Entities?.Where(x => x.Classes?.Contains(ClassNames.Record) == true)
      ?? Enumerable.Empty<IPayload>();

    #endregion
  }
}
