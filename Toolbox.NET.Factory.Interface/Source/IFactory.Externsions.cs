// There is no copyright, you can use and abuse this source without limit.
// There is no warranty, you are responsible for the consequences of your use of this source.
// There is no burden, you do not need to acknowledge this source in your use of this source.

namespace Toolbox.NET
{
    public static class ExtensionsForIFactory
    {
        /// <summary>
        /// Copies the target widget maker's methods into this factory
        /// </summary>
        /// <typeparam name="T">The type of the widget the IWidgetMaker makes.</typeparam>
        /// <param name="factory">The factory.</param>
        /// <param name="widgetMaker">The source widget maker.</param>
        /// <returns>this</returns>
        /// <remarks>onDuplicate is set to ItemSinkDuplicate.Throw</remarks>
        public static IFactory Pull<T>(this IFactory factory, IWidgetMaker<T> widgetMaker) where T : class
        {
            return factory.Pull(widgetMaker, ItemSinkDuplicate.Throw);
        }

        /// <summary>
        /// Copies the target factory's methods into this factory
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="thatFactory">The that factory.</param>
        /// <returns>this</returns>
        /// <remarks>onDuplicate is set to ItemSinkDuplicate.Throw</remarks>
        public static IFactory Pull(this IFactory factory, IFactory thatFactory)
        {
            return factory.Pull(thatFactory, ItemSinkDuplicate.Throw);
        }

        /// <summary>
        /// Copies this factory's methods to the target factory sink
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="thatFactory">The that factory.</param>
        /// <returns>this</returns>
        /// <remarks>onDuplicate is set to ItemSinkDuplicate.Throw</remarks>
        public static IFactory Push(this IFactory factory, IFactoryItemSink thatFactory)
        {
            return factory.Push(thatFactory, ItemSinkDuplicate.Throw);
        }
    }
}