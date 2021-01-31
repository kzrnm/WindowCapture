using Kzrnm.WindowCapture.Windows;
using System.Collections.Generic;

namespace Kzrnm.WindowCapture.Utils
{
    public class NaturalComparer : IComparer<string>
    {
        public static NaturalComparer Default { get; } = new NaturalComparer();
        public int Compare(string? x, string? y)
        {
            if (x is null)
            {
                if (y is null)
                    return 0;
                return -1;
            }
            if (y is null)
                return 1;
            return NativeMethods.StrCmpLogicalW(x, y);
        }
    }
}
