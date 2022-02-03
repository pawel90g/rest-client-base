using Garbacik.NetCore.Utilities.Restful.Consts;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers;
using System.IO;

namespace Garbacik.NetCore.Utilities.Restful.Tools;

public sealed class NewtonsoftJsonSerializer : ISerializer, IDeserializer
{
    public string ContentType { 
        get => ContentTypes.JSON;
        set { }
    }

    private readonly JsonSerializer serializer;

    public NewtonsoftJsonSerializer(JsonSerializer serializer)
    {
        this.serializer = serializer;
    }

    public T? Deserialize<T>(RestResponse response)
    {
        var content = response.Content;

        using var stringReader = new StringReader(content);
        using var jsonTextReader = new JsonTextReader(stringReader);
        return serializer.Deserialize<T>(jsonTextReader);
    }

    public string Serialize(object obj)
    {
        using var stringWriter = new StringWriter();
        using var jsonTextWriter = new JsonTextWriter(stringWriter);
        serializer.Serialize(jsonTextWriter, obj);

        return stringWriter.ToString();
    }

    public static NewtonsoftJsonSerializer Default =>
        new(new JsonSerializer()
        {
            NullValueHandling = NullValueHandling.Ignore,
        });
}