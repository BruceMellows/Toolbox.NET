// There is no copyright, you can use and abuse this source without limit.
// There is no warranty, you are responsible for the consequences of your use of this source.
// There is no burden, you do not need to acknowledge this source in your use of this source.

namespace Toolbox.NET
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed partial class Factory : IFactory, IFactoryItemSink
    {
        private sealed class FactoryWidgetItemSink<T> : IFactoryItemSink
            where T : class
        {
            private IFactoryItemSink sink;

            public FactoryWidgetItemSink(IFactoryItemSink argSink)
            {
                sink = argSink;
            }

            public IFactoryItemSink Add(List<Type> methodTypeInformation, object method, ItemSinkDuplicate onDuplicate)
            {
                if (methodTypeInformation.Count > 0 && methodTypeInformation.ElementAt(0) == typeof(T))
                {
                    sink.Add(methodTypeInformation, method, onDuplicate);
                }
                return this;
            }
        }
    }
}