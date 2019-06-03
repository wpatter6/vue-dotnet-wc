using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Vue.Net.WebComponents.Utilities
{
    internal static class Extensions
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

        private static HttpClient Client = new HttpClient();

        public static async Task<string> GetFileHash(this string fileLocation)
        {
            if (FileHashes.ContainsKey(fileLocation))
            {
                return FileHashes[fileLocation];
            }

            var fileString = string.Empty;
            var pathInfo = GetPathInfo(fileLocation);

            if(!pathInfo.isFile)
            {
                fileString = await Client.GetStringAsync(fileLocation);
            }
            else
            {
                if(!pathInfo.isAbsolute)
                {
                    if(string.IsNullOrEmpty(VueConfig.Settings.WebRoot))
                    {
                        FileHashes.Add(fileLocation, string.Empty);
                        return string.Empty;
                    }

                    fileLocation = VueConfig.Settings.WebRoot + fileLocation;
                }

                fileString = File.ReadAllText(fileLocation);
            }

            using (var md5 = new MD5CryptoServiceProvider())
            {
                md5.ComputeHash(Encoding.UTF8.GetBytes(fileString));
                var result = string.Join(string.Empty, md5.Hash.Select(x => x.ToString("x2")));
                FileHashes.Add(fileLocation, result);
                return result;
            }
        }

        public static (bool isAbsolute, bool isFile) GetPathInfo(string test)
        {
            Uri u;
            try
            {
                u = new Uri(test);
            }
            catch
            {
                return (false, true);
            }

            return (u.IsAbsoluteUri, u.IsFile);
        } 

        private static Dictionary<string, string> FileHashes = new Dictionary<string, string>();
    }
}