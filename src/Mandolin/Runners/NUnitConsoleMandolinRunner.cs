namespace Mandolin.Runners
{
    using System;
    using NUnit.ConsoleRunner;

    public class NUnitConsoleMandolinRunner
        : IMandolinRunner
    {
        private readonly INUnitConsoleFacade nunitConsole;
        private readonly ISlicer slicer;
        private readonly string[] args;
        private ConsoleOptions options;

        public NUnitConsoleMandolinRunner(INUnitConsoleFacade nunitConsole, ISlicer slicer, params string[] args)
        {
            this.nunitConsole = nunitConsole;
            this.slicer = slicer;
            this.args = args;
            this.options = new ConsoleOptions(args);
        }

        public string Run(int wantedSlice, int totalSlices)
        {
            var arguments = this.CreateArguments(wantedSlice, totalSlices);
            var result = this.nunitConsole.Run(arguments);

            return "Failed tests: " + result;
        }

        private string[] CreateArguments(int wantedSlice, int totalSlices)
        {
            return this.args;
        }
    }
}