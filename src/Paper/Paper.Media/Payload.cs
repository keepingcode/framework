using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Paper.Media.Design;
using Paper.Media.Serialization;
using Toolset;
using Toolset.Collections;
using Toolset.Reflection;

namespace Paper.Media
{
  [DataContract(Namespace = Namespaces.Default, Name = "Payload")]
  public class Payload : IMediaObject
  {
    [DataMember]
    public PropertyMap Error { get; set; }

    [DataMember]
    public PropertyMap Form { get; set; }

    [DataMember]
    public PropertyMap Record { get; set; }

    [DataMember]
    public RecordCollection Records { get; set; }

    Payload IMediaObject.GetPayload() => this;

    [CollectionDataContract(Namespace = Namespaces.Default, Name = "Records", ItemName = "Record")]
    public class RecordCollection : List<PropertyMap>
    {
    }
  }
}