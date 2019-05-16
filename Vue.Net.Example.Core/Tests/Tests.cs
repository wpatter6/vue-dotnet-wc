using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Vue.Net.WebComponents;
using Vue.Net.Example.Core;
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
                AppUrl,
                AppPrefix = "my-vue",
                VueUrl,
                Components = new dynamic[]
                {
                    new { Name = "Home" },
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
            var namedSlot1 = new Mock<IVueNamedSlot>();
            namedSlot1.Setup(m => m.SlotName).Returns("slot1");
            namedSlot1.Setup(m => m.ContentHtml).Returns("<div>This is slot 1.</div>");

            var namedSlot2 = new Mock<IVueNamedSlot>();
            namedSlot2.Setup(m => m.SlotName).Returns("slot2");
            namedSlot2.Setup(m => m.ContentHtml).Returns("<div>This is the second slot.</div>");

            var component = new Mock<IVueComponentWithNamedSlots>();
            component.Setup(m => m.NamedSlots).Returns(new List<IVueNamedSlot>()
            {
                namedSlot1.Object,
                namedSlot2.Object
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
                Value = $"<script src=\"{VueUrl}\"></script><script src=\"{AppUrl}\"></script>"
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
    }
}
