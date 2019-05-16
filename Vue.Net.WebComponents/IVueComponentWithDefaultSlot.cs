using System;
using System.Collections.Generic;
using System.Text;

namespace Vue.Net.WebComponents
{
    public interface IVueComponentWithDefaultSlot
    {
        /// <summary>
        /// Raw HTML string to be rendered inside the default slot of the Vue component instance.
        /// </summary>
        string SlotHtml { get; }
    }
}
