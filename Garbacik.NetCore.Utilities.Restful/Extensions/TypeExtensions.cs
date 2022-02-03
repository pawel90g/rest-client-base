using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Garbacik.NetCore.Utilities.Restful.Extensions;

public static class TypeExtensions
{
    public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<T>(this Type type)
    {
        return type.GetProperties()
            .Where(prop => prop.GetCustomAttributes(true)
                .Any(attr => attr.GetType() == typeof(T)));
    }
}