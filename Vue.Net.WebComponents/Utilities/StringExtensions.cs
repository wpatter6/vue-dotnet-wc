using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
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

        public static string ComponentToTagName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "div";
            return !string.IsNullOrEmpty(VueConfig.Settings?.AppPrefix) ? $"{VueConfig.Settings.AppPrefix}-{value.PascalToKebabCase()}" : value.PascalToKebabCase();

        }
        public static string GetNamedSlotString(this IVueComponentWithNamedSlots vueComponent)
        {
            var result = string.Empty;
            vueComponent.NamedSlots?.ToList().ForEach(content =>
            {
                var slotTag = new TagBuilder("div");

                slotTag.MergeAttribute("slot", content.Key);

                if (content.Value != null)
                {
                    slotTag.InnerHtml += content.Value;
                }

                result += slotTag.ToString();
            });
            return result;
        }

        private static string EscapeAttr(this string str)
        {
            return HttpUtility.HtmlAttributeEncode(str);
        }

        public static (string attr, string value) GetPropWithValue(this KeyValuePair<string, object> item)
        {
            var attr = item.Key.PascalToKebabCase();
            if (!(item.Value is string valString))
            {
                attr = ":" + attr;
                valString = JsonConvert.SerializeObject(item.Value, EscapeJsonSerializerSettings);
            }

            return (attr, valString.Replace('"', '\''));
        }

        private static readonly JsonSerializerSettings EscapeJsonSerializerSettings = new JsonSerializerSettings()
        {
            StringEscapeHandling = StringEscapeHandling.EscapeHtml
        };
    }
}