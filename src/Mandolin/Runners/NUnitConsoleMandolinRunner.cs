namespace Mandolin.Runners
{
    using System.Text;

    public class NUnitConsoleMandolinRunner
        : IMandolinRunner
    {
        private readonly INUnitConsoleFacade nunitConsole;
        private readonly NUnitConsoleCommandLineArgumentSlicer argumentSlicer;
        private readonly string[] args;

        public NUnitConsoleMandolinRunner(INUnitConsoleFacade nunitConsole, NUnitConsoleCommandLineArgumentSlicer argumentSlicer, params string[] args)
        {
            this.nunitConsole = nunitConsole;
            this.argumentSlicer = argumentSlicer;
            this.args = args;
        }

        public string Run(int wantedSlice, int totalSlices)
        {
            var arguments = this.CreateArguments(wantedSlice, totalSlices);
            var result = this.nunitConsole.Run(arguments);

            return new StringBuilder()
                .AppendLine("Transformed arguments: ")
                .AppendLine(string.Join(" ", this.args))
                .AppendLine(string.Join(" ", arguments))
                .AppendLine("Exit code: " + result)
                .ToString();
        }

        private string[] CreateArguments(int wantedSlice, int totalSlices)
        {
            return this.argumentSlicer.Slice(this.args, wantedSlice, totalSlices);
        }
    }
}