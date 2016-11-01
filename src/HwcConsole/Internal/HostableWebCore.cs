using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace HwcConsole.Internal
{
    public static class HostableWebCore
    {
        private static bool _isActivated;

        private delegate int FnWebCoreActivate([In, MarshalAs(UnmanagedType.LPWStr)]string appHostConfig, [In, MarshalAs(UnmanagedType.LPWStr)]string rootWebConfig, [In, MarshalAs(UnmanagedType.LPWStr)]string instanceName);
        private delegate int FnWebCoreShutdown(bool immediate);

        private static FnWebCoreActivate WebCoreActivate;
        private static FnWebCoreShutdown WebCoreShutdown;

        static HostableWebCore()
        {
            const string HWCPath = @"%windir%\system32\inetsrv\hwebcore.dll";
            IntPtr hwc = NativeMethods.LoadLibrary(Environment.ExpandEnvironmentVariables(HWCPath));

            IntPtr procaddr = NativeMethods.GetProcAddress(hwc, "WebCoreActivate");
            WebCoreActivate = (FnWebCoreActivate)Marshal.GetDelegateForFunctionPointer(procaddr, typeof(FnWebCoreActivate));

            procaddr = NativeMethods.GetProcAddress(hwc, "WebCoreShutdown");
            WebCoreShutdown = (FnWebCoreShutdown)Marshal.GetDelegateForFunctionPointer(procaddr, typeof(FnWebCoreShutdown));
        }
        public static bool IsActivated
        {
            get
            {
                return _isActivated;
            }
        }

        public static void Activate(string appHostConfig, string rootWebConfig, string instanceName)
        {
            int result = WebCoreActivate(appHostConfig, rootWebConfig, instanceName);
            if (result != 0)
            {
                Marshal.ThrowExceptionForHR(result);
            }

            _isActivated = true;
        }

        public static void Shutdown(bool immediate)
        {
            if (_isActivated)
            {
                WebCoreShutdown(immediate);
                _isActivated = false;
            }
        }

    }
}
