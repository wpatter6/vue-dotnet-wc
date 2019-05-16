using System.Collections.Generic;

namespace Vue.Net.WebComponents
{
    public interface IVueComponent : IVueComponentWithProps, IVueComponentWithDefaultSlot, IVueComponentWithNamedSlots
    {
        /// <summary>
        /// The Vue.js web component name.
        /// This should match the file name of the vue component.
        /// Ex: For a component 'VueComponent1.vue' the name would be 'VueComponent1'
        /// </summary>
        string ComponentName { get; }
    }
}
