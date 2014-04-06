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

    internal sealed class ListSystemTypeComparer : IComparer<List<Type>>
    {
        public int Compare(List<Type> lhs, List<Type> rhs)
        {
            if (lhs == null && rhs == null)
            {
                return 0;
            }
            if (lhs == null)
            {
                return -1;
            }
            if (rhs == null)
            {
                return 1;
            }
            var e1 = lhs.GetEnumerator();
            var e2 = rhs.GetEnumerator();
            bool more1;
            bool more2;
            while ((more1 = e1.MoveNext()) & (more2 = e2.MoveNext()))
            {
                var t1 = e1.Current;
                var t2 = e2.Current;
                if (t1 != t2) return t1.ToString().CompareTo(t2.ToString());
            }
            return (more1 == more2) ? 0 : more1 ? 1 : -1;
        }
    }
}