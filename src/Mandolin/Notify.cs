namespace Mandolin
{
    using System;
    
    public static class Notify
    {
        private const string Prefix = ""; //"Mandolin: ";

        public static void WriteLine(object value)
        {
            Console.WriteLine(Prefix + (value ?? "").ToString());
        }

        public static void WriteLine(string format, object arg0)
        {
            Console.WriteLine(Prefix + format, arg0);
        }

        public static void WriteLine(string format, object arg0, object arg1)
        {
            Console.WriteLine(Prefix + format, arg0, arg1);
        }

        public static void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            Console.WriteLine(Prefix + format, arg0, arg1, arg2);
        }

        public static void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(Prefix + format, args);
        }
    }
}
