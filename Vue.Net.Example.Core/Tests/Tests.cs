using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using Vue.Net.WebComponents;
using Vue.Net.Example.Core.Models;
using Moq;

namespace Vue.Net.Example.Core.Tests
{
    public class Tests
    {
        private const string VueUrl = "https://unpkg.com/vue@2.6.10";
        private const string AppUrl = "/js/dist/my-vue.js";

        public Tests()
        {
            new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()).CreateClient();
        }

        [Fact]
        public void GetVueConfigSettings()
        {
            VueConfig.Settings.Should().BeEquivalentTo(new
            {
                AppPrefix = "my-vue",
                Components = new dynamic[]
                {
                    new { Name = "Home" },
                },
                Scripts = new dynamic[]
                {
                    new { Url = VueUrl },
                    new { Url = AppUrl }
                }
            });
        }

        [Fact]
        public void RenderVueComponent()
        {
            var component = new VueComponent()
            {
                ComponentName = "HelloWorld",
                Message = "Hello Core"
            };

            var str = component.RenderComponent();
            str.Should().BeEquivalentTo(new
            {
                Value = "<my-vue-hello-world msg=\"Hello Core\"></my-vue-hello-world>"
            });
        }

        [Fact]
        public void RenderVueComponentWithBlanks()
        {
            var component = new VueComponent()
            {
                ComponentName = "HelloWorld",
                Message = ""
            };

            var str = component.RenderComponent();
            str.Should().BeEquivalentTo(new
            {
                Value = "<my-vue-hello-world></my-vue-hello-world>"
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
        public void RenderVueComponentPropsWithBlanks()
        {
            var component = new Mock<IVueComponentWithProps>();
            component.Setup(m => m.Props).Returns(new Dictionary<string, object>()
            {
                { "abc", "" },
                { "hij", new { name = "klm" } },
                { "ghi", "" }
            });

            var str = component.Object.RenderComponentProps();
            str.Should().BeEquivalentTo(new
            {
                Value = ":hij=\"{'name':'klm'}\""
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
        public void RenderVueFootScripts()
        {
            var str = VueDotNet.RenderScriptTags(VueScriptLocation.Foot);
            str.Should().BeEquivalentTo(new
            {
                Value = $"<script src=\"{VueUrl}\"></script><script src=\"{AppUrl}?9783f695694f40525c6e1bd9ad38ac6d\"></script>"
            });
        }

        [Fact]
        public void RenderVueHeadScripts()
        {
            var str = VueDotNet.RenderScriptTags(VueScriptLocation.Head);
            str.Should().BeEquivalentTo(new
            {
                Value = $"<link as=\"script\" href=\"{VueUrl}\" rel=\"preload\"></link><link as=\"script\" href=\"{AppUrl}?9783f695694f40525c6e1bd9ad38ac6d\" rel=\"preload\"></link>"
            });
        }

        [Fact]
        public void ComponentToTagName()
        {
            var tagName = "CoolComponentTagName";
            var str = tagName.ToWebComponentTagName();
            str.Should().BeEquivalentTo(new
            {
                Value = $"my-vue-cool-component-tag-name"
            });
        }
    }
}
