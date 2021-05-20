using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zero.Extensions
{
    public static class DictionaryExtension
    {
        public static void Merge<TK, TV>(this IDictionary<TK, TV> target, IDictionary<TK, TV> source, bool overwrite = false)
        {
            source.ToList().ForEach(_ => {
                if ((!target.ContainsKey(_.Key)) || overwrite)
                    target[_.Key] = _.Value;
            });
        }
    }
}