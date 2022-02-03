using System.Collections.Generic;

namespace Garbacik.NetCore.Utilities.Restful.Extensions;

public static class StringExtensions
{
    public static string PrepareString(this string source, Dictionary<string, string> replaceParts)
    {
        foreach (KeyValuePair<string, string> pair in replaceParts)
        {
            source = pair.Key.StartsWith("{") && pair.Key.EndsWith("}") ?
                source.Replace(pair.Key, pair.Value) :
                source.Replace($"{{{pair.Key}}}", pair.Value);
        }

        return source;
    }
}