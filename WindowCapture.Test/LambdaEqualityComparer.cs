using System;
using System.Collections.Generic;

namespace Kzrnm.WindowCapture
{
    public record LambdaEqualityComparer<T>(Func<T, T, bool> Func) : IEqualityComparer<T>
    {
        public bool Equals(T x, T y) => Func(x, y);
        public int GetHashCode(T obj) => 0;
    }
}
