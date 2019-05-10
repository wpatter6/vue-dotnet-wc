namespace Vue.Net.WebComponents
{
    public interface IVueNamedSlot
    {
        /// <summary>
        /// The named slot's name
        /// </summary>
        string SlotName { get; }
        /// <summary>
        /// The tag name of the top level element of the slot.  The default is 'div'.
        /// </summary>
        string TagName { get; }
        /// <summary>
        /// The raw HTML string to be rendered within the named slot.
        /// </summary>
        string ContentHtml { get; }
    }
}
