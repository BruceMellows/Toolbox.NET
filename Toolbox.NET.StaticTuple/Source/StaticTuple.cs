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

    public sealed class StaticTuple
    {
        public static StaticTuple<TItem1, TItem2> Create<TItem1, TItem2>(TItem1 item1, TItem2 item2)
            where TItem1 : class
            where TItem2 : class
        {
            return new StaticTuple<TItem1, TItem2>(item1, item2);
        }
    }

    public sealed class StaticTuple<TItem1, TItem2> : IEquatable<StaticTuple<TItem1, TItem2>>
        where TItem1 : class
        where TItem2 : class
    {
        private StaticTuple()
        {
        }

        internal StaticTuple(TItem1 item1, TItem2 item2)
        {
            this.Item1 = item1;
            this.Item2 = item2;
        }

        public TItem1 Item1 { get; private set; }

        public TItem2 Item2 { get; private set; }

        public static bool operator ==(StaticTuple<TItem1, TItem2> a, StaticTuple<TItem1, TItem2> b)
        {
            return ReferenceEquals(a, b) || a.Equals(b);
        }

        public static bool operator !=(StaticTuple<TItem1, TItem2> a, StaticTuple<TItem1, TItem2> b)
        {
            return !(ReferenceEquals(a, b) || a.Equals(b));
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as StaticTuple<TItem1, TItem2>);
        }

        public bool Equals(StaticTuple<TItem1, TItem2> that)
        {
            return !ReferenceEquals(that, null) && ReferenceEquals(this.Item1, that.Item1) && ReferenceEquals(this.Item2, that.Item2);
        }

        public override int GetHashCode()
        {
            return this.Item1.GetHashCode() ^ this.Item2.GetHashCode();
        }
    }
}