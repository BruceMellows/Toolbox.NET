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