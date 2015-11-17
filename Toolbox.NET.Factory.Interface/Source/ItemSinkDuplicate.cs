// There is no copyright, you can use and abuse this source without limit.
// There is no warranty, you are responsible for the consequences of your use of this source.
// There is no burden, you do not need to acknowledge this source in your use of this source.

namespace Toolbox.NET
{
    /// <summary>
    /// Behaviours of IFactoryItemSink when duplicates are encountered
    /// </summary>
    public enum ItemSinkDuplicate
    {
        /// <summary>
        /// Ignore the new item - therefore keeps the original item
        /// </summary>
        Ignore,

        /// <summary>
        /// Override the original item - therefore keeps the new item
        /// </summary>
        Override,

        /// <summary>
        /// Throw an exception when a duplicate is encountered
        /// </summary>
        Throw,
    }
}