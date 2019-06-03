using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Mvc;
using EPiServer.Core;
using Vue.Net.WebComponents;
using Vue.Net.Example.EPi.Models.Blocks;
using FluentAssertions;
using Moq;
using Vue.Net.Tests.EPi;
using Xunit;

namespace Vue.Net.Example.EPi.Tests
{
    public class Tests
    {
        private const string VueUrl = "https://unpkg.com/vue@2.6.10";
        private const string AppUrl = "/Scripts/dist/v-app.js";

        [Fact]
        public void GetVueConfigSettings()
        {
            VueConfig.Settings.Should().BeEquivalentTo(new
            {
                AppPrefix = "v-app",
                Components = new dynamic[]
                {
                    new { Name = "HelloWorld" },
                },
                Scripts = new dynamic[]
                {
                    new { Url = VueUrl },
                    new { Url = AppUrl }
                }
            });
        }

        [Fact]
        public void RenderVueHeadScripts()
        {
            var str = VueDotNet.RenderScriptTags(VueScriptLocation.Head);
            str.Should().BeEquivalentTo(new
            {
                Value = $"<link as=\"script\" href=\"{VueUrl}?d5c38adb09ff79efa1c4d0745dfd308c\" rel=\"preload\"></link><link as=\"script\" href=\"{AppUrl}?d666fb4b769d4533f3d3b1f76475c605\" rel=\"preload\"></link>"
            });
        }

        [Fact]
        public void RenderVueFootScripts()
        {
            var str = VueDotNet.RenderScriptTags(VueScriptLocation.Foot);
            str.Should().BeEquivalentTo(new 
            {
                Value = $"<script src=\"{VueUrl}?d5c38adb09ff79efa1c4d0745dfd308c\"></script><script src=\"{AppUrl}?d666fb4b769d4533f3d3b1f76475c605\"></script>"
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
                NamedSlots = new Dictionary<string, string>
                {
                    { "left-banner", "<v-app-vue-test-2 />" }
                }
            };

            var result = block.RenderComponent();
            result.Should().BeEquivalentTo(new
            {
                Value = "<v-app-hello-world msg=\"Hello EPiServer\"><p>Hello</p><div slot=\"left-banner\"><v-app-vue-test-2 /></div></v-app-hello-world>"
            });
        }


        [Fact]
        public void RenderVueComponentProps()
        {
            var component = new Mock<IVueComponentWithProps>();
            component.Setup(m => m.Props).Returns(new Dictionary<string, object>()
            {
                { "abc", "def" },
                { "hij", new { name = "klm" } }
            });

            var str = component.Object.RenderComponentProps();
            str.Should().BeEquivalentTo(new
            {
                Value = "abc=\"def\" :hij=\"{'name':'klm'}\""
            });
        }

        [Fact]
        public void RenderVueComponentDefaultSlot()
        {
            var slotHtml = "<div>This is a slot!</div>";
            var component = new Mock<IVueComponentWithDefaultSlot>();
            component.Setup(m => m.SlotHtml).Returns(slotHtml);

            var str = component.Object.RenderComponentDefaultSlot();
            str.Should().BeEquivalentTo(new
            {
                Value = slotHtml
            });
        }

        [Fact]
        public void RenderVueComponentNamedSlots()
        {
            var component = new Mock<IVueComponentWithNamedSlots>();
            component.Setup(m => m.NamedSlots).Returns(new Dictionary<string, string>
            {
                {"slot1", "<div>This is slot 1.</div>" },

                {"slot2", "<div>This is the second slot.</div>" },
            });

            var str = component.Object.RenderComponentNamedSlots();
            str.Should().BeEquivalentTo(new
            {
                Value = "<div slot=\"slot1\"><div>This is slot 1.</div></div><div slot=\"slot2\"><div>This is the second slot.</div></div>"
            });
        }

        [Fact]
        public void ComponentToTagName()
        {
            var tagName = "CoolComponentTagName";
            var str = tagName.ToWebComponentTagName();
            str.Should().BeEquivalentTo(new
            {
                Value = $"v-app-cool-component-tag-name"
            });
        }
    }
}
