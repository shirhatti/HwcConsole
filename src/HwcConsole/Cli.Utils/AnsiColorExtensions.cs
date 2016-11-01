using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HwcConsole.Cli.Utils
{
    public static class AnsiColorExtensions
    {
        public static string Red(this string text)
        {
            return "\x1B[31m" + text + "\x1B[39m";
        }

        public static string Yellow(this string text)
        {
            return "\x1B[33m" + text + "\x1B[39m";
        }
    }
}