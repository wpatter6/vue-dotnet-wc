using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Core;
using Vue.Net.WebComponents;

namespace Vue.Net.Example.EPi.Models.Blocks
{
    public class VueBlockNamedSlotContent : IVueNamedSlot
    {
        public string SlotName { get; set; }

        public string TagName { get; set; }

        public XhtmlString Content { get; set; }

        public string ContentHtml => Content != null ? Content.ToEditString() : string.Empty;
    }
}
