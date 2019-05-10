using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Mvc;
using EPiServer.Core;
using Vue.Net.WebComponents;
using Vue.Net.Example.EPi.Models.Blocks;
using FluentAssertions;
using Vue.Net.Tests.EPi;
using Xunit;

namespace Vue.Net.Example.EPi.Tests
{
    public class Tests
    {
        private const string VueUrl = "https://unpkg.com/vue@2.6.10";
        private const string AppUrl = "https://vuecdndev2.azureedge.net/v-app.js";

        [Fact]
        public void GetVueConfigSettings()
        {
            VueConfig.Settings.Should().BeEquivalentTo(new
            {
                AppUrl,
                AppPrefix = "v-app",
                VueUrl,
                Components = new dynamic[]
                {
                    new { Name = "HelloWorld" },
                }
            });
        }

        [Fact]
        public void RenderVueHeadScripts()
        {
            var str = VueDotNet.RenderScriptTags(VueScriptLocation.Head);
            str.Should().BeEquivalentTo(new
            {
                Value = $"<link as=\"script\" href=\"{VueUrl}\" rel=\"preload\"></link><link as=\"script\" href=\"{AppUrl}\" rel=\"preload\"></link>"
            });
        }

        [Fact]
        public void RenderVueFootScripts()
        {
            var str = VueDotNet.RenderScriptTags(VueScriptLocation.Foot);
            str.Should().BeEquivalentTo(new 
            {
                Value = $"<script src=\"{VueUrl}\"></script><script src=\"{AppUrl}\"></script>"
            });
        }

        [Fact]
        public void RenderVueBlock()
        {
            var block = new VueBlock()
            {
                ComponentName = "HelloWorld",
                Message = "Hello EPiServer",
                SlotContent = new XhtmlString("<p>Hello</p>"),
                NamedSlots = new List<IVueNamedSlot>()
                {
                    new VueBlockNamedSlotContent
                    {
                        SlotName = "left-banner",
                        TagName = "v-app-vue-test-2"
                    }
                }
            };

            var result = block.RenderTag();
            result.Should().BeEquivalentTo(new
            {
                Value = "<v-app-hello-world msg=\"Hello EPiServer\"><p>Hello</p><v-app-vue-test-2 slot=\"left-banner\"></v-app-vue-test-2></v-app-hello-world>"
            });
        }
    }
}
