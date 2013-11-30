namespace Mandolin.Runners
{
    using System;
    using NUnit.ConsoleRunner;

    public class NUnitConsoleMandolinRunner
        : IMandolinRunner
    {
        private readonly string[] args;
        private ConsoleOptions options;

        public NUnitConsoleMandolinRunner(params string[] args)
        {
            this.args = args;
            this.options = new ConsoleOptions(args);
        }

        public string Run(int wantedSlice, int totalSlices)
        {
            Runner.Main(this.CreateArguments(wantedSlice, totalSlices));
            return "";
        }

        private string[] CreateArguments(int wantedSlice, int totalSlices)
        {
            return this.args;
        }
    }
}