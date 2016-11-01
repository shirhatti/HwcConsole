using HwcConsole.Cli.Utils;
using HwcConsole.Internal;
using System;

namespace HwcConsole
{
    internal class Server :IDisposable
    {
        private readonly string _applicationHostConfigPath;

        public Server(string applicationHostConfigPath)
        {
            _applicationHostConfigPath = applicationHostConfigPath;
        }

        ~Server()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if(disposing)
            {
                GC.SuppressFinalize(this);
            }
        }

        public void Start()
        {
            // Start Web Server
            if (!HostableWebCore.IsActivated)
            {
                HostableWebCore.Activate(_applicationHostConfigPath, null, Guid.NewGuid().ToString());
            }
        }

        public void Stop()
        {
            if (HostableWebCore.IsActivated)
            {
                HostableWebCore.Shutdown(false);
            }
        }
    }
}