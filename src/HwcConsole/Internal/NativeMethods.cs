using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace HwcConsole.Internal
{
    public static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        internal static extern IntPtr LoadLibrary(String dllname);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, String procname);
    }
}
