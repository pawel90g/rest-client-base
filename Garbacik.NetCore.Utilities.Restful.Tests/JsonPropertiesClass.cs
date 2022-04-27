using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Garbacik.NetCore.Utilities.Restful.Tests;

internal class JsonPropertiesClass
{
    [JsonProperty("propertyInt")]
    public int PropertyInt { get; set; }
    [JsonProperty("propertyString")]
    public string PropertyString { get; set; }
}