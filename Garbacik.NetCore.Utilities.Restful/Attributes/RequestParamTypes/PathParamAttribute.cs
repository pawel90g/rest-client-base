using System;

namespace Garbacik.NetCore.Utilities.Restful.Attributes.RequestParamTypes;

public sealed class PathParamAttribute : Attribute
{
    public string ParamName { get; private set; }
    public PathParamAttribute(string paramName)
    {
        ParamName = paramName;
    }
}