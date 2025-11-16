using System;
using System.Collections.Generic;
using System.Text;

namespace NewFeatures.ExtensionMethods.Old;

public static class OldWay
{
    public static bool IsNullOrEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value);
    }

    public static string? Truncate(this string? value, int maxLength)
    {
        if(string.IsNullOrEmpty(value) || value.Length <=maxLength)
        {
            return value;
        }
        return value.Substring(0, maxLength);
    }
}
