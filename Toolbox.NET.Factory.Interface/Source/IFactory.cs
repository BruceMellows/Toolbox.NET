// There is no copyright, you can use and abuse this source without limit.
// There is no warranty, you are responsible for the consequences of your use of this source.
// There is no burden, you do not need to acknowledge this source in your use of this source.

namespace Toolbox.NET
{
    using System;
    using System.Collections.Generic;

    public interface IFactory
    {
        /// <summary>
        /// Copies the target widget maker's methods into this factory
        /// </summary>
        /// <typeparam name="T">The type of the widget the IWidgetMaker makes.</typeparam>
        /// <param name="widgetMaker">The source widget maker.</param>
        /// <param name="onDuplicate">The action to take when duplicates are encountered.</param>
        /// <returns>this</returns>
        IFactory Pull<T>(IWidgetMaker<T> widgetMaker, ItemSinkDuplicate onDuplicate) where T : class;

        /// <summary>
        /// Copies the target factory's methods into this factory
        /// </summary>
        /// <param name="widgetMaker">The source factory.</param>
        /// <param name="onDuplicate">The action to take when duplicates are encountered.</param>
        /// <returns>this</returns>
        IFactory Pull(IFactory factory, ItemSinkDuplicate onDuplicate);

        /// <summary>
        /// Copies this factory's methods to the target factory sink
        /// </summary>
        /// <param name="widgetMaker">The target factory.</param>
        /// <param name="onDuplicate">The action to take when duplicates are encountered.</param>
        /// <returns>this</returns>
        IFactory Push(IFactoryItemSink factory, ItemSinkDuplicate onDuplicate);

        /// <summary>
        /// Creates a "view" of this factory focused on only one widget.
        /// </summary>
        /// <typeparam name="T">The type of the widget the IWidgetMaker makes.</typeparam>
        /// <returns>The widget maker</returns>
        IWidgetMaker<T> WidgetMaker<T>() where T : class;
    }
}