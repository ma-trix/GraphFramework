using System;
using System.Collections.Generic;

namespace GraphFramework
{
    public static class LinkedListExtensions
    {
        public static void AppendRange<T>(this LinkedList<T> source,
                                          IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                source.AddLast(item);
            }
        }

        public static void PrependRange<T>(this LinkedList<T> source,
                                           IEnumerable<T> items)
        {
            LinkedListNode<T> first = source.First;
            foreach (T item in items)
            {
                source.AddBefore(first, item);
            }
        }

        public static int RemoveAll<T>(this LinkedList<T> list, Predicate<T> match)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }
            var count = 0;
            var node = list.First;
            while (node != null)
            {
                var next = node.Next;
                if (match(node.Value))
                {
                    list.Remove(node);
                    count++;
                }
                node = next;
            }
            return count;
        }
    }
}