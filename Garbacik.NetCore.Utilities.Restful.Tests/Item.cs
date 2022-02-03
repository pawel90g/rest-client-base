using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Garbacik.NetCore.Utilities.Restful.Tests;

[Serializable()]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "https://garbacik.eu")]
public partial class Item
{
    [XmlAttribute("name")]
    public string Name { get; set; }
}