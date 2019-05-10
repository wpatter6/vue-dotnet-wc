using System;
using Vue.Net.WebComponents.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Html;

namespace Vue.Net.WebComponents
{
    public static class VueDotNet
    {
        /// <summary>
        /// Renders the IVueComponent to a HtmlString which can be used inside an MVC view.
        /// </summary>
        /// <param name="vueBlock">The IVueComponent instance to render.</param>
        /// <returns></returns>
        public static HtmlString RenderTag(this IVueComponent vueBlock)
        {
            var tagName = vueBlock.ComponentName.ComponentToTagName();
            var outerElement = new TagBuilder(tagName)
            {
                InnerHtml = ""
            };

            if (vueBlock.Props.Count > 0)
            {
                foreach(var item in vueBlock.Props)
                {
                    var attr = item.Key.PascalToKebabCase();
                    if (!(item.Value is string valString))
                    {
                        attr = ":" + attr;
                        valString = JsonConvert.SerializeObject(item.Value);
                    }

                    outerElement.MergeAttribute(attr, valString);
                }
            }

            if (vueBlock.SlotHtml != null)
            {
                outerElement.InnerHtml += vueBlock.SlotHtml;
            }

            vueBlock.NamedSlots?.ToList().ForEach(content =>
            {
                var slotTag = new TagBuilder(string.IsNullOrEmpty(content.TagName) ? "div" : content.TagName);

                slotTag.MergeAttribute("slot", content.SlotName);

                if (content.ContentHtml != null)
                {
                    slotTag.InnerHtml += content.ContentHtml;
                }

                outerElement.InnerHtml += slotTag.ToString();
            });

            return new HtmlString(outerElement.ToString());
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

            return new HtmlString($"{vueLink}{appLink}");
        }
        private static TagBuilder GetStaticElement(VueScriptLocation location, string url)
        {
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