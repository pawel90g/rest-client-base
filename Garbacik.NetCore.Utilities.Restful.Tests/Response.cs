using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Garbacik.NetCore.Utilities.Restful.Tests;

[Serializable()]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "https://garbacik.eu")]
[XmlRoot(Namespace = "https://garbacik.eu", IsNullable = false, ElementName = "response")]
public partial class Response
{
    [XmlAttribute("id")]
    public int Id { get; set; }

    [XmlArray("items")]
    [XmlArrayItem("item")]
    public List<Item> Items { get; set; }

    [XmlArray("values")]
    [XmlArrayItem("value")]
    public List<string> Values { get; set; }
}