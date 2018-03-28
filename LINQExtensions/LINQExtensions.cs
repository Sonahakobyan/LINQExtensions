using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQExtensions
{
    public enum PredicateType
    {
        Where,
        Select
    }

    public class QueueNode
    {
        public Func<Object, Object> Predicate { get; set; }
        public PredicateType Type { get; set; }
    }

    public static class LINQExtensions
    {
        private static Dictionary<Int32, Queue<QueueNode>> predicates;

        static LINQExtensions()
        {
            predicates = new Dictionary<Int32, Queue<QueueNode>>();
        }

        public static IEnumerable<TResult> ExtensionSelect<TSource, TResult>(this IEnumerable<TSource> sources, Func<TSource, TResult> selector)
        {
            sources = sources ?? throw new ArgumentNullException();
            selector = selector ?? throw new ArgumentNullException();

            Int32 key;
            if (sources is ExtensionWhereEnumerable<TSource>)
            {
                key = (sources as ExtensionWhereEnumerable<TSource>).Key;
            }
            else
            {
                key = sources.GetHashCode();
            }

            if (!predicates.ContainsKey(key))
            {
                predicates.Add(key, new Queue<QueueNode>());
            }

            Object objPredicate(Object x) => selector((TSource)x);

            predicates[key].Enqueue(new QueueNode
            {
                Predicate = objPredicate,
                Type = PredicateType.Where
            });

            ExtensionSelectEnumerable<TSource, TResult> newSources = new ExtensionSelectEnumerable<TSource, TResult>(sources, selector, key);

            return newSources;
        }

        public static IEnumerable<TSource> ExtensionWhere<TSource>(this IEnumerable<TSource> sources, Func<TSource, Boolean> predicate)
        {
            sources = sources ?? throw new ArgumentNullException();
            predicate = predicate ?? throw new ArgumentNullException();

            Object objPredicate(Object x) => predicate((TSource)x);

            Int32 key;
            if (sources is ExtensionWhereEnumerable<TSource>)
            {
                key = (sources as ExtensionWhereEnumerable<TSource>).Key;
            }
            else
            {
                key = sources.GetHashCode();
            }

            if (!predicates.ContainsKey(key))
            {
                predicates.Add(key, new Queue<QueueNode>());
            }

            predicates[key].Enqueue(new QueueNode
            {
                Predicate = objPredicate,
                Type = PredicateType.Where
            });

            ExtensionWhereEnumerable<TSource> newSources = new ExtensionWhereEnumerable<TSource>(sources, predicate, key);

            return newSources;
        }

        public static IEnumerable<T> ApplyPredicates<T, TSource>(IEnumerable<TSource> sources, Int32 key)
        {
            sources = sources ?? throw new ArgumentNullException();
            IEnumerable<T> result = null;
            if (predicates.ContainsKey(key))
            {
                Queue<QueueNode> queue = predicates[key];

                while (queue.Count > 0)
                {
                    QueueNode node = queue.Dequeue();

                    if (node.Type == PredicateType.Where)
                    {
                        if (result == null)
                        {
                            Boolean predicate(TSource x) => (Boolean)node.Predicate(x);
                            result = (new ExtensionWhereEnumerable<TSource>(sources, predicate, key)) as IEnumerable<T>;
                        }
                        else
                        {
                            Boolean predicate(T x) => (Boolean)node.Predicate(x);
                            result = new ExtensionWhereEnumerable<T>(result, predicate, key);
                        }
                    }
                    else if (node.Type == PredicateType.Select)
                    {
                        if (result == null)
                        {
                            T predicate(TSource x) => (T)node.Predicate(x);
                            result = new ExtensionSelectEnumerable<TSource, T>(sources, predicate, key);
                        }
                        else
                        {
                            T predicate(T x) => (T)node.Predicate(x);
                            result = new ExtensionSelectEnumerable<T, T>(result, predicate, key);
                        }
                    }
                }

                predicates.Remove(key);
            }

            return null;
        }
    }
}