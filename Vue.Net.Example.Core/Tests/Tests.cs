using FluentAssertions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Xunit;
using Vue.Net.WebComponents;
using Vue.Net.Example.Core;
using Vue.Net.Example.Core.Models;

namespace Vue.Net.Example.Core.Tests
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
        public void RenderVueComponent()
        {
            var component = new VueComponent()
            {
                ComponentName = "HelloWorld",
                Message = "Hello Core"
            };

            var str = component.RenderTag();
            str.Should().BeEquivalentTo(new
            {
                Value = "<v-app-hello-world msg=\"Hello Core\"></v-app-hello-world>"
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
                //"<link as="script" href="https://unpkg.com/vue@2.6.10" rel=\"preload\"></link><link as=\"script\" href="https://vuecdndev2.azureedge.net/v-app.js" rel="preload"></link>"
                Value = $"<link as=\"script\" href=\"{VueUrl}\" rel=\"preload\"></link><link as=\"script\" href=\"{AppUrl}\" rel=\"preload\"></link>"
            });
        }
    }
}
