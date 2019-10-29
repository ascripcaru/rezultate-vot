using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ElectionResults.Core.Services
{
    public static class Extensions
    {
        public static string ConvertEnumToString(this Enum type)
        {
            return type
                       .GetType()
                       .GetMember(type.ToString())
                       .FirstOrDefault()
                       ?.GetCustomAttribute<DescriptionAttribute>()
                       ?.Description ?? type.ToString();
        }
    }
}
