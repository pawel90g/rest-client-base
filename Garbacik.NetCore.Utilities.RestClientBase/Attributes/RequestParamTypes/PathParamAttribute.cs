using System;

namespace Garbacik.NetCore.Utilities.RestClientBase.Attributes.RequestParamTypes
{
    public sealed class PathParamAttribute : Attribute
    {
        public string ParamName { get; private set; }
        public PathParamAttribute(string paramName)
        {
            ParamName = paramName;
        }
    }
}
