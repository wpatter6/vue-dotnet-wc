﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Vue.Net.WebComponents
{
    public interface IVueConfig
    {
        string AppUrl { get; }
        string AppPrefix { get; }
        string VueUrl { get; }
        IEnumerable<IVueConfigComponent> Components { get; }
        IEnumerable<IVueConfigScript> Scripts { get; }
        string WebRoot { get; }
        bool CacheBust { get; }
    }

    public interface IVueConfigComponent
    {
        string Name { get; }
    }

    public interface IVueConfigScript
    {
        string Url { get; }
        bool NoHash { get; }
    }

    public class VueConfig : System.Configuration.ConfigurationSection, IVueConfig
    {
        public static IVueConfig Settings => ConfigurationManager.GetSection("vueConfig") as VueConfig ?? VueConfigStartup.VueConfigStatic;

        private VueConfig() { }

        [ConfigurationProperty("appUrl")]
        public virtual string AppUrl => this["appUrl"] as string;

        [ConfigurationProperty("appPrefix")]
        public virtual string AppPrefix => this["appPrefix"] as string;

        [ConfigurationProperty("vueUrl")]
        public virtual string VueUrl => this["vueUrl"] as string;

        [ConfigurationProperty("cacheBust")]
        protected virtual string CacheBustString => this["cacheBust"] as string;

        [ConfigurationProperty("components")]
        [ConfigurationCollection(typeof(string), AddItemName = "component")]
        protected virtual Components ComponentList => this["components"] as Components;

        [ConfigurationProperty("scripts")]
        [ConfigurationCollection(typeof(string), AddItemName = "script")]
        protected virtual Scripts ScriptList => this["scripts"] as Scripts;

        public virtual IEnumerable<IVueConfigComponent> Components => ComponentList;

        public virtual IEnumerable<IVueConfigScript> Scripts => ScriptList;

        public virtual bool CacheBust => !CacheBustString?.Equals("false", StringComparison.InvariantCultureIgnoreCase) ?? true;

        public virtual string WebRoot {
            get
            {
                // TODO Fix this hack if I ever get a response on https://github.com/dotnet/standard/issues/1228
                return Directory.GetCurrentDirectory() + @"..\..\";
            }
        }
    }


    public class CoreVueConfig : IVueConfig
    {
        public string AppUrl { get; set; }
        public string AppPrefix { get; set; }
        public string VueUrl { get; set; }
        public IEnumerable<CoreVueComponent> Components { get; set; }
        public IEnumerable<CoreVueScript> Scripts { get; set; }

        IEnumerable<IVueConfigComponent> IVueConfig.Components => Components;

        IEnumerable<IVueConfigScript> IVueConfig.Scripts => Scripts;
        public string WebRoot { get; set; }
        public bool CacheBust { get; set; }
    }

    public class CoreVueComponent: IVueConfigComponent
    {
        public CoreVueComponent(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
    }

    public class CoreVueScript: IVueConfigScript
    {
        public string Url { get; set; }

        public bool NoHash { get; set; }
    }

    public class component : ConfigurationElement, IVueConfigComponent
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name => this["name"] as string;
    }

    public class Components : ConfigurationElementCollection, IEnumerable<IVueConfigComponent>
    {
        public component this[int index]
        {
            get => BaseGet(index) as component;
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new component this[string responseString]
        {
            get => BaseGet(responseString) as component;
            set
            {
                if (BaseGet(responseString) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                }
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new component();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((component)element).Name;
        }

        IEnumerator<IVueConfigComponent> IEnumerable<IVueConfigComponent>.GetEnumerator()
        {
            return this.BaseGetAllKeys().Select(key => (component)BaseGet(key)).GetEnumerator();
        }
    }

    public static class VueConfigStartup
    {
        internal static IVueConfig VueConfigStatic { get; set; }
        public static IHostingEnvironment UseVueWebComponents(this IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("vuesettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"vuesettings.{env.EnvironmentName}.json", optional: true);
            var config = builder.Build();

            var componentList = config.GetSection("components")?.Get<List<string>>();
            var scriptList = config.GetSection("scripts")?.GetChildren()?.Select(c => c.Get<CoreVueScript>());

            VueConfigStatic = new CoreVueConfig()
            {
                AppUrl = config["appUrl"],
                AppPrefix = config["appPrefix"],
                VueUrl = config["vueUrl"],
                Components = componentList?.Select(n => new CoreVueComponent(n)),
                Scripts = scriptList,
                WebRoot = env.WebRootPath ?? env.ContentRootPath + @"..\..\..\wwwroot\",
                CacheBust = !config["cacheBust"]?.Equals("false", StringComparison.InvariantCultureIgnoreCase) ?? true,
            };

            return env;
        }
    }

    public class Scripts : ConfigurationElementCollection, IEnumerable<IVueConfigScript>
    {
        public script this[int index]
        {
            get => BaseGet(index) as script;
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new script this[string responseString]
        {
            get => BaseGet(responseString) as script;
            set
            {
                if (BaseGet(responseString) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                }
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new script();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((script)element).Url;
        }

        IEnumerator<IVueConfigScript> IEnumerable<IVueConfigScript>.GetEnumerator()
        {
            return this.BaseGetAllKeys().Select(key => (script)BaseGet(key)).GetEnumerator();
        }
    }

    public class script: ConfigurationElement, IVueConfigScript
    {
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url => this["url"] as string;
        [ConfigurationProperty("noHash", IsRequired = false)]
        public bool NoHash => (this["noHash"] as string)?.Equals("true", StringComparison.InvariantCultureIgnoreCase) ?? false;
    }
}