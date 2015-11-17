// There is no copyright, you can use and abuse this source without limit.
// There is no warranty, you are responsible for the consequences of your use of this source.
// There is no burden, you do not need to acknowledge this source in your use of this source.

namespace Toolbox.NET
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// interface for adding methods to a factory
    /// </summary>
    public interface IFactoryItemSink
    {
        /// <summary>
        /// Adds the method into this factory
        /// </summary>
        /// <param name="methodTypeInformation">The type information of the method.</param>
        /// <param name="method">The method as an Object (becuase I dont know how to carry the type around).</param>
        /// <param name="onDuplicate">The action to take when duplicates are encountered.</param>
        /// <returns>this</returns>
        IFactoryItemSink Add(List<Type> methodTypeInformation, Object method, ItemSinkDuplicate onDuplicate);
    }
}