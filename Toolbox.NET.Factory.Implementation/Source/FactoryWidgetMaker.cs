﻿// Copyright (c) 2014, Bruce Mellows
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
    internal sealed partial class Factory
    {
        private sealed class FactoryWidgetMaker<T> : IWidgetMaker<T>
            where T : class
        {
            private Factory factory;

            public IFactory IFactory { get { return factory; } }

            public FactoryWidgetMaker(Factory factory)
            {
                this.factory = factory;
            }

            public IWidgetMaker<T> Push(IFactoryItemSink argFactory, ItemSinkDuplicate onDuplicate)
            {
                factory.Push(new FactoryWidgetItemSink<T>(argFactory), onDuplicate);
                return this;
            }

            #region T Create<>

            public T Create()
            {
                return factory.CreateWidgetFrom<T>();
            }

            public T Create<T1>(T1 arg1)
            {
                return factory.CreateWidgetFrom<T, T1>(arg1);
            }

            public T Create<T1, T2>(T1 arg1, T2 arg2)
            {
                return factory.CreateWidgetFrom<T, T1, T2>(arg1, arg2);
            }

            public T Create<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
            {
                return factory.CreateWidgetFrom<T, T1, T2, T3>(arg1, arg2, arg3);
            }

            public T Create<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            {
                return factory.CreateWidgetFrom<T, T1, T2, T3, T4>(arg1, arg2, arg3, arg4);
            }

            #endregion T Create<>

            #region IWidgetMaker<T> Setup

            public IWidgetMaker<T> Setup(System.Func<T> method)
            {
                factory.AddWidgetMaker<T>(method); return this;
            }

            public IWidgetMaker<T> Setup<T1>(System.Func<T1, T> method)
            {
                factory.AddWidgetMaker<T, T1>(method); return this;
            }

            public IWidgetMaker<T> Setup<T1, T2>(System.Func<T1, T2, T> method)
            {
                factory.AddWidgetMaker<T, T1, T2>(method); return this;
            }

            public IWidgetMaker<T> Setup<T1, T2, T3>(System.Func<T1, T2, T3, T> method)
            {
                factory.AddWidgetMaker<T, T1, T2, T3>(method); return this;
            }

            public IWidgetMaker<T> Setup<T1, T2, T3, T4>(System.Func<T1, T2, T3, T4, T> method)
            {
                factory.AddWidgetMaker<T, T1, T2, T3, T4>(method); return this;
            }

            #endregion IWidgetMaker<T> Setup

            #region bool CanCreateFrom

            public bool CanCreateFrom()
            {
                return factory.CanCreateWidgetFrom<T>();
            }

            public bool CanCreateFrom<T1>()
            {
                return factory.CanCreateWidgetFrom<T, T1>();
            }

            public bool CanCreateFrom<T1>(T1 arg1)
            {
                return CanCreateFrom<T1>();
            }

            public bool CanCreateFrom<T1, T2>()
            {
                return factory.CanCreateWidgetFrom<T, T1, T2>();
            }

            public bool CanCreateFrom<T1, T2>(T1 arg1, T2 arg2)
            {
                return CanCreateFrom<T1, T2>();
            }

            public bool CanCreateFrom<T1, T2, T3>()
            {
                return factory.CanCreateWidgetFrom<T, T1, T2, T3>();
            }

            public bool CanCreateFrom<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
            {
                return CanCreateFrom<T1, T2, T3>();
            }

            public bool CanCreateFrom<T1, T2, T3, T4>()
            {
                return factory.CanCreateWidgetFrom<T, T1, T2, T3, T4>();
            }

            public bool CanCreateFrom<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            {
                return CanCreateFrom<T1, T2, T3, T4>();
            }

            #endregion bool CanCreateFrom
        }
    }
}