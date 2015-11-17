// There is no copyright, you can use and abuse this source without limit.
// There is no warranty, you are responsible for the consequences of your use of this source.
// There is no burden, you do not need to acknowledge this source in your use of this source.

namespace Toolbox.NET
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Implementation of the IFactory and IFactoryItemSink interfaces
    /// </summary>
    internal sealed partial class Factory : IFactory, IFactoryItemSink
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Factory"/> class.
        /// </summary>
        public Factory()
        {
        }

        /// <summary>
        /// Copies the target widget maker's methods into this factory
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="onDuplicate">The action to take when duplicates are encountered.</param>
        /// <returns>
        /// this
        /// </returns>
        public IFactory Pull<T>(IWidgetMaker<T> widgetMaker, ItemSinkDuplicate onDuplicate)
            where T : class
        {
            widgetMaker.Push(this, onDuplicate);
            return this;
        }

        /// <summary>
        /// Copies the target factory's methods into this factory
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="onDuplicate">The action to take when duplicates are encountered.</param>
        /// <returns>
        /// this
        /// </returns>
        public IFactory Pull(IFactory factory, ItemSinkDuplicate onDuplicate)
        {
            factory.Push(this, onDuplicate);
            return this;
        }

        /// <summary>
        /// Copies this factory's methods to the target factory sink
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="onDuplicate">The action to take when duplicates are encountered.</param>
        /// <returns>
        /// this
        /// </returns>
        public IFactory Push(IFactoryItemSink factory, ItemSinkDuplicate onDuplicate)
        {
            foreach (var method in methods)
            {
                factory.Add(method.Key, method.Value, onDuplicate);
            }
            return this;
        }

        /// <summary>
        /// Adds the method into this factory
        /// </summary>
        /// <param name="methodTypeInformation">The type information of the method.</param>
        /// <param name="method">The method as an Object (becuase I dont know how to carry the type around).</param>
        /// <param name="onDuplicate">The action to take when duplicates are encountered.</param>
        /// <returns>
        /// this
        /// </returns>
        public IFactoryItemSink Add(List<Type> methodTypeInformation, object method, ItemSinkDuplicate onDuplicate)
        {
            Merge(methodTypeInformation, method, onDuplicate);
            return this;
        }

        /// <summary>
        /// Merges the specified method type information.
        /// </summary>
        /// <param name="methodTypeInformation">The method type information.</param>
        /// <param name="method">The method.</param>
        /// <param name="onDuplicate">The on duplicate.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidProgramException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IFactory")]
        private IFactory Merge(List<Type> methodTypeInformation, object method, ItemSinkDuplicate onDuplicate)
        {
            if (!methods.ContainsKey(methodTypeInformation))
            {
                methods.Add(methodTypeInformation, method);
            }
            else if (onDuplicate == ItemSinkDuplicate.Override)
            {
                methods[methodTypeInformation] = method;
            }
            else if (onDuplicate == ItemSinkDuplicate.Throw)
            {
                throw new System.InvalidProgramException(
                    System.String.Format("IFactory.Merge(IFactory) has conflicting functions assigned for IFactory.Add<{0}>()"
                    , MethodKeyToString(methodTypeInformation)));
            }
            // else onDuplicate == ItemSinkDuplicate.Ignore
            return this;
        }

        #region IFactory

        private static IWidgetMaker<T> CheckWidgetMaker<T>(IWidgetMaker<T> widgetMaker)
            where T : class
        {
            if (widgetMaker == null)
            {
                throw new InvalidProgramException(string.Format("null widget maker for {0}", TypeStringOriginalish(widgetMaker.GetType().ToString())));
            }
            return widgetMaker;
        }

        /// <summary>
        /// Merges the supplied factory's methods into my methods, ignores duplicates.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns></returns>
        //public IFactory AddFrom<T>(IWidgetMaker<T> widgetMaker) where T : class
        //{
        //	//return Merge(CheckWidgetMaker(widgetMaker).IFactory.GetMethodEnumerator, MergeType.MergeTypeIgnore);
        //    widgetMaker.SendTo(this);
        //    return this;
        //}
        public IFactory AddFrom(IFactory factory)
        {
            throw new NotImplementedException();
            //return Merge(factory.GetMethodEnumerator, MergeType.MergeTypeIgnore);
        }

        /// <summary>
        /// Merges the supplied factory's methods into my methods, but throw on duplicates.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns></returns>
        //public IFactory MergeFrom<T>(IWidgetMaker<T> widgetMaker) where T : class
        //{
        //    throw new NotImplementedException();
        //    //return Merge(CheckWidgetMaker(widgetMaker).IFactory.GetMethodEnumerator, MergeType.MergeTypeThrow);
        //}
        public IFactory MergeFrom(IFactory factory)
        {
            throw new NotImplementedException();
            //return Merge(factory.GetMethodEnumerator, MergeType.MergeTypeThrow);
        }

        /// <summary>
        /// Merges the supplied factory's methods into my methods overriding duplicates.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns></returns>
        //public IFactory UpdateFrom<T>(IWidgetMaker<T> widgetMaker) where T : class
        //{
        //    throw new NotImplementedException();
        //    //return Merge(CheckWidgetMaker(widgetMaker).IFactory.GetMethodEnumerator, MergeType.MergeTypeOverride);
        //}
        public IFactory UpdateFrom(IFactory factory)
        {
            throw new NotImplementedException();
            //return Merge(factory.GetMethodEnumerator, MergeType.MergeTypeOverride);
        }

        /// <summary>
        /// Creates a specialized factory focused on only one product.
        /// </summary>
        /// <typeparam name="T">The specific type that is set up or created.</typeparam>
        /// <returns></returns>
        public IWidgetMaker<T> WidgetMaker<T>()
            where T : class
        {
            return new FactoryWidgetMaker<T>(this);
        }

        #endregion IFactory

        #region Private

        /// <summary>
        /// Merges the supplied factory's methods into my methods.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="allowDuplicates">if set to <c>true</c> duplicates will override existing methods or throw if set to <c>false</c></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IFactory")]
        private IFactory Merge(IEnumerator<KeyValuePair<List<Type>, object>> methodEnumerator, MergeType mergeType)
        {
            while (methodEnumerator.MoveNext())
            {
                if (!methods.ContainsKey(methodEnumerator.Current.Key))
                {
                    methods.Add(methodEnumerator.Current.Key, methodEnumerator.Current.Value);
                }
                else if (mergeType == MergeType.MergeTypeOverride)
                {
                    methods[methodEnumerator.Current.Key] = methodEnumerator.Current.Value;
                }
                else if (mergeType == MergeType.MergeTypeThrow)
                {
                    throw new System.InvalidProgramException(
                        System.String.Format("IFactory.Merge(IFactory) has conflicting functions assigned for IFactory.Add<{0}>()"
                        , MethodKeyToString(methodEnumerator.Current.Key)));
                }
                // else mergeType == MergeType.MergeTypeIgnore
            }
            return this;
        }

        private SortedDictionary<List<Type>, object> methods = new SortedDictionary<List<Type>, object>(new ListSystemTypeComparer());

        private static string MethodKeyToString(List<Type> key)
        {
            return string.Join(",", (from item in key select item.ToString()));
        }

        private IFactory AddMethod(List<Type> key, object method)
        {
            if (!methods.ContainsKey(key))
            {
                methods.Add(key, method);
            }
            else
            {
                methods[key] = method;
            }
            return this;
        }

        private static bool IsNumeric(char c)
        {
            int x;
            return int.TryParse(new string(c, 1), out x);
        }

        private static string TypeStringOriginalish(string s)
        {
            int index;
            while ((index = s.LastIndexOf("`")) != -1)
            {
                s = s.Remove(index, 1);
                while (index < s.Length && IsNumeric(s[index]))
                    s = s.Remove(index, 1);
            }
            return s.Replace('[', '<').Replace(']', '>').Replace(",", ", ");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IFactory")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WidgetMaker")]
        private METHOD FindMethod<METHOD>(List<Type> key, FindFailAction findFailAction)
            where METHOD : class
        {
            if (methods.ContainsKey(key))
            {
                var method = methods[key] as METHOD;
                if (method != null)
                {
                    return method;
                }
            }

            if (findFailAction == FindFailAction.FindFailActionReturn)
            {
                return null;
            }

            var tresult = string.Format("IFactory.WidgetMaker<{0}>()", TypeStringOriginalish(key.Find((item) => true).ToString()));
            var argtypes = TypeStringOriginalish((key.Count > 1) ? MethodKeyToString(key.GetRange(1, key.Count - 1)) : string.Empty);
            int argindex = 0;
            var argspec = TypeStringOriginalish(string.Join(", ", (from typename in key.GetRange(1, key.Count - 1) select string.Format("{0} arg{1}", typename, ++argindex))));
            throw new System.InvalidProgramException(
                System.String.Format("{0}.Create<{1}>(...) has no create function assigned, use {0}.Setup(({2}) => {{ ... }}) to assign one"
                , tresult
                , argtypes
                , argspec));
        }

        #endregion Private
    }
}