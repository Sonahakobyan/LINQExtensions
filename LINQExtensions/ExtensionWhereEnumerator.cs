using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQExtensions
{
    public sealed class ExtensionEnumerator<TSource> : IEnumerator<TSource>
    {
        private IEnumerable<TSource> sources;
        private Func<TSource, Object> predicate;
        private IEnumerator<TSource> enumerator;
        private Int32 key;
        private static readonly Dictionary<Int32, Boolean> firstIterations;
        private PredicateType Type;

        static ExtensionEnumerator()
        {
            firstIterations = new Dictionary<Int32, Boolean>();
        }

        public ExtensionEnumerator(IEnumerable<TSource> sources, Func<TSource, Object> predicate, Int32 key, PredicateType type)
        {
            this.sources = sources;
            enumerator = sources.GetEnumerator();

            this.predicate = predicate;
            Current = default(TSource);

            this.key = key;
            if (!firstIterations.ContainsKey(key))
            {
                firstIterations[key] = true;
            }

            Type = type;
        }

        public TSource Current { get; private set; }

        public Object OCurrent { get; private set; }

        Object IEnumerator.Current
        {
            get
            {
                if (Type == PredicateType.Where)
                {
                    return Current;
                }
                else if (Type == PredicateType.Select)
                {
                    return predicate((TSource)OCurrent);
                }

                return null;
            }
        }

        public void Dispose()
        {
            sources.GetEnumerator().Dispose();
        }

        public Boolean MoveNext()
        {
            Helper.Validate<TSource, Boolean>(sources, key);

            if (Type == PredicateType.Where)
            {
                while (enumerator.MoveNext())
                {
                    Boolean objPredicate(Object x) => (Boolean)predicate((TSource)x);

                    if (objPredicate(enumerator.Current))
                    {
                        Current = enumerator.Current;
                        return true;
                    }
                }

                return false;
            }
            else if (Type == PredicateType.Select)
            {
                if (enumerator.MoveNext())
                {
                    OCurrent = enumerator.Current;
                    return true;
                }

                return false;
            }

            return false;
        }

        public void Reset()
        {
            firstIterations[key] = false;
            Current = default(TSource);
        }
    }
}