using System;
using System.Linq;

namespace Common
{
    public static class ErrorWriter
    {
        private static string _file;

        public static string File
        {
            get => _file;
            set => _file = value.Split('/').Last();
        }

        public static void RaiseError(this string message, int line)
        {
            Console.WriteLine($"ERROR: {File}: {line}. {message}");
        }

        public static void RaiseError(this string message)
        {
            Console.WriteLine($"ERROR: {File}. {message}");
        }
    }
}
