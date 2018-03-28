using System;
using System.Collections;
using System.Collections.Generic;

namespace LINQExtensions
{
    public sealed class ExtensionWhereEnumerable<TSource> : IEnumerable<TSource>
    {
        private IEnumerable<TSource> sources;
        private Func<TSource, Object> predicate;
        public Int32 Key { get; set; }

        public ExtensionWhereEnumerable(IEnumerable<TSource> sources, Func<TSource, Boolean> predicate, Int32 key)
        {
            this.sources = sources;
            Object objPredicate(TSource x) => predicate(x);
            this.predicate = objPredicate;
            Key = key;
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            return new ExtensionEnumerator<TSource>(sources, predicate, Key, PredicateType.Where);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}