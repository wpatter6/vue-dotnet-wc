﻿using System;
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

        private static readonly HttpClient Client = new HttpClient()
        {
            Timeout = new TimeSpan(0, 0, 5),
        };

        public static string GetFileHash(this string fileLocation, bool forceUpdate = false)
        {
            var location = fileLocation;
            if (FileHashes.ContainsKey(fileLocation) && !forceUpdate)
            {
                return FileHashes[fileLocation];
            }

            var fileString = string.Empty;
            var pathInfo = GetPathInfo(location);

            if(!pathInfo.isFile)
            {
                var requestTask = Client.GetStringAsync(location);

                requestTask.Wait();

                fileString = requestTask.Result;
            }
            else
            {
                if(!pathInfo.isAbsolute)
                {
                    location = VueConfig.Settings.WebRoot + location;
                }

                if(!File.Exists(location))
                {
                    FileHashes[fileLocation] = string.Empty;
                    return string.Empty;
                }

                fileString = File.ReadAllText(location);
            }

            using (var md5 = new MD5CryptoServiceProvider())
            {
                md5.ComputeHash(Encoding.UTF8.GetBytes(fileString));
                var result = string.Join(string.Empty, md5.Hash.Select(x => x.ToString("x2")));
                FileHashes[fileLocation] = result;
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