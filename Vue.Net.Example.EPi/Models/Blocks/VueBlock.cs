using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Vue.Net.WebComponents;

namespace Vue.Net.Example.EPi.Models.Blocks
{
    [ContentType(DisplayName = "Vue component",
        GUID = "58fe24ed-95e4-4c8f-8a27-1a6237146ba3",
        Description = "Block for housing a pre-built vue component.",
        GroupName = SystemTabNames.Content)]
    public class VueBlock : IVueComponent
    {
        [Required]
        [Display(Name = "component",
            GroupName = SystemTabNames.Content,
            Description = "The name of the component from the Vue.js project to use in the block.",
            Order = 10)]
        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(VueComponentSelectionFactory))]
        public string ComponentName { get; set; }

        [Display(Name = "Message",
            GroupName = SystemTabNames.Content,
            Description = "An example prop.",
            Order = 20)]
        [CultureSpecific]
        public string Message { get; set; }


        public IDictionary<string, object> Props => new Dictionary<string, object>()
        {
            { "msg", Message }
        };

        [Display(Name = "Inner Content",
            GroupName = SystemTabNames.Content,
            Description = "Content to place inside the vue component's default slot.  The component must be built with a default slot for this content to be visible.",
            Order = 30)]
        [CultureSpecific]
        public XhtmlString SlotContent { get; set; }

        public string SlotHtml => SlotContent.ToEditString();

        [Display(Name = "Named Content",
            GroupName = SystemTabNames.Content,
            Description = "Content to place inside the vue component's named slot(s).  The component must be built with named slots for this content to be visible.",
            Order = 40)]
        [CultureSpecific]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<VueBlockNamedSlotContent>))]
        public IDictionary<string, string> NamedSlots { get; set; }

    }
}
