using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Garbacik.NetCore.Utilities.Restful.Tests;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
internal class JsonObjectAnnotationClass
{
    public int PropertyInt { get; set; }
    public string PropertyString { get; set; }
}