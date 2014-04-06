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