using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vue.Net.WebComponents;

namespace Vue.Net.Example.Core.Models
{
    public class VueComponent : IVueComponent
    {
        public string ComponentName { get; set; }
        public string Message { get; set; }
        public IDictionary<string, object> Props => new Dictionary<string, object>()
        {
            { "msg", Message }
        };
        public string SlotHtml { get; set; }
        public IList<IVueNamedSlot> NamedSlots { get; set; }
    }
}
