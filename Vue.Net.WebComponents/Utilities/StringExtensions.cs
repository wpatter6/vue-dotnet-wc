using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Vue.Net.WebComponents.Utilities
{
    internal static class StringExtensions
    {
        public static string PascalToKebabCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return Regex.Replace(
                    value,
                    "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                    "-$1",
                    RegexOptions.Compiled)
                .Trim()
                .ToLower();
        }

        public static string ToProps(this string value)
        {
            var propObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(value);
            var result = "";
            foreach(var item in propObj)
            {
                var attr = item.Key.PascalToKebabCase();
                if (!(item.Value is string valString))
                {
                    attr = ":" + attr;
                    valString = JsonConvert.SerializeObject(item.Value);
                }

                result += $"{attr}=\"{valString}\"";
            }
            return result;
        }

        public static string ComponentToTagName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "div";
            return !string.IsNullOrEmpty(VueConfig.Settings?.AppPrefix) ? $"{VueConfig.Settings.AppPrefix}-{value.PascalToKebabCase()}" : value.PascalToKebabCase();

        }
    }
}