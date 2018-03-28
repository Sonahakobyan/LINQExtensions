using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQExtensions
{
    public static class Helper
    {
        private static readonly Dictionary<Int32, Boolean> firstIterations;

        static Helper()
        {
            firstIterations = new Dictionary<Int32, Boolean>();
        }

        public static void Validate<T1, T2>(IEnumerable<T1> sources, Int32 key)
        {
            if (firstIterations.ContainsKey(key) && firstIterations[key])
            {
                LINQExtensions.ApplyPredicates<T2, T1>(sources, key);
                firstIterations[key] = false;
            }
        }
    }
}