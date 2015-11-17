// There is no copyright, you can use and abuse this source without limit.
// There is no warranty, you are responsible for the consequences of your use of this source.
// There is no burden, you do not need to acknowledge this source in your use of this source.

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