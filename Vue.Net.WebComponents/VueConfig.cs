using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;

namespace Vue.Net.WebComponents
{
    public interface IVueConfig
    {
        string AppUrl { get; }
        string AppPrefix { get; }
        string VueUrl { get; }
        IEnumerable<IVueConfigComponent> Components { get; }
    }

    public interface IVueConfigComponent
    {
        string Name { get; }
    }

    public class VueConfig : System.Configuration.ConfigurationSection, IVueConfig
    {
        public static IVueConfig Settings => ConfigurationManager.GetSection("vueConfig") as VueConfig ?? GetVueConfigCore();

        private VueConfig() { }

        [ConfigurationProperty("appUrl")]
        public virtual string AppUrl => this["appUrl"] as string;

        [ConfigurationProperty("appPrefix")]
        public virtual string AppPrefix => this["appPrefix"] as string;

        [ConfigurationProperty("vueUrl")]
        public virtual string VueUrl => this["vueUrl"] as string;

        [ConfigurationProperty("components")]
        [ConfigurationCollection(typeof(string), AddItemName = "component")]
        public virtual Components ComponentList => this["components"] as Components;

        public virtual IEnumerable<IVueConfigComponent> Components => ComponentList;

        private static IVueConfig GetVueConfigCore()
        {
            try
            {
                var settingsString = File.ReadAllText("vuesettings.json");
                var dynamic = JsonConvert.DeserializeObject(settingsString);
                return JsonConvert.DeserializeObject<CoreVueConfig>(settingsString);
            }
            catch
            {
                throw new Exception(
                    "Unable to read vuesettings.json.  Make sure the file exists and Copy To Output Directory is set to \"always\"");
            }
        }
    }

    public class CoreVueConfig : IVueConfig
    {
        public string AppUrl { get; set; }
        public string AppPrefix { get; set; }
        public string VueUrl { get; set; }
        public IEnumerable<CoreVueComponent> Components { get; set; }

        IEnumerable<IVueConfigComponent> IVueConfig.Components => Components;
    }

    public class CoreVueComponent: IVueConfigComponent
    {
        public CoreVueComponent(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
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
}