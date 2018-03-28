using System;
using System.Collections;
using System.Collections.Generic;

namespace LINQExtensions
{
    public class ExtensionSelectEnumerator<TSource, TResult> : IEnumerator<TResult>
    {
        private IEnumerable<TSource> sources;
        private Func<TSource, TResult> predicate;
        private IEnumerator<TSource> enumerator;
        private TSource current;
        private Int32 key;

        public ExtensionSelectEnumerator(IEnumerable<TSource> sources, Func<TSource, TResult> predicate, Int32 key)
        {
            this.sources = sources;
            this.predicate = predicate;
            enumerator = this.sources.GetEnumerator();
            current = default(TSource);
            this.key = key;
        }

        public TResult Current
        {
            get
            {
                return predicate(current);
            }
        }

        Object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose()
        {
        }

        public Boolean MoveNext()
        {
            Helper.Validate<TSource, TResult>(sources, key);

            if (enumerator.MoveNext())
            {
                current = enumerator.Current;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            enumerator = sources.GetEnumerator();
            current = default(TSource);
        }
    }
}