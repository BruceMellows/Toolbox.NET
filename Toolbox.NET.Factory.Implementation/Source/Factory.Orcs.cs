// There is no copyright, you can use and abuse this source without limit.
// There is no warranty, you are responsible for the consequences of your use of this source.
// There is no burden, you do not need to acknowledge this source in your use of this source.

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