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
            string[] slicedArgs;
            int result;
            using (var runlist = this.CreateArguments(wantedSlice, totalSlices, out slicedArgs))
            {
                 result = this.nunitConsole.Run(slicedArgs);
            }
            
            return new StringBuilder()
                .AppendLine("Transformed arguments: ")
                .AppendLine(string.Join(" ", this.args))
                .AppendLine(string.Join(" ", slicedArgs))
                .AppendLine("Exit code: " + result)
                .ToString();
        }

        private IRunListFile CreateArguments(int wantedSlice, int totalSlices, out string[] slicedArgs)
        {
            return this.argumentSlicer.Slice(this.args, wantedSlice, totalSlices, out slicedArgs);
        }
    }
}