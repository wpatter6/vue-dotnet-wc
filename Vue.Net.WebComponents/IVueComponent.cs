using System.Collections.Generic;

namespace Vue.Net.WebComponents
{
    public interface IVueComponent
    {
        /// <summary>
        /// The Vue.js web component name.
        /// This should match the file name of the vue component.
        /// Ex: For a component 'VueComponent1.vue' the name would be 'VueComponent1'
        /// </summary>
        string ComponentName { get; }
        /// <summary>
        /// Props to be passed into the Vue component instance.
        /// </summary>
        IDictionary<string, object> Props { get; }
        /// <summary>
        /// Raw HTML string to be rendered inside the default slot of the Vue component instance.
        /// </summary>
        string SlotHtml { get; }
        /// <summary>
        /// A list of names and content to be rendered inside the Vue component instance as named slots.
        /// </summary>
        IList<IVueNamedSlot> NamedSlots { get; }
    }
}
