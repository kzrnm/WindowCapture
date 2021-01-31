using System.Runtime.InteropServices;

namespace Kzrnm.WindowCapture.Windows
{
    internal static class NativeMethods
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string? psz1, string? psz2);
    }
}
