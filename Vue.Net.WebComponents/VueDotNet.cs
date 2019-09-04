using System;
using Vue.Net.WebComponents.Utilities;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNetCore.Html;
using System.Threading.Tasks;
using System.Linq;

namespace Vue.Net.WebComponents
{
    public static class VueDotNet
    {
        /// <summary>
        /// Renders the IVueComponent to a HtmlString which can be used inside an MVC view.
        /// </summary>
        /// <param name="vueComponent">The IVueComponent instance to render.</param>
        /// <returns></returns>
        public static HtmlString RenderComponent(this IVueComponent vueComponent)
        {
            var tagName = vueComponent.ComponentName.ComponentToTagName();
            var outerElement = new TagBuilder(tagName)
            {
                InnerHtml = ""
            };

            if (vueComponent.Props.Count > 0)
            {
                foreach(var item in vueComponent.Props)
                {
                    var (attr, value) = item.GetPropWithValue();

                    outerElement.MergeAttribute(attr, value);
                }
            }

            if (vueComponent.SlotHtml != null)
            {
                outerElement.InnerHtml += vueComponent.SlotHtml;
            }

            outerElement.InnerHtml += vueComponent.GetNamedSlotString();

            return new HtmlString(outerElement.ToString());
        }

        public static HtmlString ToWebComponentTagName(this string value)
        {
            return new HtmlString(value.ComponentToTagName());
        }

        public static HtmlString RenderComponentProps(this IDictionary<string, object> dictionary)
        {
            var result = string.Empty;

            foreach (var item in dictionary)
            {
                var (attr, value) = item.GetPropWithValue();

                if (!string.IsNullOrEmpty(result))
                {
                    result += " ";
                }

                if (value != null)
                {
                    result += $"{attr}=\"{value}\"";
                }
            }
            return new HtmlString(result);
        }

        /// <summary>
        /// Renders the HtmlString of props for an IVueComponentWithProps
        /// </summary>
        /// <param name="vueComponent"></param>
        /// <returns></returns>
        public static HtmlString RenderComponentProps(this IVueComponentWithProps vueComponent)
        {
            return vueComponent.Props.RenderComponentProps();
        }

        /// <summary>
        /// Renders the HtmlString of a default slot for an IVueComponentWithProps
        /// </summary>
        /// <param name="vueComponent"></param>
        /// <returns></returns>
        public static HtmlString RenderComponentDefaultSlot(this IVueComponentWithDefaultSlot vueComponent)
        {
            return new HtmlString(vueComponent.SlotHtml);
        }

        /// <summary>
        /// Renders the HtmlString for a set of named slots in an IVueComponentWithNamedSlots
        /// </summary>
        /// <param name="vueComponent"></param>
        /// <returns></returns>
        public static HtmlString RenderComponentNamedSlots(this IVueComponentWithNamedSlots vueComponent)
        {
            return new HtmlString(vueComponent.GetNamedSlotString());
        }
        public static string GetNamedSlotString(this IDictionary<string, string> dictionary)
        {
            var result = string.Empty;
            dictionary?.ToList().ForEach(content =>
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
        public static string GetNamedSlotString(this IVueComponentWithNamedSlots vueComponent)
        {
            return vueComponent.NamedSlots.GetNamedSlotString();
        }

        /// <summary>
        /// Renders the script tags using the appUrl and vueUrl values in the application configuration file.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="location">The location of the script tag.  Use Head to render preload links, Foot to render the actual script tags.</param>
        /// <returns></returns>
        public static HtmlString RenderScriptTags(VueScriptLocation location = VueScriptLocation.Foot)
        {
            var vueLink = GetStaticElement(location, VueConfig.Settings.VueUrl);
            var appLink = GetStaticElement(location, VueConfig.Settings.AppUrl);
            var result = $"{vueLink}{appLink}";
            foreach (var script in VueConfig.Settings.Scripts)
            {
                result += GetStaticElement(location, script.Url, !script.NoHash);
            }

            return new HtmlString(result);
        }

        public static void GetScriptHashes()
        {
            foreach(var script in VueConfig.Settings.Scripts.Where(script => script.NoHash))
            {
                script.Url.GetFileHash(true);
            }
        }

        private static TagBuilder GetStaticElement(VueScriptLocation location, string url, bool cacheBust = false)
        {
            if(string.IsNullOrEmpty(url))
            {
                return null;
            }

            if(VueConfig.Settings.CacheBust && cacheBust)
            {
                var hash = url.GetFileHash();
                if(!string.IsNullOrEmpty(hash))
                {
                    if (url.Contains("?"))
                    {
                        url += "&";
                    }
                    else
                    {
                        url += "?";
                    }

                    url += hash;
                }
            }

            switch (location)
            {
                case VueScriptLocation.Head:
                    var headTag = new TagBuilder("link");
                    headTag.MergeAttributes(new Dictionary<string, string>()
                    {
                        {"rel", "preload" },
                        {"as", "script" },
                        {"href", url }
                    });
                    return headTag;
                case VueScriptLocation.Foot:
                    var bodyTag = new TagBuilder("script");
                    
                    bodyTag.MergeAttributes(new Dictionary<string, string>()
                    {
                        {"src", url }
                    });
                    return bodyTag;
                default:
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
            }
        }
    }

    public enum VueScriptLocation
    {
        Head,
        Foot
    }
}