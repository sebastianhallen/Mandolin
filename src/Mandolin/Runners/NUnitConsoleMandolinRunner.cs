namespace Mandolin.Runners
{
    using System;
    using NUnit.ConsoleRunner;

    public class NUnitConsoleMandolinRunner
        : IMandolinRunner
    {
        private ConsoleOptions options;

        public NUnitConsoleMandolinRunner(params string[] args)
        {
            this.options = new ConsoleOptions(args);
        }

        public string Run(int wantedSlice, int totalSlices)
        {
            Runner.Main(this.CreateArguments(wantedSlice, totalSlices));
            return "";
        }

        private string[] CreateArguments(int wantedSlice, int totalSlices)
        {
            throw new NotImplementedException();
        }
    }
}