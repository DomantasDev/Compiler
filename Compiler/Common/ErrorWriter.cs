using System;
using System.Linq;

namespace Common
{
    public static class ErrorWriter
    {
        private static string _file;
        private static string template => $"ERROR: {_file}";
        public static int ErrorCount { get; private set; }

        public static string File
        {
            get => _file;
            set => _file = value.Split('/').Last();
        }

        public static string RaiseError(this string message, int line)
        {
            var error = $"{template}: {line}. {message}";
            Console.WriteLine(error);
            ErrorCount++;
            return error;
        }

        public static string RaiseError(this string message)
        {
            var error = $"{template}. {message}";
            Console.WriteLine(error);
            ErrorCount++;
            return error;
        }

        public static string UnexpectedError(this string message, int line)
        {
            var error = $"{template}: {line}. {message.Length}. BAD STUFF";
            Console.WriteLine(error);
            ErrorCount++;
            return error;
        }
    }
}
