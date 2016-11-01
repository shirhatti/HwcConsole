using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HwcConsole.Cli.Utils
{
    // Stupid-simple console manager
    public class Reporter
    {
        private static readonly Reporter NullReporter = new Reporter(console: null);
        private static object _lock = new object();

        private readonly AnsiConsole _console;

        private Reporter(AnsiConsole console)
        {
            _console = console;
        }

        public static Reporter Output { get; } = new Reporter(AnsiConsole.GetOutput());
        public static Reporter Error { get; } = new Reporter(AnsiConsole.GetError());

        public void WriteLine(string message)
        {
            lock (_lock)
            {
                _console?.WriteLine(message);
            }
        }

        public void WriteLine()
        {
            lock (_lock)
            {
                _console?.Writer?.WriteLine();
            }
        }

        public void Write(string message)
        {
            lock (_lock)
            {
                _console?.Write(message);
            }
        }
    }
}
