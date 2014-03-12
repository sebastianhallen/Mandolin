namespace Mandolin.Runners
{
    using NUnit.ConsoleRunner;

    public class NUnitConsoleFacade
        : INUnitConsoleFacade
    {
        public int Run(string[] args)
        {
            return Runner.Main(args);
        }
    }
}