using System.Collections.Generic;

namespace Vue.Net.WebComponents
{
    public interface IVueComponentWithProps
    {
        /// <summary>
        /// Props to be passed into the Vue component instance.
        /// </summary>
        IDictionary<string, object> Props { get; }
    }
}
