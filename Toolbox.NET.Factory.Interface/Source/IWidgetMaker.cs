// There is no copyright, you can use and abuse this source without limit.
// There is no warranty, you are responsible for the consequences of your use of this source.
// There is no burden, you do not need to acknowledge this source in your use of this source.

namespace Toolbox.NET
{
    /// <summary>
    /// Provides the multi argument setup and instantiation interface for an IFactory. Add more as required.
    /// </summary>
    /// <typeparam name="T">The type of the Object that is the focus of this interface.</typeparam>
    public interface IWidgetMaker<T>
        where T : class
    {
        /// <summary>
        /// Gets the factory that this is a "view" of.
        /// </summary>
        /// <value>The factory.</value>
        IFactory IFactory { get; }

        /// <summary>
        /// Copies this widget maker's methods to the target factory sink
        /// </summary>
        /// <param name="widgetMaker">The target factory.</param>
        /// <param name="onDuplicate">The action to take when duplicates are encountered.</param>
        /// <returns>this</returns>
        IWidgetMaker<T> Push(IFactoryItemSink factory, ItemSinkDuplicate onDuplicate);

        /// <summary>
        /// Instantiates a T from the supplied arguments.
        /// </summary>
        /// <returns>A T</returns>
        T Create();

        T Create<T1>(T1 arg1);

        T Create<T1, T2>(T1 arg1, T2 arg2);

        T Create<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);

        T Create<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

        /// <summary>
        /// Supply a method to handle the given number and type of arguments
        /// </summary>
        /// <param name="method">The method that is called when those arguments are passed to Create(...).</param>
        /// <returns>this</returns>
        IWidgetMaker<T> Setup(System.Func<T> method);

        IWidgetMaker<T> Setup<T1>(System.Func<T1, T> method);

        IWidgetMaker<T> Setup<T1, T2>(System.Func<T1, T2, T> method);

        IWidgetMaker<T> Setup<T1, T2, T3>(System.Func<T1, T2, T3, T> method);

        IWidgetMaker<T> Setup<T1, T2, T3, T4>(System.Func<T1, T2, T3, T4, T> method);

        /// <summary>
        /// Checks if an instance of T could be instantiated from the supplied arguments.
        /// </summary>
        /// <returns>A T</returns>
        bool CanCreateFrom();

        bool CanCreateFrom<T1>();

        bool CanCreateFrom<T1>(T1 arg1);

        bool CanCreateFrom<T1, T2>();

        bool CanCreateFrom<T1, T2>(T1 arg1, T2 arg2);

        bool CanCreateFrom<T1, T2, T3>();

        bool CanCreateFrom<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);

        bool CanCreateFrom<T1, T2, T3, T4>();

        bool CanCreateFrom<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    }
}