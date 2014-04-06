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

    internal sealed partial class Factory
    {
        private static List<Type> MethodKey<T1>()
        {
            return new List<Type> { typeof(T1) };
        }

        private IFactory AddWidgetMaker<T>(System.Func<T> method)
        {
            return AddMethod(MethodKey<T>(), method);
        }

        private T CreateWidgetFrom<T>()
        {
            return FindMethod<System.Func<T>>(MethodKey<T>(), FindFailAction.FindFailActionThrow)();
        }

        private bool CanCreateWidgetFrom<T>()
        {
            return FindMethod<System.Func<T>>(MethodKey<T>(), FindFailAction.FindFailActionReturn) != null;
        }

        private static List<Type> MethodKey<T1, T2>()
        {
            return new List<Type> { typeof(T1), typeof(T2) };
        }

        private IFactory AddWidgetMaker<T, T1>(System.Func<T1, T> method)
        {
            return AddMethod(MethodKey<T, T1>(), method);
        }

        private T CreateWidgetFrom<T, T1>(T1 arg1)
        {
            return FindMethod<System.Func<T1, T>>(MethodKey<T, T1>(), FindFailAction.FindFailActionThrow)(arg1);
        }

        private bool CanCreateWidgetFrom<T, T1>()
        {
            return FindMethod<System.Func<T1, T>>(MethodKey<T, T1>(), FindFailAction.FindFailActionReturn) != null;
        }

        private static List<Type> MethodKey<T1, T2, T3>()
        {
            return new List<Type> { typeof(T1), typeof(T2), typeof(T3) };
        }

        private IFactory AddWidgetMaker<T, T1, T2>(System.Func<T1, T2, T> method)
        {
            return AddMethod(MethodKey<T, T1, T2>(), method);
        }

        private T CreateWidgetFrom<T, T1, T2>(T1 arg1, T2 arg2)
        {
            return FindMethod<System.Func<T1, T2, T>>(MethodKey<T, T1, T2>(), FindFailAction.FindFailActionThrow)(arg1, arg2);
        }

        private bool CanCreateWidgetFrom<T, T1, T2>()
        {
            return FindMethod<System.Func<T1, T2, T>>(MethodKey<T, T1, T2>(), FindFailAction.FindFailActionReturn) != null;
        }

        private static List<Type> MethodKey<T1, T2, T3, T4>()
        {
            return new List<Type> { typeof(T1), typeof(T2), typeof(T3), typeof(T4) };
        }

        private IFactory AddWidgetMaker<T, T1, T2, T3>(System.Func<T1, T2, T3, T> method)
        {
            return AddMethod(MethodKey<T, T1, T2, T3>(), method);
        }

        private T CreateWidgetFrom<T, T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
        {
            return FindMethod<System.Func<T1, T2, T3, T>>(MethodKey<T, T1, T2, T3>(), FindFailAction.FindFailActionThrow)(arg1, arg2, arg3);
        }

        private bool CanCreateWidgetFrom<T, T1, T2, T3>()
        {
            return FindMethod<System.Func<T1, T2, T3, T>>(MethodKey<T, T1, T2, T3>(), FindFailAction.FindFailActionReturn) != null;
        }

        private static List<Type> MethodKey<T1, T2, T3, T4, T5>()
        {
            return new List<Type> { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) };
        }

        private IFactory AddWidgetMaker<T, T1, T2, T3, T4>(System.Func<T1, T2, T3, T4, T> method)
        {
            return AddMethod(MethodKey<T, T1, T2, T3, T4>(), method);
        }

        private T CreateWidgetFrom<T, T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return FindMethod<System.Func<T1, T2, T3, T4, T>>(MethodKey<T, T1, T2, T3, T4>(), FindFailAction.FindFailActionThrow)(arg1, arg2, arg3, arg4);
        }

        private bool CanCreateWidgetFrom<T, T1, T2, T3, T4>()
        {
            return FindMethod<System.Func<T1, T2, T3, T4, T>>(MethodKey<T, T1, T2, T3, T4>(), FindFailAction.FindFailActionReturn) != null;
        }
    }
}