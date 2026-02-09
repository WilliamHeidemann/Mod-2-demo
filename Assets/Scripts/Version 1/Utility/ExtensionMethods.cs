using System;
using System.Collections.Generic;
using System.Linq;

namespace Version_1.Utility
{
    public static class ExtensionMethods
    {
        public static IEnumerable<(T First, T Second)> Pairs<T>(
            this IEnumerable<T> source)
        {
            T[] sourceArray = source.ToArray();
            return 
                from a in sourceArray 
                from b in sourceArray 
                select (a, b);
        }
        
        public static IEnumerable<(T First, T Second)> Pairs<T>(
            this IEnumerable<T> source,
            Func<T, T, bool> predicate)
        {
            T[] sourceArray = source.ToArray();
            return 
                from a in sourceArray 
                from b in sourceArray 
                where predicate(a, b) 
                select (a, b);
        }
    }
}