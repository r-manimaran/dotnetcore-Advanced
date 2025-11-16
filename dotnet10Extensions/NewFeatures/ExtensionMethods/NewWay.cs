using System;
using System.Collections.Generic;
using System.Text;

namespace NewFeatures.ExtensionMethods.New;

public static class NewWay
{
    extension(string value)
    {
        public bool IsNullOrEmpty()
        {
            return string.IsNullOrEmpty(value);
        }

        public string? Truncate(int maxLength)
        {
            if(string.IsNullOrEmpty(value) || value.Length <=maxLength)
            {
                return value;
            }
            return value.Substring(0, maxLength);
        }
    }
}
