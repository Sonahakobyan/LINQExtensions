using System;
using System.Collections;
using System.Collections.Generic;

namespace LINQExtensions
{
    public class ExtensionSelectEnumerable<TSource, TResult> : IEnumerable<TResult>
    {
        private IEnumerable<TSource> sources;
        private Func<TSource, TResult> predicate;
        private Int32 key;

        public ExtensionSelectEnumerable(IEnumerable<TSource> sources, Func<TSource, TResult> predicate, Int32 key)
        {
            this.sources = sources;
            this.predicate = predicate;
            this.key = key;
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            return new ExtensionSelectEnumerator<TSource, TResult>(sources, predicate, key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}