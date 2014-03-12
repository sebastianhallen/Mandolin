namespace Mandolin.Runners
{
    using System;
    using NUnit.ConsoleRunner;

    public class NUnitConsoleFacade
        : INUnitConsoleFacade
    {
        public int Run(string[] args)
        {
            try
            {
                return Runner.Main(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Crashed when running tests: " + Environment.NewLine + ex);
                return -1;
            }
        }
    }
}