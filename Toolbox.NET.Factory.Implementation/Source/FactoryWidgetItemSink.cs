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