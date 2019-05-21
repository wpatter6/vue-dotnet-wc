using System.Collections.Generic;

namespace Vue.Net.WebComponents
{
    public interface IVueComponentWithNamedSlots
    {
        /// <summary>
        /// A dictionary of names and content html strings to be rendered inside the Vue component instance as named slots.
        /// </summary>
        IDictionary<string, string> NamedSlots { get; }
    }
}
