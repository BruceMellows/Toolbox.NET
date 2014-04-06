// Copyright (c) 2014, Bruce Mellows
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
// 1. This file must retain the above copyright notice, this list of conditions
//    and the following disclaimer.
// 2. Redistributions of source code must retain the above copyright notice, this
//    list of conditions and the following disclaimer.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

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