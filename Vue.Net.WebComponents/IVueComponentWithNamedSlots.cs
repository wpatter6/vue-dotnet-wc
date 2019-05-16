using System;
using System.Collections.Generic;
using System.Text;

namespace Vue.Net.WebComponents
{
    public interface IVueComponentWithNamedSlots
    {
        /// <summary>
        /// A list of names and content to be rendered inside the Vue component instance as named slots.
        /// </summary>
        IList<IVueNamedSlot> NamedSlots { get; }
    }
}
