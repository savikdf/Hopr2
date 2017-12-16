using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class EnumerableExtensions {

    public static T MaxObject<T, U>(this IEnumerable<T> source, Func<T, U> selector)
      where U : IComparable<U>
    {
        if (source == null) throw new ArgumentNullException("source");
        bool first = true;
        T maxObj = default(T);
        U maxKey = default(U);
        foreach (var item in source)
        {
            if (first)
            {
                maxObj = item;
                maxKey = selector(maxObj);
                first = false;
            }
            else
            {
                U currentKey = selector(item);
                if (currentKey.CompareTo(maxKey) > 0)
                {
                    maxKey = currentKey;
                    maxObj = item;
                }
            }
        }
        if (first) throw new InvalidOperationException("Sequence is empty.");
        return maxObj;
    }



    public static T MinObject<T, U>(this IEnumerable<T> source, Func<T, U> selector)
      where U : IComparable<U>
    {
        if (source == null) throw new ArgumentNullException("source");
        bool first = true;
        T minObj = default(T);
        U minKey = default(U);
        foreach (var item in source)
        {
            if (first)
            {
                minObj = item;
                minKey = selector(minObj);
                first = false;
            }
            else
            {
                U currentKey = selector(item);
                if (currentKey.CompareTo(minKey) < 0)
                {
                    minKey = currentKey;
                    minObj = item;
                }
            }
        }
        if (first) throw new InvalidOperationException("Sequence is empty.");
        return minObj;
    }
}
