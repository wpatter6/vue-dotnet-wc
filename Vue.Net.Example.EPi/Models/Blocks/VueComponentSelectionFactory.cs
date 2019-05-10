using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;
using System.Linq;
using Vue.Net.WebComponents;

namespace Vue.Net.Example.EPi.Models.Blocks
{
    public class VueComponentSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var vueSettings = VueConfig.Settings;
            return vueSettings.Components.Select(x => new SelectItem { Text = x.Name, Value = x.Name });
        }
    }
}
